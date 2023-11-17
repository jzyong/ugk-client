// Snapshot Interpolation: https://gafferongames.com/post/snapshot_interpolation/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Common.Tools;
using Common.Tools.SnapshotInterpolation;
using UnityEngine;

namespace Network.Sync
{
    /// <summary>
    /// Transform 同步 TODO 需要调试，对比Mirror测试
    /// </summary>
    public abstract class NetworkTransform : MonoBehaviour
    {
        // 作用同步对象
        [Header("Target")] [Tooltip("The Transform component to sync. May be on on this GameObject, or on a child.")]
        public Transform target;

        // selective sync 
        [Header("Selective Sync\nDon't change these at Runtime")]
        public bool syncPosition = true; // do not change at runtime!

        public bool syncRotation = true; // do not change at runtime!
        public bool syncScale = false; // do not change at runtime! rare. off by default.

        // interpolation is on by default, but can be disabled to jump to
        // the destination immediately. some projects need this.
        [Header("Interpolation")] [Tooltip("Set to false to have a snap-like effect on position movement.")]
        public bool interpolatePosition = true;

        [Tooltip("Set to false to have a snap-like effect on rotations.")]
        public bool interpolateRotation = true;

        [Tooltip(
            "Set to false to remove scale smoothing. Example use-case: Instant flipping of sprites that use -X and +X for direction.")]
        public bool interpolateScale = true;

        [Tooltip("消息发送间隔")] public double sendInterval = 0.033;

        //下次消息发送时间
        private double nextSendTime;


        public readonly SortedList<double, TransformSnapshot> snapshots = new SortedList<double, TransformSnapshot>();


        // debugging 
        [Header("Debug")] public bool showGizmos;
        public bool showOverlay;
        public Color overlayColor = new Color(0, 0, 0, 0.5f);

        // initialization //////////////////////////////////////////////////////
        // make sure to call this when inheriting too!
        protected virtual void Awake()
        {
        }

        protected void OnValidate()
        {
            // set target to self if none yet
            if (target == null) target = transform;
        }

        // snapshot functions //////////////////////////////////////////////////
        // construct a snapshot of the current state
        // => internal for testing
        protected virtual TransformSnapshot Construct()
        {
            // NetworkTime.localTime for double precision until Unity has it too
            return new TransformSnapshot(
                // our local time is what the other end uses as remote time
                NetworkTime.LocalTime, // Unity 2019 doesn't have timeAsDouble yet
                0, // the other end fills out local time itself
                target.localPosition,
                target.localRotation,
                target.localScale
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        protected void AddSnapshot(double timeStamp, Vector3? position,
            Quaternion? rotation, Vector3? scale)
        {
            // position, rotation, scale can have no value if same as last time.
            // saves bandwidth.
            // but we still need to feed it to snapshot interpolation. we can't
            // just have gaps in there if nothing has changed. for example, if
            //   client sends snapshot at t=0
            //   client sends nothing for 10s because not moved
            //   client sends snapshot at t=10
            // then the server would assume that it's one super slow move and
            // replay it for 10 seconds.
            if (!position.HasValue)
                position = snapshots.Count > 0 ? snapshots.Values[snapshots.Count - 1].position : target.localPosition;
            if (!rotation.HasValue)
                rotation = snapshots.Count > 0 ? snapshots.Values[snapshots.Count - 1].rotation : target.localRotation;
            if (!scale.HasValue)
                scale = snapshots.Count > 0 ? snapshots.Values[snapshots.Count - 1].scale : target.localScale;

            // insert transform snapshot
            SnapshotInterpolation.InsertIfNotExists(snapshots, new TransformSnapshot(
                timeStamp, // arrival remote timestamp. NOT remote time.
                NetworkTime.LocalTime, // Unity 2019 doesn't have timeAsDouble yet
                position.Value,
                rotation.Value,
                scale.Value
            ));
        }

        // apply a snapshot to the Transform.
        // -> start, end, interpolated are all passed in caes they are needed
        // -> a regular game would apply the 'interpolated' snapshot
        // -> a board game might want to jump to 'goal' directly
        // (it's easier to always interpolate and then apply selectively,
        //  instead of manually interpolating x, y, z, ... depending on flags)
        // => internal for testing
        //
        // NOTE: stuck detection is unnecessary here.
        //       we always set transform.position anyway, we can't get stuck. @
        protected virtual void Apply(TransformSnapshot interpolated, TransformSnapshot endGoal)
        {
            // local position/rotation for VR support
            //
            // if syncPosition/Rotation/Scale is disabled then we received nulls
            // -> current position/rotation/scale would've been added as snapshot
            // -> we still interpolated
            // -> but simply don't apply it. if the user doesn't want to sync
            //    scale, then we should not touch scale etc.

            if (syncPosition)
                target.localPosition = interpolatePosition ? interpolated.position : endGoal.position;

            if (syncRotation)
                target.localRotation = interpolateRotation ? interpolated.rotation : endGoal.rotation;

            if (syncScale)
                target.localScale = interpolateScale ? interpolated.scale : endGoal.scale;
        }

        /// <summary>
        /// @
        /// </summary>
        public virtual void Reset()
        {
            // disabled objects aren't updated anymore.
            // so let's clear the buffers.
            snapshots.Clear();
            // reset 'last' for delta too
            last = new TransformSnapshot(0, 0, Vector3.zero, Quaternion.identity, Vector3.zero);
        }

        protected virtual void OnEnable()
        {
            Reset();
        }

        protected virtual void OnDisable()
        {
            Reset();
        }


        // OnGUI allocates even if it does nothing. avoid in release.
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        // debug ///////////////////////////////////////////////////////////////
        protected virtual void OnGUI()
        {
            if (!showOverlay) return;
            if (!Camera.main) return;

            // show data next to player for easier debugging. this is very useful!
            // IMPORTANT: this is basically an ESP hack for shooter games.
            //            DO NOT make this available with a hotkey in release builds
            if (!Debug.isDebugBuild) return;

            // project position to screen
            Vector3 point = Camera.main.WorldToScreenPoint(target.position);

            // enough alpha, in front of camera and in screen?
            if (point.z >= 0 && IsPointInScreen(point))
            {
                GUI.color = overlayColor;
                GUILayout.BeginArea(new Rect(point.x, Screen.height - point.y, 200, 100));

                // always show both client & server buffers so it's super
                // obvious if we accidentally populate both.
                GUILayout.Label($"Buffer:{snapshots.Count}");

                GUILayout.EndArea();
                GUI.color = Color.white;
            }
        }

        public static bool IsPointInScreen(Vector2 point) =>
            0 <= point.x && point.x < Screen.width &&
            0 <= point.y && point.y < Screen.height;
#endif


        [Tooltip(
            "If we only sync on change, then we need to correct old snapshots if more time than sendInterval * multiplier has elapsed.\n\nOtherwise the first move will always start interpolating from the last move sequence's time, which will make it stutter when starting every time.")]
        public float onlySyncOnChangeCorrectionMultiplier = 2;

        [Header("Rotation")] [Tooltip("Sensitivity of changes needed before an updated state is sent over the network")]
        public float rotationSensitivity = 0.01f;

        [Tooltip(
            "Apply smallest-three quaternion compression. This is lossy, you can disable it if the small rotation inaccuracies are noticeable in your project.")]
        public bool compressRotation = false;

        // delta compression is capable of detecting byte-level changes.
        // if we scale float position to bytes,
        // then small movements will only change one byte.
        // this gives optimal bandwidth.
        //   benchmark with 0.01 precision: 130 KB/s => 60 KB/s
        //   benchmark with 0.1  precision: 130 KB/s => 30 KB/s
        [Header("Precision")]
        [Tooltip(
            "Position is rounded in order to drastically minimize bandwidth.\n\nFor example, a precision of 0.01 rounds to a centimeter. In other words, sub-centimeter movements aren't synced until they eventually exceeded an actual centimeter.\n\nDepending on how important the object is, a precision of 0.01-0.10 (1-10 cm) is recommended.\n\nFor example, even a 1cm precision combined with delta compression cuts the Benchmark demo's bandwidth in half, compared to sending every tiny change.")]
        [Range(0.00_01f, 1f)]
        // disallow 0 division. 1mm to 1m precision is enough range.
        public float positionPrecision = 0.01f; // 1 cm

        [Range(0.00_01f, 1f)] // disallow 0 division. 1mm to 1m precision is enough range.
        public float scalePrecision = 0.01f; // 1 cm


        // Used to store last sent snapshots
        protected TransformSnapshot last;

        protected int lastClientCount = 1;

        void Update()
        {
            UpdateClient();
        }

        void LateUpdate()
        {
            if (NetworkTime.ServerTime > nextSendTime && Changed(Construct()))
            {
                SyncTransform();
                nextSendTime += sendInterval;
            }
        }

        protected virtual void UpdateClient()
        {
            // only while we have snapshots
            if (snapshots.Count > 0)
            {
                // step the interpolation without touching time.
                // NetworkClient is responsible for time globally.
                SnapshotInterpolation.StepInterpolation(
                    snapshots,
                    NetworkTime.ServerTime, // == NetworkClient.localTimeline from snapshot interpolation
                    out TransformSnapshot from,
                    out TransformSnapshot to,
                    out double t);

                // interpolate & apply
                TransformSnapshot computed = TransformSnapshot.Interpolate(from, to, t);
                Apply(computed, to);
            }

            lastClientCount = snapshots.Count;
        }

        /// <summary>
        /// 同步数据，子类实现
        /// </summary>
        protected abstract void SyncTransform();
        

        // check if position / rotation / scale changed since last sync 
        protected virtual bool Changed(TransformSnapshot current) =>
            // position is quantized and delta compressed.
            // only consider it changed if the quantized representation is changed.
            QuantizedChanged(last.position, current.position, positionPrecision) ||
            // rotation isn't quantized / delta compressed.
            // check with sensitivity.
            Quaternion.Angle(last.rotation, current.rotation) > rotationSensitivity ||
            // scale is quantized and delta compressed.
            // only consider it changed if the quantized representation is changed.
            // careful: don't use 'serialized / deserialized last'. as it depends on sync mode etc.
            QuantizedChanged(last.scale, current.scale, scalePrecision);

        // helper function to compare quantized representations of a Vector3 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool QuantizedChanged(Vector3 u, Vector3 v, float precision)
        {
            Compression.ScaleToLong(u, precision, out Vector3Long uQuantized);
            Compression.ScaleToLong(v, precision, out Vector3Long vQuantized);
            return uQuantized != vQuantized;
        }


        // server broadcasts sync message to all clients
        protected virtual void OnReceiveTransform(Vector3? position, Quaternion? rotation, Vector3? scale,
            double remoteTime)
        {
            // 'only sync on change' needs a correction on every new move sequence.
            if (NeedsCorrection(snapshots, remoteTime, 1, onlySyncOnChangeCorrectionMultiplier))
            {
                RewriteHistory(
                    snapshots,
                    NetworkTime.LocalTime, // arrival remote timestamp. NOT remote timeline.
                    NetworkTime.LocalTime, // Unity 2019 doesn't have timeAsDouble yet
                    1,
                    target.localPosition,
                    target.localRotation,
                    target.localScale);
            }

            AddSnapshot( remoteTime ,position, rotation, scale);
        }

        // only sync on change /////////////////////////////////////////////////
        // snap interp. needs a continous flow of packets.
        // 'only sync on change' interrupts it while not changed.
        // once it restarts, snap interp. will interp from the last old position.
        // this will cause very noticeable stutter for the first move each time.
        // the fix is quite simple.

        // 1. detect if the remaining snapshot is too old from a past move. 
        static bool NeedsCorrection(
            SortedList<double, TransformSnapshot> snapshots,
            double remoteTimestamp,
            double bufferTime,
            double toleranceMultiplier) =>
            snapshots.Count == 1 &&
            remoteTimestamp - snapshots.Keys[0] >= bufferTime * toleranceMultiplier;

        // 2. insert a fake snapshot at current position,
        //    exactly one 'sendInterval' behind the newly received one.
        static void RewriteHistory(
            SortedList<double, TransformSnapshot> snapshots,
            // timestamp of packet arrival, not interpolated remote time!
            double remoteTimeStamp,
            double localTime,
            double sendInterval,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            // clear the previous snapshot
            snapshots.Clear();

            // insert a fake one at where we used to be,
            // 'sendInterval' behind the new one.
            SnapshotInterpolation.InsertIfNotExists(snapshots, new TransformSnapshot(
                remoteTimeStamp - sendInterval, // arrival remote timestamp. NOT remote time.
                localTime - sendInterval, // Unity 2019 doesn't have timeAsDouble yet
                position,
                rotation,
                scale
            ));
        }

    }
}