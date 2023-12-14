using System;
using Common.Tools;
using Network.Sync;
using UnityEngine;

namespace Network
{
    /// <summary>
    /// Shows Network messages and bytes sent and received per second.
    /// </summary>
    /// <remarks>
    /// <para>Add this component to the same object as Network Manager.</para>
    /// </remarks>
    [DisallowMultipleComponent]
    public class NetworkStatistics : MonoBehaviour
    {
        // update interval
        double intervalStartTime;

        // ---------------------------------------------------------------------

        // CLIENT (public fields for other components to grab statistics)
        // long bytes to support >2GB
        [HideInInspector] public int clientIntervalReceivedPackets;
        [HideInInspector] public long clientIntervalReceivedBytes;
        [HideInInspector] public int clientIntervalSentPackets;
        [HideInInspector] public long clientIntervalSentBytes;

        // results from last interval
        // long bytes to support >2GB
        [HideInInspector] public int clientReceivedPacketsPerSecond;
        [HideInInspector] public long clientReceivedBytesPerSecond;
        [HideInInspector] public int clientSentPacketsPerSecond;
        [HideInInspector] public long clientSentBytesPerSecond;

        //场景网络同步对象数
        [HideInInspector] public static int sceneObjectCount;
        private int fpsCount;
        private int fps;


        void Start()
        {
            // find available transport
            Transport transport = Transport.active;
            if (transport != null)
            {
                transport.OnClientDataReceived += OnClientReceive;
                transport.OnClientDataSent += OnClientSend;
            }
            else
                Debug.LogError(
                    $"NetworkStatistics: no available or active Transport found on this platform: {Application.platform}");
        }

        void OnDestroy()
        {
            // remove transport hooks
            Transport transport = Transport.active;
            if (transport != null)
            {
                transport.OnClientDataReceived -= OnClientReceive;
                transport.OnClientDataSent -= OnClientSend;
            }
        }

        void OnClientReceive(ArraySegment<byte> data)
        {
            ++clientIntervalReceivedPackets;
            clientIntervalReceivedBytes += data.Count;
        }

        void OnClientSend(ArraySegment<byte> data)
        {
            ++clientIntervalSentPackets;
            clientIntervalSentBytes += data.Count;
        }


        void Update()
        {
            // calculate results every second
            if (NetworkTime.LocalTime >= intervalStartTime + 1)
            {
                if (NetworkClient.active) UpdateClient();

                intervalStartTime = NetworkTime.LocalTime;
                fps = fpsCount;
                fpsCount=0;
            }

            fpsCount++;
        }

        void UpdateClient()
        {
            clientReceivedPacketsPerSecond = clientIntervalReceivedPackets;
            clientReceivedBytesPerSecond = clientIntervalReceivedBytes;
            clientSentPacketsPerSecond = clientIntervalSentPackets;
            clientSentBytesPerSecond = clientIntervalSentBytes;

            clientIntervalReceivedPackets = 0;
            clientIntervalReceivedBytes = 0;
            clientIntervalSentPackets = 0;
            clientIntervalSentBytes = 0;
        }

        void OnGUI()
        {
            if (NetworkClient.active)
            {
                // create main GUI area
                // 120 is below NetworkManager HUD in all cases.
                GUILayout.BeginArea(new Rect(10, 10, 215, 300));

                // show client / server stats if active
                if (NetworkClient.active) OnClientGUI();

                // end of GUI area
                GUILayout.EndArea();
            }
        }

        void OnClientGUI()
        {
            // background
            GUILayout.BeginVertical("Box");
            GUILayout.Label("<b>Client Statistics</b>");

            // sending ("msgs" instead of "packets" to fit larger numbers)
            GUILayout.Label(
                $"Send: {clientSentPacketsPerSecond} msgs @ {Utils.PrettyBytes(clientSentBytesPerSecond)}/s");

            // receiving ("msgs" instead of "packets" to fit larger numbers)
            GUILayout.Label(
                $"Recv: {clientReceivedPacketsPerSecond} msgs @ {Utils.PrettyBytes(clientReceivedBytesPerSecond)}/s");

            GUILayout.Label(
                $"Sync object count: {sceneObjectCount} @ fps {fps}" );

            // end background
            GUILayout.EndVertical();
        }
    }
}