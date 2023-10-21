using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Lobby.UI.Index
{
    /// <summary>
    /// 大厅首页面板
    /// </summary>
    public class LobbyPanel : MonoBehaviour
    {
        [SerializeField] [Tooltip("游戏列表")] private ScrollRect gameListView;

        [FormerlySerializedAs("gameInfo")] [SerializeField] private GameItem gameItem;

        // Start is called before the first frame update
        void Start()
        {
            MessageEventManager.Singleton.AddEvent<LoadPlayerResponse>(MessageEvent.LoadPlayer, LoadPlayerRes);
        }

        private void OnDestroy()
        {
            MessageEventManager.Singleton.RemoveEvent<LoadPlayerResponse>(MessageEvent.LoadPlayer, LoadPlayerRes);
        }

        // Update is called once per frame
        void Update()
        {
        }


        private void LoadPlayerRes(LoadPlayerResponse response)
        {
            foreach (var info in response.GameInfo)
            {
                var addGameInfo = Instantiate<GameItem>(gameItem, gameListView.content);
                addGameInfo.SetInfo(info);
                Debug.Log($"添加游戏{info.GameId}-{info.Name}");
            }
        }


      
        
    }
}