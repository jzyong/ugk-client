using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GalacticKittens.Selection
{

    
    /// <summary>
    /// 角色选择容器 TODO 按钮事件
    /// </summary>
    [Serializable]
    public class CharacterContainer
    {
        public Image imageContainer;                    // The image of the character container
        public TextMeshProUGUI nameContainer;           // Character name container
        public GameObject border;                       // The border of the character container when not ready
        public GameObject borderReady;                  // The border of the character container when ready
        public GameObject borderClient;                 // Client border of the character container
        public Image playerIcon;                        // The background icon of the player (p1, p2)
        public GameObject waitingText;                  // The waiting text on the container were no client connected
        public GameObject backgroundShip;               // The background of the ship when not ready
        public Image backgroundShipImage;               // The image of the ship when not ready
        public GameObject backgroundShipReady;          // The background of the ship when ready
        public Image backgroundShipReadyImage;          // The image of the ship when ready
        public GameObject backgroundClientShipReady;    // Client background of the ship when ready
        public Image backgroundClientShipReadyImage;    // Client image of the ship when ready

        private GalacticKittensPlayerInfo _playerInfo;
        
        
        public void Init(GalacticKittensPlayerInfo playerInfo)
        {
            this._playerInfo = playerInfo;
        }
        
    }
    
    
   
    
}