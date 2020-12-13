using Mirror;
using Mirror.Discovery;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
//[AddComponentMenu("Network/NetworkDiscoveryHUD")]
[HelpURL("https://mirror-networking.com/docs/Components/NetworkDiscovery.html")]
[RequireComponent(typeof(NetworkDiscovery))]
public class NetworkSystem : MonoBehaviour {
    public static Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

    public static NetworkSystem Instance;
    public static GameObject player;
    public static bool isPlaying = false;
    public NetworkDiscovery networkDiscovery;

    Vector2 scrollViewPos = Vector2.zero;

    public Vector3 SpawnPosition {
        get {
            if (SceneManager.GetActiveScene().name == "Gameplay") {
                return new Vector3(0, 0);
            } else {
                return new Vector3(-9999f, -9999f);
            }
        }
    }

    private void Awake() {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    void OnValidate() {
        if (networkDiscovery == null) {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif

    //void OnGUI()
    //{
    //    if (NetworkManager.singleton == null)
    //        return;

    //    if (NetworkServer.active || NetworkClient.active)
    //        return;

    //    if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active)
    //        DrawGUI();
    //}

    public void StartHost() {
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
        //SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
    }

    //void DrawGUI() {
    //    GUILayout.BeginHorizontal();

    //    GUILayoutOption[] options = new GUILayoutOption[] {
    //        GUILayout.Width(400f),
    //        GUILayout.Height(200f),
    //    };

    //    if (GUILayout.Button("Find Servers", options)) {
    //        discoveredServers.Clear();
    //        networkDiscovery.StartDiscovery();
    //    }

    //    // LAN Host
    //    if (GUILayout.Button("Start Host", options)) {
    //        discoveredServers.Clear();
    //        NetworkManager.singleton.StartHost();
    //        networkDiscovery.AdvertiseServer();
    //        SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
    //    }

    //    // Dedicated server
    //    if (GUILayout.Button("Start Server", options)) {
    //        discoveredServers.Clear();
    //        NetworkManager.singleton.StartServer();

    //        networkDiscovery.AdvertiseServer();
    //    }

    //    GUILayout.EndHorizontal();

    //    // show list of found server

    //    GUILayout.Label($"Discovered Servers [{discoveredServers.Count}]:");

    //    // servers
    //    scrollViewPos = GUILayout.BeginScrollView(scrollViewPos);

    //    foreach (ServerResponse info in discoveredServers.Values) {
    //        if (GUILayout.Button(info.EndPoint.Address.ToString(), options)) {
    //            Connect(info);
    //            SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
    //        }
    //    }

    //    GUILayout.EndScrollView();
    //}

    void Connect(ServerResponse info) {
        NetworkManager.singleton.StartClient(info.uri);
    }

    public void OnDiscoveredServer(ServerResponse info) {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
    }
}
