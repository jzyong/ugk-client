using System;
using Game.GalacticKittens.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GalacticKittens.Player
{
    /// <summary>
    /// 玩家UI 
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

        [SerializeField] HealthUI m_healthUI; // A struct for all the data relate to the health UI

        [SerializeField] DeathUI m_deathUI; // A struct for all the data relate to the death UI

        [Header("Set in runtime")] public uint maxHealth=30; // Max health the player has, use for the conversion to the




        // TODO: check if the initial values are set on client
        // Set the initial values of the UI
        public void SetUI(Spaceship spaceship)
        {
            var characterDataSo = spaceship._characterDataSo;
            Color color = new Color(characterDataSo.darkColor.r, characterDataSo.darkColor.g,
                characterDataSo.darkColor.b, 255);
            m_healthUI.playerIconImage.sprite = characterDataSo.iconSprite;
            m_healthUI.playerIdText.color = color;
            m_healthUI.playerIdText.text = $"P{(spaceship.Id)}";

            m_deathUI.playerIdDeathText.color = color;
            m_deathUI.playerIconDeathImage.sprite = characterDataSo.iconDeathSprite;

            m_healthUI.healthImage.color = m_healthUI.healthColor.normalColor;

            // Turn on my lifeUI
            m_healthUI.healthUI.SetActive(true);

            // Safety -> inactive in scene
            m_deathUI.deathUI.SetActive(false);
            UpdateHealth(maxHealth);
        }

        // Update the UI health 
        public void UpdateHealth(uint currentHealth)
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
        }
        

        // Activate/deactivate the power up icons base on the index pass
        public void UpdatePowerUp(int index, bool hasSpecial)
        {
            m_healthUI.powerUp[index].SetActive(hasSpecial);
        }
    }
}