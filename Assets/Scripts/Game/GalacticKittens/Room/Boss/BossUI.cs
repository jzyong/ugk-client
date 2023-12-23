using Game.GalacticKittens.Utility;
using UnityEngine;
using UnityEngine.UI;


namespace Game.GalacticKittens.Room.Boss
{
    public class BossUI : MonoBehaviour
    {
        [SerializeField] Slider m_healthSlider;

        [SerializeField] Image m_healthImage;

        [SerializeField] HealthColor m_healthColor;

        uint maxHealth;
        private uint nowHealth;


        /// <summary>
        /// 初始设置血条
        /// </summary>
        /// <param name="currentHealth"></param>
        public void SetHealth(uint currentHealth)
        {
            maxHealth = currentHealth;
            nowHealth = maxHealth;
            UpdateHealth(0);
            gameObject.SetActive(true);
        }

        public void UpdateHealth(uint changeHealth)
        {
            nowHealth -= changeHealth;
            float convertedHealth = (float)nowHealth / maxHealth;
            m_healthSlider.value = convertedHealth;
            m_healthImage.color = m_healthColor.GetHealthColor(convertedHealth);
        }
    }
}