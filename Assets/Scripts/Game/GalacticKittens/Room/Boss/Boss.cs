using System.Collections;
using Game.GalacticKittens.Room.Enemy;
using UnityEngine;

namespace Game.GalacticKittens.Room.Boss
{
    
    public class Boss : BaseEnemy
    {
        
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private SpriteRenderer[] m_sprites;
        
        private bool m_isInmmune;
        
        private const string k_effectHit = "_Hit";
        private const string k_animHit = "hit";
        
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