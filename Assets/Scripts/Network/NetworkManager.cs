using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace Network
{
    /// <summary>
    /// 网络管理
    /// </summary>
    [DisallowMultipleComponent]
    public class NetworkManager : MonoBehaviour
    {
        // transport layer 
        [Header("Network Info")]
        [Tooltip("Transport component attached to this object that server and client will use to connect")]
        public Transport transport;

        /// <summary>Server's address for clients to connect to. </summary>
        [Tooltip(
            "Network Address where the client should connect to the server. Server does not use this for anything.")]
        public string networkAddress = "localhost";


        /// <summary>The one and only NetworkManager </summary>
        public static NetworkManager singleton { get; internal set; }


        /// <summary>True if the server is running or client is connected/connecting.</summary>
        public bool isNetworkActive => NetworkClient.active;


        public void Awake()
        {
            // Don't allow collision-destroyed second instance to continue.
            if (!InitializeSingleton()) return;
        }

        public  void Start()
        {
        }

        public  void Update()
        {
        }

        // virtual so that inheriting classes' LateUpdate() can call base.LateUpdate() too
        public  void LateUpdate()
        {
        }

      

        //
        void SetupClient()
        {
            InitializeSingleton();
        }

        /// <summary>Starts the client, connects it to the server with networkAddress. </summary>
        public void StartClient()
        {
            if (NetworkClient.active)
            {
                Debug.LogWarning("Client already started.");
                return;
            }


            SetupClient();



            if (string.IsNullOrWhiteSpace(networkAddress))
            {
                Debug.LogError("Must set the Network Address field in the manager");
                return;
            }
            // Debug.Log($"NetworkManager StartClient address:{networkAddress}");

            NetworkClient.Connect(networkAddress);

        }

        /// <summary>Starts the client, connects it to the server via Uri</summary>
        public void StartClient(Uri uri)
        {
            if (NetworkClient.active)
            {
                Debug.LogWarning("Client already started.");
                return;
            }


            SetupClient();


            // Debug.Log($"NetworkManager StartClient address:{uri}");
            networkAddress = uri.Host;

            NetworkClient.Connect(uri);

        }

      

        /// <summary>Stops and disconnects the client. </summary>
        public void StopClient()
        {

            // ask client -> transport to disconnect.
            // handle voluntary and involuntary disconnects in OnClientDisconnect.
            //
            //   StopClient
            //     NetworkClient.Disconnect
            //       Transport.Disconnect
            //         ...
            //       Transport.OnClientDisconnect
            //     NetworkClient.OnTransportDisconnect
            //   NetworkManager.OnClientDisconnect
            NetworkClient.Disconnect();

            // UNET invoked OnDisconnected cleanup immediately.
            // let's keep it for now, in case any projects depend on it.
            // TODO simply remove this in the future.
        }

        // called when quitting the application by closing the window / pressing
        // stop in the editor. virtual so that inheriting classes'
        // OnApplicationQuit() can call base.OnApplicationQuit() too
        public virtual void OnApplicationQuit()
        {
            // stop client first
            // (we want to send the quit packet to the server instead of waiting
            //  for a timeout)
            if (NetworkClient.isConnected)
            {
                StopClient();
                //Debug.Log("OnApplicationQuit: stopped client");
            }


            // Call ResetStatics to reset statics and singleton
            ResetStatics();
        }


        // @
        bool InitializeSingleton()
        {
            if (singleton != null && singleton == this)
                return true;

            if (singleton != null)
            {
                Debug.LogWarning(
                    "Multiple NetworkManagers detected in the scene. Only one NetworkManager can exist at a time. The duplicate NetworkManager will be destroyed.");
                Destroy(gameObject);

                // Return false to not allow collision-destroyed second instance to continue.
                return false;
            }

            //Debug.Log("NetworkManager created singleton (DontDestroyOnLoad)");
            singleton = this;
            if (Application.isPlaying)
            {
                // Force the object to scene root, in case user made it a child of something
                // in the scene since DDOL is only allowed for scene root objects
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }

            // set active transport AFTER setting singleton.
            // so only if we didn't destroy ourselves.

            // This tries to avoid missing transport errors and more clearly tells user what to fix.
            if (transport == null)
                if (TryGetComponent(out Transport newTransport))
                {
                    Debug.LogWarning(
                        $"No Transport assigned to Network Manager - Using {newTransport} found on same object.");
                    transport = newTransport;
                }
                else
                {
                    Debug.LogError("No Transport on Network Manager...add a transport and assign it.");
                    return false;
                }

            Transport.active = transport;
            return true;
        }

      


        // This is the only way to clear the singleton, so another instance can be created.
        // RuntimeInitializeOnLoadMethod -> fast playmode without domain reload
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ResetStatics()
        {
            // and finally (in case it isn't null already)...
            singleton = null;
        }

        public  void OnDestroy()
        {
            //Debug.Log("NetworkManager destroyed");
        }
        
    }
}