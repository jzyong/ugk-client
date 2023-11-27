using System;
using Common.Tools;
using Google.Protobuf;
using Network.Serialize;

namespace Network.Sync
{
    /// <summary>
    /// 预测同步 TODO 怎样平滑处理？
    /// TODO 通过权重优先级实现,参考fps ？
    /// 
    /// </summary>
    public class PredictionTransform : NetworkTransform
    {
        public void Update()
        {
            //TODO 计算位置和方向并应用
        }

        public void LateUpdate()
        {
            //超时强制同步一下
            if (NetworkTime.ServerTime>nextSendTime)
            {
                
                nextSendTime += sendInterval;
            }
        }


        public void OnTransformChange()
        {
            //TODO 增加权重
        }
        
        //TODO 序列化和反序列化
        
          /// <summary>
        /// 发送同步数据,序列化
        /// </summary>
        protected void OnSerialize(bool initialState)
        {
            // using (NetworkWriterPooled writer = NetworkWriterPool.Get())
            // {
            //     // get current snapshot for broadcasting.
            //     TransformSnapshot snapshot = Construct();
            //
            //     // initial
            //     if (initialState)
            //     {
            //         if (last.remoteTime > 0) snapshot = last;
            //         if (syncPosition) writer.WriteVector3(snapshot.position);
            //         if (syncRotation)
            //         {
            //             // (optional) smallest three compression for now. no delta.
            //             if (compressRotation)
            //                 writer.WriteUInt(Compression.CompressQuaternion(snapshot.rotation));
            //             else
            //                 writer.WriteQuaternion(snapshot.rotation);
            //         }
            //
            //         if (syncScale) writer.WriteVector3(snapshot.scale);
            //     }
            //     // delta
            //     else
            //     {
            //         if (syncPosition)
            //         {
            //             // quantize -> delta -> varint
            //             Compression.ScaleToLong(snapshot.position, positionPrecision, out Vector3Long quantized);
            //             DeltaCompression.Compress(writer, lastSerializedPosition, quantized);
            //         }
            //
            //         if (syncRotation)
            //         {
            //             // (optional) smallest three compression for now. no delta.
            //             if (compressRotation)
            //                 writer.WriteUInt(Compression.CompressQuaternion(snapshot.rotation));
            //             else
            //                 writer.WriteQuaternion(snapshot.rotation);
            //         }
            //
            //         if (syncScale)
            //         {
            //             // quantize -> delta -> varint
            //             Compression.ScaleToLong(snapshot.scale, scalePrecision, out Vector3Long quantized);
            //             DeltaCompression.Compress(writer, lastSerializedScale, quantized);
            //         }
            //     }
            //
            //     // save serialized as 'last' for next delta compression
            //     if (syncPosition)
            //         Compression.ScaleToLong(snapshot.position, positionPrecision, out lastSerializedPosition);
            //     if (syncScale) Compression.ScaleToLong(snapshot.scale, scalePrecision, out lastSerializedScale);
            //
            //     // set 'last'
            //     last = snapshot;
            //
            //     //发送数据
            //     var data = ByteString.CopyFrom(writer.ToArray());
            //     SyncManager.Instance.SnapSyncMessage.Payload[id] = data;
            // }
        }

        /// <summary>
        /// 接收同步数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="initialState"></param>
        public void OnDeserialize(ByteString data, bool initialState)
        {
            // var segment = new ArraySegment<byte>(data.ToByteArray());
            // using (NetworkReaderPooled reader = NetworkReaderPool.Get(segment))
            // {
            //     Vector3? position = null;
            //     Quaternion? rotation = null;
            //     Vector3? scale = null;
            //
            //     // initial
            //     if (initialState)
            //     {
            //         if (syncPosition) position = reader.ReadVector3();
            //         if (syncRotation)
            //         {
            //             // (optional) smallest three compression for now. no delta.
            //             if (compressRotation)
            //                 rotation = Compression.DecompressQuaternion(reader.ReadUInt());
            //             else
            //                 rotation = reader.ReadQuaternion();
            //         }
            //
            //         if (syncScale) scale = reader.ReadVector3();
            //     }
            //     // delta
            //     else
            //     {
            //         // varint -> delta -> quantize
            //         if (syncPosition)
            //         {
            //             Vector3Long quantized = DeltaCompression.Decompress(reader, lastDeserializedPosition);
            //             position = Compression.ScaleToFloat(quantized, positionPrecision);
            //         }
            //
            //         if (syncRotation)
            //         {
            //             // (optional) smallest three compression for now. no delta.
            //             if (compressRotation)
            //                 rotation = Compression.DecompressQuaternion(reader.ReadUInt());
            //             else
            //                 rotation = reader.ReadQuaternion();
            //         }
            //
            //         if (syncScale)
            //         {
            //             Vector3Long quantized = DeltaCompression.Decompress(reader, lastDeserializedScale);
            //             scale = Compression.ScaleToFloat(quantized, scalePrecision);
            //         }
            //     }
            //
            //     OnReceiveTransform(position, rotation, scale);
            //
            //     // save deserialized as 'last' for next delta compression
            //     if (syncPosition)
            //         Compression.ScaleToLong(position.Value, positionPrecision, out lastDeserializedPosition);
            //     if (syncScale) Compression.ScaleToLong(scale.Value, scalePrecision, out lastDeserializedScale);
            // }
        }
        
    }
}