﻿using System;
using System.Collections.Generic;
using Common;
using Common.Tools.SnapshotInterpolation;
using Google.Protobuf;
using Lobby;
using Network.Sync;
using UnityEngine;


namespace Network.Handlers
{
    /// <summary>
    /// 登录消息处理器
    /// </summary>
    internal class LoginHandler
    {
        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="ugkMessage"></param>
        [MessageMap(MID.HeartRes)]
        private static void Heart(UgkMessage ugkMessage)
        {
             // Debug.Log($" 收到心跳返回：{ugkMessage.TimeStamp}");
            var response = new HeartResponse();
            response.MergeFrom(ugkMessage.Bytes);
            //计算rtt
            double newRtt = NetworkTime.LocalTime - response.ClientTime;
            NetworkTime.RttEMV.Add(newRtt);
            //时间平滑插值
            if (ugkMessage.TimeStamp < 86400000)
            {
                double remoteTime = ugkMessage.GetTime();
                NetworkTimeInterpolation.OnTimeSnapshot(new TimeSnapshot(remoteTime, NetworkTime.LocalTime));
                Debug.Log($"localServerTime={NetworkTime.ServerTime}-remoteServerTime={remoteTime}={NetworkTime.ServerTime-remoteTime}s rtt={newRtt} rttValue={NetworkTime.RTT}" );
            }
            else
            {
                NetworkTimeInterpolation.ServerTimeStamp = ugkMessage.TimeStamp;
            }
        }


        /// <summary>
        /// 登录
        /// </summary>
        [MessageMap(MID.LoginRes)]
        private static void Login(UgkMessage ugkMessage)
        {
            var response = new LoginResponse();
            response.MergeFrom(ugkMessage.Bytes);
            Debug.Log($" 收到登录消息：{response.PlayerId} 结果：{response.Result.Msg}");
            MessageEventManager.Instance.OnEvent(MessageEvent.Login, response);
        }

        /// <summary>
        /// 加载玩家数据
        /// </summary>
        [MessageMap(MID.LoadPlayerRes)]
        private static void LoadPlayer(UgkMessage ugkMessage)
        {
            var response = new LoadPlayerResponse();
            response.MergeFrom(ugkMessage.Bytes);

            // 大厅面板,游戏列表处理
            DataManager.Instance.PlayerInfo = response.PlayerInfo;
            List<GameInfo> gameLists = new List<GameInfo>(response.GameInfo);
            DataManager.Instance.GameList = gameLists;
            Debug.Log($" 收到数据加载消息：{response} 结果：{response.Result?.Msg}");
            MessageEventManager.Instance.OnEvent(MessageEvent.LoadPlayer, response);
        }
    }
}