using System;
using Common;
using Common.Tools;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        [SerializeField]
        CharacterContainer[] m_charactersContainers;

        [SerializeField]
        GameObject m_readyButton;

        [SerializeField]
        GameObject m_cancelButton;

        [SerializeField]
        float m_timeToStartGame;


        [SerializeField]
        Color m_clientColor;

        [SerializeField]
        Color m_playerColor;

        [Header("Audio clips")]
        [SerializeField]
        AudioClip m_confirmClip;

        [SerializeField]
        AudioClip m_cancelClip;
        [SerializeField]
        private AudioClip _changedCharacterClip;

        bool m_isTimerOn;
        float m_timer;

        private readonly Color k_selectedColor = new Color32(74, 74, 74, 255);
        //自己选择的角色索引
        private int charachterIndex=0;
        //玩家自己位置索引
        private int playerIndex = 0;
        //玩家数
        private int playerCount;

        void Start()
        {
            m_timer = m_timeToStartGame;
        }

        void Update()
        {
           SwitchCharacter();
           QuitRoom();
        }


        /// <summary>
        /// 选择角色
        /// </summary>
        private void SwitchCharacter()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
               ChangeCharacterSelection(-1);
            }else if (Input.GetKeyDown(KeyCode.D))
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
                NetworkManager.Singleton.Send(MID.GalacticKittensQuitRoomReq,new GalacticKittensQuitRoomRequest());
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
            else if (charachterIndex<0)
            {
                charachterIndex = charactersData.Length - 1;
            }
            GalacticKittensAudioManager.Instance.PlaySoundEffect(_changedCharacterClip);
            // 告知服务器角色改变
            var selectCharacterRequest = new GalacticKittenSelectCharacterRequest()
            {
                CharacterId = charachterIndex
            };
            NetworkManager.Singleton.Send(MID.GalacticKittenSelectCharacterReq,selectCharacterRequest);
            SetPlayer(playerIndex,charachterIndex,true);
            
        }
        

        private void OnEnable()
        {
            MessageEventManager.Singleton.AddEvent<GalacticKittensRoomInfoResponse>(MessageEvent.GalacticKittensRoomInfo,RoomInfoRes);
        }

        void OnDisable()
        {
            MessageEventManager.Singleton.RemoveEvent<GalacticKittensRoomInfoResponse>(MessageEvent.GalacticKittensRoomInfo,RoomInfoRes);
            // if (IsServer)
            // {
            //     NetworkManager.Singleton.OnClientDisconnectCallback -= PlayerDisconnects;
            // }
        }



        /// <summary>
        /// @
        /// </summary>
        void RemoveSelectedStates()
        {
            for (int i = 0; i < charactersData.Length; i++)
            {
                charactersData[i].isSelected = false;
            }
        }

        // /// <summary>
        // /// @
        // /// </summary>
        // /// <param name="playerId"></param>
        // /// <param name="disconected"></param>
        // void RemoveReadyStates(long playerId, bool disconected)
        // {
        //     for (int i = 0; i < m_playerStates.Length; i++)
        //     {
        //         if (m_playerStates[i].playerState == ConnectionState.ready &&
        //             m_playerStates[i].playerId == playerId)
        //         {
        //
        //             if (disconected)
        //             {
        //                 m_playerStates[i].playerState = ConnectionState.disconnected;
        //             }
        //             else
        //             {
        //                 m_playerStates[i].playerState = ConnectionState.connected;
        //             }
        //         }
        //     }
        // }



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

        /// <summary>
        /// @
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool IsReady(int playerId)
        {
            return charactersData[playerId].isSelected;
        }

        /// <summary>
        /// 设置角色颜色
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="characterSelected"></param>
        public void SetCharacterColor(int playerId, int characterSelected)
        {
            if (charactersData[characterSelected].isSelected)
            {
                m_charactersContainers[playerId].imageContainer.color = k_selectedColor;
                m_charactersContainers[playerId].nameContainer.color = k_selectedColor;
            }
            else
            {
                m_charactersContainers[playerId].imageContainer.color = Color.white;
                m_charactersContainers[playerId].nameContainer.color = Color.white;
            }
        }

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

            SetCharacterColor(playerIndex, characterSelected);
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

        // /// <summary>
        // /// @
        // /// </summary>
        // /// <param name="playerId"></param>
        // /// <returns></returns>
        // public ConnectionState GetConnectionState(int playerId)
        // {
        //     if (playerId != -1)
        //         return m_playerStates[playerId].playerState;
        //
        //     return ConnectionState.disconnected;
        // }

        /// <summary>
        /// 收到房间改变消息
        /// </summary>
        private void RoomInfoRes(GalacticKittensRoomInfoResponse response)
        {

            
            int i = 0;
            foreach (var playerInfo in response.Room.Player)
            {
                //TODO 其他状态判断 ，选择的角色下标需要服务器传送过来，暂时都为0，都为自己
                m_charactersContainers[i].Init(playerInfo);
                if (playerInfo.PlayerId==DataManager.Singleton.PlayerInfo.PlayerId)
                {
                    playerIndex = i;
                    SetPlayer(i,playerInfo.CharacterId,true);
                }
                else
                {
                    SetPlayer(i,playerInfo.CharacterId,false);
                }
                
                i++;
            }

            //玩家数减少，有退出
            if (response.Room.Player.Count<playerCount)
            {
                for (int j = i; j < 4; j++)
                {
                    ClearPlayer(j);
                }
                
            }
            

            playerCount = response.Room.Player.Count;

        }

        // [ClientRpc]
        // void PlayerConnectsClientRpc(
        //     ulong clientId,
        //     int stateIndex,
        //     ConnectionState state,
        //     NetworkObjectReference player)
        // {
        //     if (IsServer)
        //         return;
        //
        //     if (state != ConnectionState.disconnected)
        //     {
        //         m_playerStates[stateIndex].playerState = state;
        //         m_playerStates[stateIndex].clientId = clientId;
        //
        //         if (player.TryGet(out NetworkObject playerObject))
        //             m_playerStates[stateIndex].playerObject = 
        //                 playerObject.GetComponent<PlayerCharSelection>();
        //     }
        // }
        //
        // /// <summary>
        // /// @
        // /// </summary>
        // /// <param name="clientId"></param>
        // public void PlayerDisconnects(long clientId)
        // {
        //     if (!ClientConnection.Instance.IsExtraClient(clientId))
        //         return;
        //
        //     PlayerNotReady(clientId, isDisconected: true);
        //
        //     m_playerStates[GetPlayerId(clientId)].playerObject.Despawn();
        //
        //     // The client disconnected is the host
        //     if (clientId == 0)
        //     {
        //         NetworkManager.Singleton.Shutdown();
        //     }
        // }

        // /// <summary>
        // /// @
        // /// </summary>
        // /// <param name="clientId"></param>
        // /// <param name="characterSelected"></param>
        // /// <param name="isDisconected"></param>
        // public void PlayerNotReady(ulong clientId, int characterSelected = 0, bool isDisconected = false)
        // {
        //     int playerId = GetPlayerId(clientId);
        //     m_isTimerOn = false;
        //     m_timer = m_timeToStartGame;
        //
        //     RemoveReadyStates(clientId, isDisconected);
        //
        //     // Notify clients to change UI
        //     if (isDisconected)
        //     {
        //         PlayerDisconnectedClientRpc(playerId);
        //     }
        //     else
        //     {
        //         PlayerNotReadyClientRpc(clientId, playerId, characterSelected);
        //     }
        // }

        // /// <summary>
        // /// @
        // /// </summary>
        // /// <param name="playerId"></param>
        // /// <returns></returns>
        // public int GetPlayerId(long playerId)
        // {
        //     for (int i = 0; i < m_playerStates.Length; i++)
        //     {
        //         if (m_playerStates[i].playerId == playerId)
        //             return i;
        //     }
        //
        //     //! This should never happen
        //     Debug.LogError("This should never happen");
        //     return -1;
        // }

        // // Set the player ready if the player is not selected and check if all player are ready to start the countdown @
        // public void PlayerReady(ulong clientId, int playerId, int characterSelected)
        // {
        //     if (!charactersData[characterSelected].isSelected)
        //     {
        //         PlayerReadyClientRpc(clientId, playerId, characterSelected);
        //
        //         StartGameTimer();
        //     }
        // }

        // Set the players UI button @
        public void SetPlayerReadyUIButtons(bool isReady, int characterSelected)
        {
            if (isReady && !charactersData[characterSelected].isSelected)
            {
                m_readyButton.SetActive(false);
                m_cancelButton.SetActive(true);
            }
            else if (!isReady && charactersData[characterSelected].isSelected)
            {
                m_readyButton.SetActive(true);
                m_cancelButton.SetActive(false);
            }
        }

        // Check if the player has selected the character @
        public bool IsSelectedByPlayer(int playerId, int characterSelected)
        {
            return charactersData[characterSelected].playerId == playerId ? true : false;
        }

        // /// <summary>
        // /// @
        // /// </summary>
        // /// <param name="clientId"></param>
        // /// <param name="playerId"></param>
        // /// <param name="characterSelected"></param>
        // [ClientRpc]
        // void PlayerReadyClientRpc(ulong clientId, int playerId, int characterSelected)
        // {
        //     charactersData[characterSelected].isSelected = true;
        //     charactersData[characterSelected].clientId = clientId;
        //     charactersData[characterSelected].playerId = playerId;
        //     m_playerStates[playerId].playerState = ConnectionState.ready;
        //
        //     if (clientId == NetworkManager.Singleton.LocalClientId)
        //     {
        //         m_charactersContainers[playerId].backgroundClientShipReady.SetActive(true);
        //         m_charactersContainers[playerId].backgroundShip.SetActive(false);
        //     }
        //     else
        //     {
        //         m_charactersContainers[playerId].border.SetActive(false);
        //         m_charactersContainers[playerId].borderReady.SetActive(true);
        //         m_charactersContainers[playerId].backgroundShip.SetActive(false);
        //         m_charactersContainers[playerId].backgroundShipReady.SetActive(true);
        //     }
        //
        //     for (int i = 0; i < m_playerStates.Length; i++)
        //     {
        //         // Only changes the ones on clients that are not selected
        //         if (m_playerStates[i].playerState == ConnectionState.connected)
        //         {
        //             if (m_playerStates[i].playerObject.CharSelected == characterSelected)
        //             {
        //                 SetCharacterColor(i, characterSelected);
        //             }
        //         }
        //     }
        //
        //     AudioManager.Instance.PlaySoundEffect(m_confirmClip);
        // }

        // /// <summary>
        // /// @
        // /// </summary>
        // /// <param name="clientId"></param>
        // /// <param name="playerId"></param>
        // /// <param name="characterSelected"></param>
        // [ClientRpc]
        // void PlayerNotReadyClientRpc(ulong clientId, int playerId, int characterSelected)
        // {
        //     charactersData[characterSelected].isSelected = false;
        //     charactersData[characterSelected].clientId = 0UL;
        //     charactersData[characterSelected].playerId = -1;
        //
        //     if (clientId == NetworkManager.Singleton.LocalClientId)
        //     {
        //         m_charactersContainers[playerId].borderClient.SetActive(true);
        //         m_charactersContainers[playerId].backgroundClientShipReady.SetActive(false);
        //         m_charactersContainers[playerId].backgroundShip.SetActive(true);
        //     }
        //     else
        //     {
        //         m_charactersContainers[playerId].border.SetActive(true);
        //         m_charactersContainers[playerId].borderReady.SetActive(false);
        //         m_charactersContainers[playerId].borderClient.SetActive(false);
        //         m_charactersContainers[playerId].backgroundShip.SetActive(true);
        //         m_charactersContainers[playerId].backgroundShipReady.SetActive(false);
        //     }
        //
        //     AudioManager.Instance.PlaySoundEffect(m_cancelClip);
        //     for (int i = 0; i < m_playerStates.Length; i++)
        //     {
        //         // Only changes the ones on clients that are not selected
        //         if (m_playerStates[i].playerState == ConnectionState.connected)
        //         {
        //             if (m_playerStates[i].playerObject.CharSelected == characterSelected)
        //             {
        //                 SetCharacterColor(i, characterSelected);
        //             }
        //         }
        //     }
        // }
        //
        // /// <summary>
        // /// @
        // /// </summary>
        // /// <param name="playerId"></param>
        // [ClientRpc]
        // public void PlayerDisconnectedClientRpc(int playerId)
        // {
        //     SetNonPlayableChar(playerId);
        //
        //     // All character data unselected
        //     RemoveSelectedStates();
        //
        //     m_playerStates[playerId].playerState = ConnectionState.disconnected;
        // }
        //
        // public override void OnNetworkSpawn()
        // {
        //     if (IsServer)
        //     {
        //         NetworkManager.Singleton.OnClientDisconnectCallback += PlayerDisconnects;
        //     }
        // }
    }
}
