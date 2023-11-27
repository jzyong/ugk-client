using System;
using System.Collections.Generic;
using Common.Tools;
using UnityEngine;

namespace Network.Sync
{
    /// <summary>
    /// 同步管理器，保存需要同步管理的对象
    /// TODO
    /// </summary>
    public class SyncManager : SingletonPersistent<SyncManager>
    {
        /// <summary>
        /// 场景中所有快照同步对象
        /// </summary>
        private Dictionary<long, SnapTransform> _snapTransforms = new Dictionary<long, SnapTransform>();

        /// <summary>
        /// 场景中所有预测同步对象
        /// </summary>
        private Dictionary<long, PredictionTransform> _predictionTransforms =
            new Dictionary<long, PredictionTransform>();


        /// <summary>
        /// 批量同步快照的消息
        /// </summary>
        public SnapSyncRequest SnapSyncMessage { get; set; }

        /// <summary>
        /// 批量预测同步消息 , TODO 使用优先级队列，每次最多发送64个对象，防止消息同步过多
        /// </summary>
        public PredictionSyncRequest PredictionSyncMessage { get; set; }


        public override void Awake()
        {
            base.Awake();
            SnapSyncMessage = new SnapSyncRequest();
            PredictionSyncMessage = new PredictionSyncRequest();
        }

        private void OnEnable()
        {
            _snapTransforms.Clear();
            _predictionTransforms.Clear();
        }

        private void OnDisable()
        {
            _snapTransforms.Clear();
            _predictionTransforms.Clear();
        }


        /// <summary>
        /// 收到同步消息
        /// </summary>
        /// <param name="response"></param>
        public void OnSnapSyncReceive(SnapSyncResponse response)
        {
            foreach (var kv in response.Payload)
            {
                if (!_snapTransforms.TryGetValue(kv.Key, out SnapTransform snapTransform))
                {
                    Debug.LogWarning($"同步对象{kv.Key} 不存在");
                    continue;
                }

                snapTransform.OnDeserialize(kv.Value, false);
            }
        }

        /// <summary>
        /// 收到同步消息
        /// </summary>
        /// <param name="response"></param>
        public void OnPredictionSyncReceive(PredictionSyncResponse response)
        {
            foreach (var kv in response.Payload)
            {
                if (!_predictionTransforms.TryGetValue(kv.Key, out PredictionTransform predictionTransform))
                {
                    Debug.LogWarning($"同步对象{kv.Key} 不存在");
                    continue;
                }

                predictionTransform.OnDeserialize(kv.Value, false);
            }
        }


        public void Update()
        {
            //批量同步消息
            if (SnapSyncMessage.Payload.Count > 0)
            {
                NetworkManager.Singleton.Send(MID.SnapSyncReq, SnapSyncMessage);
                SnapSyncMessage.Payload.Clear();
            }

            if (PredictionSyncMessage.Payload.Count > 0)
            {
                NetworkManager.Singleton.Send(MID.PredictionSyncReq, PredictionSyncMessage);
                PredictionSyncMessage.Payload.Clear();
            }
        }
    }
}