using System;
using Common;
using Google.Protobuf;
using UnityEngine;


namespace Network.Handlers.Game
{
    /// <summary>
    /// GalacticKittens 游戏
    /// </summary>
    internal class GalacticKittensHandler
    {
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="data"></param>
        [MessageMap(MID.GalacticKittensEnterRoomRes)]
        private static void EnterRoom(UgkMessage ugkMessage)
        {
            var response = new GalacticKittensEnterRoomResponse();
            response.MergeFrom(ugkMessage.Bytes);
            Debug.Log($"进入房间消息结果：{response}");
        }
        
        /// <summary>
        /// 房间消息变更（服务器推送）
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="data"></param>
        [MessageMap(MID.GalacticKittensRoomInfoRes)]
        private static void RoomInfo(UgkMessage ugkMessage)
        {
            var response = new GalacticKittensRoomInfoResponse();
            response.MergeFrom(ugkMessage.Bytes);
            Debug.Log($"房间消息：{response}");
            //TODO 修改UI显示页面
            //MessageEventManager.Singleton.OnEvent(MessageEvent.Login,response);
        }
      
        
    }
    
}