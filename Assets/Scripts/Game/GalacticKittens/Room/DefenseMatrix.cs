using System.Collections;
using UnityEngine;

namespace Game.GalacticKittens.Room
{
    /// <summary>
    /// 防御护盾
    /// </summary>
    public class DefenseMatrix :MonoBehaviour
    {
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
        }

       
        
        private void TurnOffShield()
        {
            isShieldActive = false;

            m_spriteRenderer.enabled = false;
            m_circleCollider2D.enabled = false;
        }

    }
}