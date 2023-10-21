﻿using System;
using Common;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Index
{
    /// <summary>
    /// 游戏信息
    /// </summary>
    public class GameItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text name;
        [SerializeField] private Button button;


        public void SetInfo(GameInfo gameInfo)
        {
            name.text = gameInfo.Name;
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

            var request = new GalacticKittensEnterRoomRequest
            {
                PlayerId = DataManager.Singleton.PlayerInfo.PlayerId
            };
            NetworkManager.Singleton.Send(MID.GalacticKittensEnterRoomReq,request);
        }
    }
}