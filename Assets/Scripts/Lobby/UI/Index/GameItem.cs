using System;
using Common;
using Game.GalacticKittens;
using Game.GalacticKittens.Manager;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lobby.UI.Index
{
    /// <summary>
    /// 游戏信息
    /// </summary>
    public class GameItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text gameName;
        [SerializeField] private Button button;


        public void SetInfo(GameInfo gameInfo)
        {
            gameName.text = gameInfo.Name;
            button.name = gameInfo.Name;
            SetClickListener(gameInfo.GameId);
            gameObject.SetActive(true);
        }

        private void SetClickListener(uint gameId)
        {
            switch (gameId)
            {
                case 1:
                    button.onClick.AddListener(EnterGalacticKittens);
                    break;
            }
        }

        private void EnterGalacticKittens()
        {
            Debug.Log("进入GalacticKittens");
            var request = new GalacticKittensEnterRoomRequest();
            NetworkManager.Instance.Send(MID.GalacticKittensEnterRoomReq, request);

            //进入选择界面  应该收到返回消息再加载
            // LoadingFadeEffect.Instance.LoadScene("GalacticKittensCharacterSelection");
            SceneManager.LoadScene("GalacticKittensCharacterSelection");
        }
    }
}