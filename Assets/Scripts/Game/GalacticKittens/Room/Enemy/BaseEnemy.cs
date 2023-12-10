using System;
using System.Collections;
using Game.GalacticKittens.Manager;
using Game.GalacticKittens.Utility;
using Network.Sync;
using UnityEngine;

namespace Game.GalacticKittens.Room.Enemy
{
    /// <summary>
    /// 敌人基类
    /// </summary>
    public class BaseEnemy : MonoBehaviour,IObjectDestory
    {
         protected SpriteRenderer m_sprite;

        [SerializeField]
        protected float m_hitEffectDuration=0.2f;

        [SerializeField] protected GameObject m_vfxExplosion;

        private void Start()
        {
            m_sprite = GetComponent<SpriteRenderer>();
        }

        public void Despawn(GalacticKittensObjectDieResponse response)
        {
            var blast = Instantiate(m_vfxExplosion, transform);
            blast.GetComponent<ParticleSystem>().Play();
            
            var explosion = Instantiate(m_vfxExplosion, transform.position,Quaternion.identity,GalacticKittensRoomManager.Instance.transform);
            explosion.GetComponent<ParticleSystem>().Play();
            SyncManager.Instance.RemoveSyncObject(GetComponent<SnapTransform>().Id);
            Destroy(gameObject);
        }
        
        private IEnumerator HitEffect()
        {
            bool active = false;
            float timer = 0f;

            while (timer < m_hitEffectDuration)
            {
                active = !active;
                m_sprite.material.SetInt("_Hit", active ? 1 : 0);
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
            }

            m_sprite.material.SetInt("_Hit", 0);
        }

        public void PlayerHitEffect()
        {
            StopCoroutine(HitEffect());
            StartCoroutine(HitEffect());
        }
        
    }
}