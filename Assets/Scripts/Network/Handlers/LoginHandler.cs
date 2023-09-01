using System;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            //TODO 判断消息
            SceneManager.LoadScene("Lobby");
            Debug.Log($" 收到登录消息：{response.PlayerId} 结果：{response.Result.Msg}");
        }
        
    }
    
}