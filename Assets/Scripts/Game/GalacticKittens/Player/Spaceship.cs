using System.Collections;
using Game.GalacticKittens.Manager;
using Network;
using Network.Sync;
using UnityEngine;

namespace Game.GalacticKittens.Player
{
    /// <summary>
    /// 飞船
    /// </summary>
    public class Spaceship : MonoBehaviour
    {
        private SnapTransform _snapTransform;

        [SerializeField] [Tooltip("护盾")] private DefenseMatrix _defenseMatrix;
        public PlayerUI playerUI;

        [Header("AudioClips")] [SerializeField]
        AudioClip m_hitClip;
        
        [SerializeField]
        float m_hitEffectDuration;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public CharacterDataSO _characterDataSo;
        
        const string k_hitEffect = "_Hit";

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
            //
            // //使用护盾 TODO 需要判断护盾是否充足 ，暂时屏蔽，需要设置引用
            // if (!_defenseMatrix.isShieldActive && (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.LeftShift)))
            // {
            //     UseShieldReq();
            // }
            //
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
            // playerUI.UpdatePowerUp(); TODO 更新UI
        }
    }
}