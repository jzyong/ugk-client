using System.Collections.Generic;
using Common.Tools;
using UnityEngine;

namespace Network.Sync
{
    /// <summary>
    /// 同步管理器，保存需要同步管理的对象
    /// </summary>o
    public class SyncManager : SingletonPersistent<SyncManager>
    {
        /// <summary>
        /// 场景中所有快照同步对象
        /// </summary>
        private readonly Dictionary<long, SnapTransform> _snapTransforms = new Dictionary<long, SnapTransform>();

        /// <summary>
        /// 场景中所有预测同步对象
        /// </summary>
        private readonly Dictionary<long, PredictionTransform> _predictionTransforms =
            new Dictionary<long, PredictionTransform>();


        /// <summary>
        /// 批量同步快照的消息
        /// </summary>
        public SnapSyncRequest SnapSyncMessage { get; set; }

        /// <summary>
        /// 批量预测同步消息 
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
            ResetData();
        }

        private void OnDisable()
        {
            ResetData();
        }

        private void ResetData()
        {
            _snapTransforms.Clear();
            _predictionTransforms.Clear();
            SnapSyncMessage.Payload.Clear();
            PredictionSyncMessage.Payload.Clear();
        }

        /// <summary>
        /// 收到同步消息
        /// </summary>
        public void OnSnapSyncReceive(UgkMessage ugkMessage, SnapSyncResponse response)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            foreach (var kv in response.Payload)
            {
                if (!_snapTransforms.TryGetValue(kv.Key, out SnapTransform snapTransform))
                {
                    Debug.LogWarning($"同步对象{kv.Key} 不存在");
                    continue;
                }

                // 自己的消息丢弃
                if (snapTransform.IsOnwer)
                {
                    continue;
                }


                snapTransform.OnDeserialize(ugkMessage, kv.Value, false);
            }
        }

        /// <summary>
        /// 收到同步消息
        /// </summary>
        public void OnPredictionSyncReceive(UgkMessage ugkMessage, PredictionSyncResponse response)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            foreach (var kv in response.Payload)
            {
                if (!_predictionTransforms.TryGetValue(kv.Key, out PredictionTransform predictionTransform))
                {
                    Debug.LogWarning($"同步对象{kv.Key} 不存在");
                    continue;
                }

                predictionTransform.OnDeserialize(ugkMessage, kv.Value, false);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">0移除，1移除并隐藏，2移除并销毁</param>
        public bool RemovePredictionTransform(long id, int type = 0)
        {
            if (_predictionTransforms.Remove(id, out PredictionTransform predictionTransform))
            {
                if (type == 1)
                {
                    predictionTransform.gameObject.SetActive(false);
                }
                else if (type == 2)
                {
                    Destroy(predictionTransform);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">0移除，1移除并隐藏，2移除并销毁</param>
        public bool RemoveSnapTransform(long id, int type = 0)
        {
            if (_snapTransforms.Remove(id, out SnapTransform snapTransform))
            {
                if (type == 1)
                {
                    snapTransform.gameObject.SetActive(false);
                }
                else if (type == 2)
                {
                    Destroy(snapTransform);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 移除同步对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">0移除，1移除并隐藏，2移除并销毁</param>
        /// <returns></returns>
        public bool RemoveSyncObject(long id, int type = 0)
        {
            if (RemoveSnapTransform(id, type))
            {
                return true;
            }

            return RemovePredictionTransform(id, type);
        }


        public void AddSnapTransform(SnapTransform snapTransform)
        {
            _snapTransforms[snapTransform.Id] = snapTransform;
        }


        public void AddPredictionTransform(PredictionTransform predictionTransform)
        {
            _predictionTransforms[predictionTransform.Id] = predictionTransform;
        }


        public void Update()
        {
            //批量同步消息
            if (SnapSyncMessage.Payload.Count > 0)
            {
                NetworkManager.Instance.Send(MID.SnapSyncReq, SnapSyncMessage);
                SnapSyncMessage.Payload.Clear();
            }

            var predictionCount = PredictionSyncMessage.Payload.Count;
            if (predictionCount > 0)
            {
                NetworkManager.Instance.Send(MID.PredictionSyncReq, PredictionSyncMessage);
                if (predictionCount > 64)
                {
                    Debug.LogWarning($"同步消息太多{predictionCount} =>{PredictionSyncMessage.Payload.Keys}");
                }

                PredictionSyncMessage.Payload.Clear();
            }
        }
    }
}