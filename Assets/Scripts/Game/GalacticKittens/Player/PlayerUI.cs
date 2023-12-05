using System;
using Game.GalacticKittens.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GalacticKittens.Player
{
    /// <summary>
    /// 玩家UI TODO 制作prefab，消息定义
    /// </summary>
    public class PlayerUI : MonoBehaviour
    {
        // Struct for a better organization of the health UI 
        [Serializable]
        public struct HealthUI
        {
            public GameObject healthUI;
            public Image playerIconImage;
            public TextMeshProUGUI playerIdText;
            public Slider healthSlider;
            public Image healthImage;
            public HealthColor healthColor;
            public GameObject[] powerUp;
        }

        // Struct for a better organization of the death UI
        [Serializable]
        public struct DeathUI
        {
            public GameObject deathUI;
            public Image playerIconDeathImage;
            public TextMeshProUGUI playerIdDeathText;
        }
        
        [SerializeField]
        HealthUI m_healthUI;                // A struct for all the data relate to the health UI

        [SerializeField]
        DeathUI m_deathUI;                  // A struct for all the data relate to the death UI

        [Header("Set in runtime")]
        public int maxHealth;               // Max health the player has, use for the conversion to the
        // slider and the coloring of the bar
    
        /// <summary>
        /// 更新血条 TODO 添加血条改变消息
        /// </summary>
        /// <param name="currentHealth"></param>
        void UpdateHealthResponse(float currentHealth)
        {
        
            m_healthUI.healthSlider.value = currentHealth;
            m_healthUI.healthImage.color = m_healthUI.healthColor.GetHealthColor(currentHealth);

            if (currentHealth <= 0f)
            {
                // Turn off lifeUI
                m_healthUI.healthUI.SetActive(false);

                // Turn on deathUI
                m_deathUI.deathUI.SetActive(true);
            }
        }
     
        // TODO: check if the initial values are set on client
        // Set the initial values of the UI
        public void SetUI(
            int playerId,
            Sprite playerIcon,
            Sprite playerDeathIcon,
            int maxHealth,
            Color color)
        {
            m_healthUI.playerIconImage.sprite = playerIcon;
            m_healthUI.playerIdText.color = color;
            m_healthUI.playerIdText.text = $"P{(playerId + 1)}";

            m_deathUI.playerIdDeathText.color = color;
            m_deathUI.playerIconDeathImage.sprite = playerDeathIcon;

            this.maxHealth = maxHealth;
            m_healthUI.healthImage.color = m_healthUI.healthColor.normalColor;

            // Turn on my lifeUI
            m_healthUI.healthUI.SetActive(true);

            // Safety -> inactive in scene
            m_deathUI.deathUI.SetActive(false);
        }

        // Update the UI health 
        public void UpdateHealth(int currentHealth)
        {
           

            // Don't let health to go below 
            currentHealth = currentHealth < 0 ? 0 : currentHealth;

            float convertedHealth = (float)currentHealth / (float)maxHealth;
            m_healthUI.healthSlider.value = convertedHealth;
            m_healthUI.healthImage.color = m_healthUI.healthColor.GetHealthColor(convertedHealth);

            if (currentHealth <= 0)
            {
                // Turn off lifeUI
                m_healthUI.healthUI.SetActive(false);

                // Turn on deathUI
                m_deathUI.deathUI.SetActive(true);
            }

            UpdateHealthResponse(convertedHealth);
        }

        // Activate/deactivate the power up icons base on the index pass
        public void UpdatePowerUp(int index, bool hasSpecial)
        {
           

            m_healthUI.powerUp[index - 1].SetActive(hasSpecial);

            UpdatePowerUpResponse(index, hasSpecial);
        }
        
        /// <summary>
        /// 更新能量值 TODO 添加能量，血条改变消息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hasSpecial"></param>
        void UpdatePowerUpResponse(int index, bool hasSpecial)
        {
           

            m_healthUI.powerUp[index - 1].SetActive(hasSpecial);
        }
    }
}