using System;
using System.Collections;
using Game.GalacticKittens.Manager;
using Game.GalacticKittens.Player;
using Game.GalacticKittens.Utility;
using Network.Sync;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.GalacticKittens.Room.Enemy
{
    public class Meteor : MonoBehaviour, IObjectDestory
    {
        [SerializeField] private GameObject m_vfxExplosion;

        [SerializeField] private Sprite[] m_meteors;

        [SerializeField] private GameObject m_meteorSprite;

        [SerializeField] private SpriteRenderer m_spriteRenderer;

        [SerializeField] float m_hitEffectDuration = 0.2f;


        private void Start()
        {
            // Randomly select the sprite to use 
            m_spriteRenderer.GetComponent<SpriteRenderer>().sprite =
                m_meteors[Random.Range(0, m_meteors.Length)];
        }


        public void Despawn(GalacticKittensObjectDieResponse response)
        {
            var sceneObject = GalacticKittensRoomManager.Instance.GetSceneObject(response.KillerId);
            if (sceneObject != null)
            {
                var spaceship = sceneObject.GetComponent<Spaceship>();
                if (spaceship != null)
                {
                    spaceship.PlayHitEffect();
                }
            }

            var explosion = Instantiate(m_vfxExplosion, transform.position, Quaternion.identity,
                GalacticKittensRoomManager.Instance.transform);
            explosion.GetComponent<ParticleSystem>().Play();
            SyncManager.Instance.RemoveSyncObject(GetComponent<PredictionTransform>().Id);
            Destroy(gameObject);
        }
    }
}