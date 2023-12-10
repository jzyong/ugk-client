using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.GalacticKittens.Room.Enemy
{
    public class Meteor : MonoBehaviour
    {

        [SerializeField]
        private GameObject m_vfxExplosion;

        [SerializeField]
        private Sprite[] m_meteors;

        [SerializeField]
        private GameObject m_meteorSprite;

        [SerializeField]
        private SpriteRenderer m_spriteRenderer;

        [SerializeField]
        float m_hitEffectDuration = 0.2f;


        private void Start()
        {
            // Randomly select the sprite to use 
            m_spriteRenderer.GetComponent<SpriteRenderer>().sprite =
                m_meteors[Random.Range(0, m_meteors.Length)];
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            // if (!IsServer)
            //     return;
            //
            // if (collider.TryGetComponent(out IDamagable damagable))
            // {
            //     // Hit the object that collide with me
            //     damagable.Hit(m_damage);
            //
            //     // Hit me too!
            //     Hit(m_damage);
            // }
        }

        public IEnumerator HitEffect()
        {
            bool active = false;
            float timer = 0f;
            while (timer < m_hitEffectDuration)
            {
                active = !active;
                m_spriteRenderer.material.SetInt("_Hit", active ? 1 : 0);

                yield return new WaitForEndOfFrame();

                timer += Time.deltaTime;
            }

            m_spriteRenderer.material.SetInt("_Hit", 0);
        }

        // public void Hit(int damage)
        // {
        //     m_health -= damage;
        //     if (m_health <= 0)
        //     {
        //         PowerUpSpawnController.instance.OnPowerUpSpawn(transform.position);
        //         NetworkObjectSpawner.SpawnNewNetworkObject(m_vfxExplosion, transform.position);
        //
        //         NetworkObjectDespawner.DespawnNetworkObject(NetworkObject);
        //     }
        //
        //     StopCoroutine(HitEffect());
        //     StartCoroutine(HitEffect());
        // }
    }
}