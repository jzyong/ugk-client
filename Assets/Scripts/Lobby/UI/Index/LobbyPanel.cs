using Network;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Index
{
    /// <summary>
    /// 大厅首页面板
    /// </summary>
    public class LobbyPanel : MonoBehaviour
    {
        [SerializeField] [Tooltip("游戏列表")] private ScrollRect gameListView;

        [SerializeField] private GameItem gameItem;

        void Start()
        {
            MessageEventManager.Singleton.AddEvent<LoadPlayerResponse>(MessageEvent.LoadPlayer, LoadPlayerRes);
            InitDatas();
        }

        private void OnDestroy()
        {
            MessageEventManager.Singleton.RemoveEvent<LoadPlayerResponse>(MessageEvent.LoadPlayer, LoadPlayerRes);
        }



        private void InitDatas()
        {
            if (DataManager.Instance.GameList!=null)
            {
                foreach (var info in DataManager.Instance.GameList)
                {
                    var addGameInfo = Instantiate<GameItem>(gameItem, gameListView.content);
                    addGameInfo.SetInfo(info);
                }
            }
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