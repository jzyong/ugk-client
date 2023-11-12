using System;
using Common;
using Common.Tools;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
* Singleton to control the changes on the char sprites and the flow of the scene
*/


namespace Game.GalacticKittens.Selection
{
    /// <summary>
    /// 角色选择管理
    /// TODO 场景入场退出渐变,其他UI界面操作，显示需要调整，背景音乐，切换角色、确认、取消音效，选择角色需要高速服务器广播
    /// </summary>
    public class CharacterSelectionManager : Singleton<CharacterSelectionManager>
    {
        public CharacterDataSO[] charactersData;

        [SerializeField] CharacterContainer[] m_charactersContainers;

        [SerializeField] Button m_readyButton;

        [SerializeField] Button m_cancelButton;

        [SerializeField] float m_timeToStartGame;


        [SerializeField] Color m_clientColor;

        [SerializeField] Color m_playerColor;

        [Header("Audio clips")] [SerializeField]
        AudioClip m_confirmClip;

        [SerializeField] AudioClip m_cancelClip;
        [SerializeField] private AudioClip _changedCharacterClip;

        bool m_isTimerOn;
        float m_timer;

        /// <summary>
        /// 角色确认颜色
        /// </summary>
        private readonly Color k_selectedColor = new Color32(74, 74, 74, 255);
        private readonly Color k_unselectedColor = new Color32(255, 255, 255, 255);

        //自己选择的角色索引
        private int charachterIndex = 0;

        //玩家自己位置索引
        private int playerIndex = 0;

        //玩家数
        private int playerCount;

        void Start()
        {
            m_timer = m_timeToStartGame;
            m_readyButton.onClick.AddListener(PlayerReady);
            m_cancelButton.onClick.AddListener(PlayerNotReady);
        }

        void Update()
        {
            SwitchCharacter();
            QuitRoom();
            ConfirmOrCancel();
        }


        /// <summary>
        /// 选择角色
        /// </summary>
        private void SwitchCharacter()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ChangeCharacterSelection(-1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                ChangeCharacterSelection(1);
            }
        }

        /// <summary>
        /// 退出房间
        /// </summary>
        private void QuitRoom()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                NetworkManager.Singleton.Send(MID.GalacticKittensQuitRoomReq, new GalacticKittensQuitRoomRequest());
                DestroyImmediate(GalacticKittensAudioManager.Instance.gameObject);
                SceneManager.LoadScene("Lobby");
            }
        }

        private void ChangeCharacterSelection(int value)
        {
            charachterIndex += value;
            if (charachterIndex >= charactersData.Length)
            {
                charachterIndex = 0;
            }
            else if (charachterIndex < 0)
            {
                charachterIndex = charactersData.Length - 1;
            }

            GalacticKittensAudioManager.Instance.PlaySoundEffect(_changedCharacterClip);
            // 告知服务器角色改变
            var selectCharacterRequest = new GalacticKittenSelectCharacterRequest()
            {
                CharacterId = charachterIndex
            };
            NetworkManager.Singleton.Send(MID.GalacticKittenSelectCharacterReq, selectCharacterRequest);
            SetPlayer(playerIndex, charachterIndex, true);
        }

        private void ConfirmOrCancel()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_readyButton.gameObject.activeSelf)
                {
                    PlayerReady(DataManager.Singleton.PlayerInfo.PlayerId,playerIndex,charachterIndex);
                }
                else
                {
                    PlayerNotReady(DataManager.Singleton.PlayerInfo.PlayerId,playerIndex,charachterIndex);
                }
            }
        }


        private void OnEnable()
        {
            MessageEventManager.Singleton.AddEvent<GalacticKittensRoomInfoResponse>(
                MessageEvent.GalacticKittensRoomInfo, RoomInfoRes);
        }

        void OnDisable()
        {
            MessageEventManager.Singleton.RemoveEvent<GalacticKittensRoomInfoResponse>(
                MessageEvent.GalacticKittensRoomInfo, RoomInfoRes);
        }


        /// <summary>
        /// 清除选择
        /// </summary>
        /// <param name="playerId"></param>
        void ClearPlayer(int playerId)
        {
            m_charactersContainers[playerId].imageContainer.sprite = null;
            m_charactersContainers[playerId].imageContainer.color = new Color(1f, 1f, 1f, 0f);
            m_charactersContainers[playerId].nameContainer.text = "";
            m_charactersContainers[playerId].border.SetActive(true);
            m_charactersContainers[playerId].borderClient.SetActive(false);
            m_charactersContainers[playerId].borderReady.SetActive(false);
            m_charactersContainers[playerId].playerIcon.gameObject.SetActive(false);
            m_charactersContainers[playerId].playerIcon.color = m_playerColor;
            m_charactersContainers[playerId].backgroundShip.SetActive(false);
            m_charactersContainers[playerId].backgroundShipReady.SetActive(false);
            m_charactersContainers[playerId].backgroundClientShipReady.SetActive(false);
            m_charactersContainers[playerId].waitingText.SetActive(true);
        }


        // /// <summary>
        // /// 设置角色颜色
        // /// </summary>
        // /// <param name="playerId"></param>
        // /// <param name="characterSelected"></param>
        // public void SetCharacterColor(int playerId, int characterSelected)
        // {
        //     if (charactersData[characterSelected].isSelected)
        //     {
        //         m_charactersContainers[playerId].imageContainer.color = k_selectedColor;
        //         m_charactersContainers[playerId].nameContainer.color = k_selectedColor;
        //     }
        //     else
        //     {
        //         m_charactersContainers[playerId].imageContainer.color = Color.white;
        //         m_charactersContainers[playerId].nameContainer.color = Color.white;
        //     }
        // }

        /// <summary>
        /// 设置UI 
        /// </summary>
        /// <param name="playerIndex">角色位置下标</param>
        /// <param name="characterSelected"> 角色配置数据下标</param>
        public void SetCharacterUI(int playerIndex, int characterSelected)
        {
            m_charactersContainers[playerIndex].imageContainer.sprite =
                charactersData[characterSelected].characterSprite;

            m_charactersContainers[playerIndex].backgroundShipImage.sprite =
                charactersData[characterSelected].characterShipSprite;

            m_charactersContainers[playerIndex].backgroundShipReadyImage.sprite =
                charactersData[characterSelected].characterShipSprite;

            m_charactersContainers[playerIndex].backgroundClientShipReadyImage.sprite =
                charactersData[characterSelected].characterShipSprite;

            m_charactersContainers[playerIndex].nameContainer.text =
                charactersData[characterSelected].characterName;

            
            // SetCharacterColor(playerIndex, characterSelected);
        }

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="playerIndex">玩家位置下标</param>
        /// <param name="characterSelected">选择的角色</param>
        /// <param name="isSelf"></param>
        public void SetPlayer(int playerIndex, int characterSelected, bool isSelf)
        {
            SetCharacterUI(playerIndex, characterSelected);
            m_charactersContainers[playerIndex].playerIcon.gameObject.SetActive(true);
            if (isSelf)
            {
                m_charactersContainers[playerIndex].borderClient.SetActive(true);
                m_charactersContainers[playerIndex].border.SetActive(false);
                m_charactersContainers[playerIndex].borderReady.SetActive(false);
                m_charactersContainers[playerIndex].playerIcon.color = m_clientColor;
            }
            else
            {
                m_charactersContainers[playerIndex].border.SetActive(true);
                m_charactersContainers[playerIndex].borderReady.SetActive(false);
                m_charactersContainers[playerIndex].borderClient.SetActive(false);
                m_charactersContainers[playerIndex].playerIcon.color = m_playerColor;
            }

            m_charactersContainers[playerIndex].backgroundShip.SetActive(true);
            m_charactersContainers[playerIndex].waitingText.SetActive(false);
        }


        /// <summary>
        /// 收到房间改变消息
        /// </summary>
        private void RoomInfoRes(GalacticKittensRoomInfoResponse response)
        {
            //一个玩家修改，所有数据都从新设置了，人数少为了简便，暂时这样
            Debug.Log($"房间信息：{response}");
            int i = 0;
            foreach (var playerInfo in response.Room.Player)
            {
                m_charactersContainers[i].Init(playerInfo);
                //自己
                if (playerInfo.PlayerId == DataManager.Singleton.PlayerInfo.PlayerId)
                {
                    playerIndex = i;
                    SetPlayer(i, playerInfo.CharacterId, true);
                }
                else //别人
                {
                    SetPlayer(i, playerInfo.CharacterId, false);
                    if (playerInfo.Prepare)
                    {
                        PlayerReady(playerInfo.PlayerId,i,playerInfo.CharacterId);
                    }
                    else
                    {
                        PlayerNotReady(playerInfo.PlayerId,i,playerInfo.CharacterId);
                    }
                }

                if (playerInfo.Prepare)
                {
                    m_charactersContainers[i].imageContainer.color=k_selectedColor;
                    m_charactersContainers[i].nameContainer.color=k_selectedColor;
                }
                else
                {
                    m_charactersContainers[i].imageContainer.color=k_unselectedColor;
                    m_charactersContainers[i].nameContainer.color=k_unselectedColor;
                }

                i++;
            }

            //玩家数减少，有退出
            if (response.Room.Player.Count < playerCount)
            {
                for (int j = i; j < 4; j++)
                {
                    ClearPlayer(j);
                }
            }


            playerCount = response.Room.Player.Count;
        }



        void PlayerReady()
        {
            PlayerReady(DataManager.Singleton.PlayerInfo.PlayerId,playerIndex,charachterIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerIndex"></param>
        /// <param name="characterSelected"></param>
        void PlayerReady(long playerId, int playerIndex, int characterSelected)
        {
          
            charactersData[characterSelected].isSelected = true;
            charactersData[characterSelected].playerId = playerIndex;
            charactersData[characterSelected].clientId = playerId;

            //自己
            if (playerId == DataManager.Singleton.PlayerInfo.PlayerId)
            {
                m_readyButton.gameObject.SetActive(false);
                m_cancelButton.gameObject.SetActive(true);
                m_charactersContainers[playerIndex].backgroundClientShipReady.SetActive(true);
                m_charactersContainers[playerIndex].backgroundShip.SetActive(false);
                var request = new GalacticKittensPrepareRequest()
                {
                    Prepare = true
                };
                NetworkManager.Singleton.Send(MID.GalacticKittensPrepareReq, request);
                GalacticKittensAudioManager.Instance.PlaySoundEffect(m_confirmClip);
                NetworkTimeInterpolation.InitTimeInterpolation();
            }
            else
            {
                m_charactersContainers[playerIndex].border.SetActive(false);
                m_charactersContainers[playerIndex].borderReady.SetActive(true);
                m_charactersContainers[playerIndex].backgroundShip.SetActive(false);
                m_charactersContainers[playerIndex].backgroundShipReady.SetActive(true);
            }
            // m_charactersContainers[playerIndex].imageContainer.color=k_selectedColor;
            // m_charactersContainers[playerIndex].nameContainer.color=k_selectedColor;
           
        }

        void PlayerNotReady()
        {
            PlayerNotReady(DataManager.Singleton.PlayerInfo.PlayerId,playerIndex,charachterIndex);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="playerId"></param>
        /// <param name="characterSelected"></param>
        void PlayerNotReady(long playerId, int playerIndex, int characterSelected)
        {
           
            charactersData[characterSelected].isSelected = false;
            charactersData[characterSelected].clientId = 0L;
            charactersData[characterSelected].playerId = -1;

            if (playerId == DataManager.Singleton.PlayerInfo.PlayerId)
            {
                m_charactersContainers[playerIndex].borderClient.SetActive(true);
                m_charactersContainers[playerIndex].backgroundClientShipReady.SetActive(false);
                m_charactersContainers[playerIndex].backgroundShip.SetActive(true);
                m_readyButton.gameObject.SetActive(true);
                m_cancelButton.gameObject.SetActive(false);
                var request = new GalacticKittensPrepareRequest()
                {
                    Prepare = false
                };
                NetworkManager.Singleton.Send(MID.GalacticKittensPrepareReq, request);
                GalacticKittensAudioManager.Instance.PlaySoundEffect(m_cancelClip);
            }
            else
            {
                m_charactersContainers[playerIndex].border.SetActive(true);
                m_charactersContainers[playerIndex].borderReady.SetActive(false);
                m_charactersContainers[playerIndex].borderClient.SetActive(false);
                m_charactersContainers[playerIndex].backgroundShip.SetActive(true);
                m_charactersContainers[playerIndex].backgroundShipReady.SetActive(false);
            }
            // m_charactersContainers[playerIndex].imageContainer.color=k_unselectedColor;
            // m_charactersContainers[playerIndex].nameContainer.color=k_unselectedColor;
           
        }
    }
}