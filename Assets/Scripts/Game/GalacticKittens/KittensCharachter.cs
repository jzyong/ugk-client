﻿using System;
using Network;
using Network.Sync;
using UnityEngine;

namespace Game.GalacticKittens
{
    /// <summary>
    /// Kittens角色
    /// </summary>
    public class KittensCharacter : MonoBehaviour
    {
        private SnapTransform _snapTransform;


        public long Id { set; get; }


        private void Start()
        {
            _snapTransform = GetComponent<SnapTransform>();
        }

        private void Update()
        {
            //TODO 监听按键事件，进行移动

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireReq();
            }
        }


        //玩家开火
        private void FireReq()
        {
            GalacticKittensFireRequest request = new GalacticKittensFireRequest();
            NetworkManager.Singleton.Send(MID.GalacticKittensFireReq, request);
        }
    }
}