﻿using System.Collections;
using System.Linq;
using Game.GalacticKittens.Manager;
using Game.GalacticKittens.Utility;
using Lobby;
using Network;
using Network.Sync;
using UnityEngine;

namespace Game.GalacticKittens.Player
{
    /// <summary>
    /// 飞船
    /// </summary>
    public class Spaceship : MonoBehaviour, IObjectDestory
    {
        private SnapTransform _snapTransform;

        [SerializeField] [Tooltip("护盾")] private DefenseMatrix _defenseMatrix;

        [HideInInspector] public PlayerUI playerUI;

        [Header("AudioClips")] [SerializeField]
         AudioClip m_hitClip;

        [SerializeField] float m_hitEffectDuration;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] protected GameObject m_vfxExplosion;

        public CharacterDataSO _characterDataSo;

        const string k_hitEffect = "_Hit";

        public GalacticKittensPlayerPropertyResponse.Types.PlayerProperty PlayerProperty { get; set; }

        public long Id { set; get; }


        private void Start()
        {
            _snapTransform = GetComponent<SnapTransform>();
        }

        private void Update()
        {
            if (!_snapTransform.IsOnwer)
            {
                return;
            }

            // 监听按键事件，开火
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireReq();
            }


            //使用护盾
            if (PlayerProperty?.PowerUpCount > 0 && !_defenseMatrix.isShieldActive &&
                (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.LeftShift)))
            {
                UseShieldReq();
            }

            // 退出游戏
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GalacticKittensRoomManager.Instance.QuitToLobby();
                //TODO 需要告知服务器退出游戏
            }
        }

        /// <summary>
        /// 被击中音效
        /// </summary>
        void PlayShipHitSound()
        {
            GalacticKittensAudioManager.Instance.PlaySoundEffect(m_hitClip);
        }


        // Set the hit animation effect  
        private IEnumerator HitEffect()
        {
            bool active = false;
            float timer = 0f;

            while (timer < m_hitEffectDuration)
            {
                active = !active;
                spriteRenderer.material.SetInt(k_hitEffect, active ? 1 : 0);
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
            }

            spriteRenderer.material.SetInt(k_hitEffect, 0);
        }

        public void PlayHitEffect()
        {
            StopCoroutine(HitEffect());
            StartCoroutine(HitEffect());
        }


        /// <summary>
        /// 玩家开火
        /// </summary>
        private void FireReq()
        {
            GalacticKittensFireRequest request = new GalacticKittensFireRequest();
            NetworkManager.Instance.Send(MID.GalacticKittensFireReq, request);
        }

        /// <summary>
        /// 使用护盾
        /// </summary>
        private void UseShieldReq()
        {
            GalacticKittensUseShieldRequest request = new GalacticKittensUseShieldRequest();
            NetworkManager.Instance.Send(MID.GalacticKittensUseShieldReq, request);
            GalacticKittensAudioManager.Instance.PlaySoundEffect(_defenseMatrix.m_shieldClip);
        }

        public void Despawn(GalacticKittensObjectDieResponse response)
        {
            if (m_vfxExplosion != null)
            {
                var explosion = Instantiate(m_vfxExplosion, transform.position, Quaternion.identity,
                    GalacticKittensRoomManager.Instance.transform);
                explosion.GetComponent<ParticleSystem>().Play();
            }

            gameObject.SetActive(false);
            SyncManager.Instance.RemoveSyncObject(GetComponent<SnapTransform>().Id);
            if (DataManager.Instance.GalacticKittens.Spaceships.Values.All((spaceship =>
                    spaceship.gameObject.activeSelf == false)))
            {
                GalacticKittensRoomManager.Instance.GameFinish(false);
            }
        }
    }
}