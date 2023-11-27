using System;
using System.Collections.Generic;
using Common;
using Common.Tools.SnapshotInterpolation;
using Google.Protobuf;
using Network.Sync;
using UnityEngine;


namespace Network.Handlers
{
    /// <summary>
    /// 同步消息处理器
    /// </summary>
    internal class SyncHandler
    {
        /// <summary>
        /// 快照插值同步
        /// </summary>
        /// <param name="ugkMessage"></param>
        [MessageMap(MID.SnapSyncRes)]
        private static void Heart(UgkMessage ugkMessage)
        {
            // Debug.Log($" 收到心跳返回：{timeStamp}");
            var response = new SnapSyncResponse();
            response.MergeFrom(ugkMessage.Bytes);
            SyncManager.Instance.OnSnapSyncReceive(response);
        }

    }
}