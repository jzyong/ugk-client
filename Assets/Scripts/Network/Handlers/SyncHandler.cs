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
        private static void SnapSync(UgkMessage ugkMessage)
        {
            var response = new SnapSyncResponse();
            response.MergeFrom(ugkMessage.Bytes);
            SyncManager.Instance.OnSnapSyncReceive(response);
        }
        
        /// <summary>
        /// 快照插值同步
        /// </summary>
        /// <param name="ugkMessage"></param>
        [MessageMap(MID.PredictionSyncRes)]
        private static void PredictionSync(UgkMessage ugkMessage)
        {
            var response = new PredictionSyncResponse();
            response.MergeFrom(ugkMessage.Bytes);
            SyncManager.Instance.OnPredictionSyncReceive(response);
        }

    }
}