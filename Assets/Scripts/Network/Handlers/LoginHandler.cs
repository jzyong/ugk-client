using System;
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
            Debug.Log($" 收到心跳返回：{timeStamp}");
        }
    }
}