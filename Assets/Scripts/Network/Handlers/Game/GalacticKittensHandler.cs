using System;
using Common;
using Game.GalacticKittens;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.SceneManagement;


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

            GalacticKittens galacticKittens= DataManager.Singleton.GalacticKittens;
            if (galacticKittens==null)
            {
                galacticKittens = new GalacticKittens();
                DataManager.Singleton.GalacticKittens = galacticKittens;
            }
            
            
            // 修改UI显示页面
            MessageEventManager.Singleton.OnEvent(MessageEvent.GalacticKittensRoomInfo, response);
            
            //切换为加载场景
            if (response.Room.State == (uint)RoomState.Load && galacticKittens.RoomState != response.Room.State)
            {
                SceneManager.LoadScene("GalacticKittensControls");
            }
            else if (response.Room.State==(uint)RoomState.Gameing&&response.Room.State!=galacticKittens.RoomState)
            {
                SceneManager.LoadScene("GalacticKittensGamePlay");
            }
            

            galacticKittens.RoomState = response.Room.State;
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
        
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="ugkMessage"></param>
        [MessageMap(MID.GalacticKittensPrepareRes)]
        private static void Prepare(UgkMessage ugkMessage)
        {
            var response = new GalacticKittensPrepareResponse();
            response.MergeFrom(ugkMessage.Bytes);
            if (response.Result!=null&&response.Result.Status != 200)
            {
                Debug.LogWarning($"准备确认取消失败：{response.Result.Msg}");
            }
        }
    }
}