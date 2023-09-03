using System;
using Common;
using Google.Protobuf;
using UnityEngine;


namespace Network.Handlers
{
    /// <summary>
    /// 登录消息处理器
    /// </summary>
    internal class LoginHandler
    {
        [MessageMap(MID.HeartRes)]
        private static void Heart(Int64 timeStamp, byte[] data)
        {
           // Debug.Log($" 收到心跳返回：{timeStamp}");
           
        }
        
        
        
        [MessageMap(MID.LoginRes)]
        private static void Login(Int64 timeStamp, byte[] data)
        {
            var response = new LoginResponse();
            response.MergeFrom(data);
            Debug.Log($" 收到登录消息：{response.PlayerId} 结果：{response.Result.Msg}");
            MessageEventManager.Singleton.OnEvent(MessageEvent.Login,response);
        }

        [MessageMap(MID.LoadPlayerRes)]
        private static void LoadPlayer(Int64 timeStamp, byte[] data)
        {
            var response = new LoadPlayerResponse();
            response.MergeFrom(data);
            
            //TODO 大厅面板
            DataManager.Singleton.PlayerInfo = response.PlayerInfo;

        }
        
    }
    
}