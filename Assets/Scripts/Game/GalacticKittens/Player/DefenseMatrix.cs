using Game.GalacticKittens.Manager;
using UnityEngine;

namespace Game.GalacticKittens.Player
{
    /// <summary>
    /// 防御护盾 TODO unity对象操作
    /// </summary>
    public class DefenseMatrix : MonoBehaviour
    {
        public AudioClip m_shieldClip;
        public bool isShieldActive { get; private set; } = false;

        private SpriteRenderer m_spriteRenderer;
        private CircleCollider2D m_circleCollider2D;

        private void Start()
        {
            m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            m_circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        }


        public void TurnOnShield()
        {
            isShieldActive = true;

            m_spriteRenderer.enabled = true;
            m_circleCollider2D.enabled = true;
            GalacticKittensAudioManager.Instance.PlaySoundEffect(m_shieldClip);
        }


        public void TurnOffShield()
        {
            isShieldActive = false;

            m_spriteRenderer.enabled = false;
            m_circleCollider2D.enabled = false;
        }
    }
}