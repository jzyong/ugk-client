// NetworkTime now uses NetworkClient's snapshot interpolated timeline.
// this gives ideal results & ensures everything is on the same timeline.
// previously, NetworkTransforms were on separate timelines.
//
// however, some of the old NetworkTime code remains for ping time (rtt).
// some users may still be using that.

using System.Runtime.CompilerServices;
using Common.Tools;
using kcp2k;
using UnityEngine;
#if !UNITY_2020_3_OR_NEWER
using Stopwatch = System.Diagnostics.Stopwatch;
#endif

namespace Network
{
    // TODO 待测试
    /// <summary>Synchronizes server time to clients. </summary>
    public static class NetworkTime
    {
        /// <summary>Ping message frequency, used to calculate network time and RTT</summary>
        public static float PingFrequency = KcpPeer.PING_INTERVAL/1000f;

        /// <summary>Average out the last few results from Ping</summary>
        public static int PingWindowSize = 6;

        public static ExponentialMovingAverage RttEMV = new ExponentialMovingAverage(PingWindowSize);

        /// <summary>Returns double precision clock time _in this system_, unaffected by the network.</summary>
#if UNITY_2020_3_OR_NEWER
        public static double LocalTime
        {
            // NetworkTime uses unscaled time and ignores Time.timeScale.
            // fixes Time.timeScale getting server & client time out of sync:
            // https://github.com/MirrorNetworking/Mirror/issues/3409
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Time.unscaledTimeAsDouble;
        }
#else
        // need stopwatch for older Unity versions, but it's quite slow.
        // CAREFUL: unlike Time.time, this is not a FRAME time.
        //          it changes during the frame too.
        static readonly Stopwatch stopwatch = new Stopwatch();
        static NetworkTime() => stopwatch.Start();
        public static double localTime => stopwatch.Elapsed.TotalSeconds;
#endif

        /// <summary>The time in seconds since the server started. </summary>
        // via global NetworkClient snapshot interpolated timeline (if client).
        // on server, this is simply Time.timeAsDouble.
        //
        // I measured the accuracy of float and I got this:
        // for the same day,  accuracy is better than 1 ms
        // after 1 day,  accuracy goes down to 7 ms
        // after 10 days, accuracy is 61 ms
        // after 30 days , accuracy is 238 ms
        // after 60 days, accuracy is 454 ms
        // in other words,  if the server is running for 2 months,
        // and you cast down to float,  then the time will jump in 0.4s intervals.
        public static double ServerTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NetworkTimeInterpolation.localTimeline;
        }

        /// <summary>
        /// 服务器时间搓 ms
        /// </summary>
        public static long ServerTimeStamp
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NetworkTimeInterpolation.ServerTimeStamp;
        }

        /// <summary>Clock difference in seconds between the client and the server.  </summary>
        // original implementation used 'client - server' time. keep it this way.
        public static double Offset => LocalTime - ServerTime;

        /// <summary>Round trip time (in seconds) that it takes a message to go client->server->client. </summary>
        public static double RTT => RttEMV.Value;

        // RuntimeInitializeOnLoadMethod -> fast playmode without domain reload 
        [RuntimeInitializeOnLoadMethod]
        public static void ResetStatics()
        {
            PingFrequency = KcpPeer.PING_INTERVAL/1000f;
            PingWindowSize = 6;
            RttEMV = new ExponentialMovingAverage(PingWindowSize);
#if !UNITY_2020_3_OR_NEWER
            stopwatch.Restart();
#endif
        }

    }
}
