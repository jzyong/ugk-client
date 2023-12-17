using System.Collections;
using System.Collections.Generic;
using Game.GalacticKittens.Manager;
using Game.GalacticKittens.Player;
using Game.GalacticKittens.Room.Enemy;
using Network.Sync;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GalacticKittens.Room.Boss
{
    
    public class Boss : BaseEnemy
    {
        
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private SpriteRenderer[] m_sprites;
        
        [SerializeField]
        int m_maxNumberOfExplosions;

        [SerializeField]
        float m_explosionDuration;

        [SerializeField]
        Transform m_explosionPositionsContainer;

        [SerializeField]
        GameObject m_explosionVfx;

        [SerializeField]
        [Range(1f, 40f)]
        float m_shakeSpeed;

        [SerializeField]
        [Range(0.1f, 2f)]
        float m_shakeAmount;

        List<Transform> explosionPositions = new List<Transform>();
        
        
        private bool m_isInmmune;
        
        private const string k_effectHit = "_Hit";
        private const string k_animHit = "hit";

        public void Start()
        {
            // Add the explosions Positions 
            foreach (Transform transform in m_explosionPositionsContainer)
            {
                explosionPositions.Add(transform);
            }
        }
        IEnumerator Shake()
        {
            float currentPositionx = transform.position.x;
            while (true)
            {
                float shakeValue = Mathf.Sin(Time.time * m_shakeSpeed) * m_shakeAmount;

                transform.position = new Vector2(currentPositionx + shakeValue, transform.position.y);

                yield return new WaitForEndOfFrame();
            }
        }
        
        IEnumerator RunDeath()
        {
            // Show various explosion vfx for some seconds
            int numberOfExplosions = 0;
            float stepDuration = m_explosionDuration / m_maxNumberOfExplosions;

            StartCoroutine(Shake());
            while (numberOfExplosions < m_maxNumberOfExplosions)
            {
                Vector3 randPosition = explosionPositions[Random.Range(0, explosionPositions.Count)].position;
                Instantiate(m_explosionVfx, randPosition, Quaternion.identity, transform);

                yield return new WaitForSeconds(stepDuration);

                numberOfExplosions++;
            }
            StopCoroutine(Shake());

            yield return new WaitForEndOfFrame();
            SyncManager.Instance.RemoveSyncObject(GetComponent<SnapTransform>().Id);
            Destroy(gameObject);
            GalacticKittensRoomManager.Instance.GameFinish();
            
        }
        
        public override void Despawn(GalacticKittensObjectDieResponse response)
        {
            //玩家可能碰撞到boss
            var sceneObject = GalacticKittensRoomManager.Instance.GetSceneObject(response.KillerId);
            if (sceneObject != null)
            {
                var spaceship = sceneObject.GetComponent<Spaceship>();
                if (spaceship != null)
                {
                    spaceship.PlayHitEffect();
                }
            }

            StartCoroutine(RunDeath());
            
        }
        
        
        // The hit effect use in the game
        public IEnumerator HitEffect()
        {
            m_isInmmune = true;
            m_animator.SetBool(k_animHit, true);
            bool active = false;
            float timer = 0f;

            while (timer < m_hitEffectDuration)
            {
                active = !active;
                foreach (var sprite in m_sprites)
                {
                    sprite.material.SetInt(k_effectHit, active ? 1 : 0);
                }
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
            }

            foreach (var sprite in m_sprites)
            {
                sprite.material.SetInt(k_effectHit, 0);
            }

            m_animator.SetBool(k_animHit, false);

            yield return new WaitForSeconds(0.2f);
            m_isInmmune = false;
        }
        
        public override void PlayerHitEffect()
        {
            StopCoroutine(HitEffect());
            StartCoroutine(HitEffect());
        }
        
    }
}