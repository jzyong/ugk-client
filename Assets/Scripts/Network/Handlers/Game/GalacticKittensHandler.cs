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
        /// <param name="ugkMessage"></param>
        [MessageMap(MID.GalacticKittensEnterRoomRes)]
        private static void EnterRoom(UgkMessage ugkMessage)
        {
            var response = new GalacticKittensEnterRoomResponse();
            response.MergeFrom(ugkMessage.Bytes);
            Debug.Log($"进入房间消息结果：{response}");
        }
        
        /// <summary>
        /// 退出房间
        /// </summary>
        /// <param name="ugkMessage"></param>
        [MessageMap(MID.GalacticKittensQuitRoomRes)]
        private static void QuitRoom(UgkMessage ugkMessage)
        {
            var response = new GalacticKittensQuitRoomResponse();
            response.MergeFrom(ugkMessage.Bytes);
            if (response.Result.Status != 200)
            {
                Debug.LogWarning($"退出房间错误：{response.Result.Msg}");
            }
        }

        /// <summary>
        /// 房间消息变更（服务器推送）
        /// </summary>
        /// <param name="ugkMessage"></param>
        [MessageMap(MID.GalacticKittensRoomInfoRes)]
        private static void RoomInfo(UgkMessage ugkMessage)
        {
            var response = new GalacticKittensRoomInfoResponse();
            response.MergeFrom(ugkMessage.Bytes);
            Debug.Log($"房间消息：{response}");
            // 修改UI显示页面
            MessageEventManager.Singleton.OnEvent(MessageEvent.GalacticKittensRoomInfo, response);
        }

        /// <summary>
        /// 选择角色
        /// </summary>
        /// <param name="ugkMessage"></param>
        [MessageMap(MID.GalacticKittenSelectCharacterRes)]
        private static void SelectCharacter(UgkMessage ugkMessage)
        {
            var response = new GalacticKittenSelectCharacterResponse();
            response.MergeFrom(ugkMessage.Bytes);
            if (response.Result.Status != 200)
            {
                Debug.LogWarning($"选择角色失败：{response.Result.Msg}");
            }
        }
    }
}