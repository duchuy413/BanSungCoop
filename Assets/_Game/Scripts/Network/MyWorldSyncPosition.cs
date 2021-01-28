using NativeWebSocket;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

// Link github Native websocket: https://github.com/endel/NativeWebSocket
public class MyWorldSyncPosition : MonoBehaviour {
    public static MyWorldSyncPosition Instance;

    public static string SERVER_URL = "ws://chatgolang.herokuapp.com/ws/main/1";

    public GameObject puppet;

    Dictionary<string, GameObject> positions = new Dictionary<string, GameObject>();
    //Dictionary<string, GameObject> objs = new Dictionary<string, GameObject>();

    WebSocket websocket;
    int user_id;

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Dictionary<string, UserWorldPosition> dic = new Dictionary<string, UserWorldPosition>();
        dic.Add("abc", new UserWorldPosition());
        dic.Add("def", new UserWorldPosition());
        dic.Add("ght", new UserWorldPosition());
        dic.Add("vda", new UserWorldPosition());

        Debug.Log("KET QUA" + JsonConvert.SerializeObject(dic));
    }

    async void Start() {
        user_id = Random.Range(0, 10000);
        StartCoroutine(SendPosition());

        websocket = new WebSocket(SERVER_URL);

        websocket.OnOpen += () => {
            MyDebug.Log("Connection open!");
        };

        websocket.OnError += (e) => {
            MyDebug.Log("Error! " + e);
        };

        websocket.OnClose += (e) => {
            MyDebug.Log("Connection closed!" + e);
        };

        websocket.OnMessage += (bytes) => {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            MyDebug.Log(message);

            Dictionary<string, UserWorldPosition> dic = new Dictionary<string, UserWorldPosition>();
            dic = JsonConvert.DeserializeObject<Dictionary<string, UserWorldPosition>>(message);

            foreach (KeyValuePair<string, UserWorldPosition> entry in dic) {
                UserWorldPosition pos = entry.Value;

                if (!positions.ContainsKey(entry.Key)) {
                    GameObject go = Instantiate(puppet, new Vector3(pos.x, pos.y), Quaternion.identity);
                    positions.Add(entry.Key, go);
                }

                positions[entry.Key].transform.position = new Vector3(pos.x, pos.y);
            }

            //foreach (KeyValuePair<string, UserWorldPosition> entry in positions) {
            //    if (!positions.ContainsKey(entry.Key)) {
            //        UserWorldPosition pos = entry.Value;
            //        GameObject go = Instantiate(puppet, new Vector3(pos.x, pos.y), Quaternion.identity);
            //        positions.Add(entry.Key, go);
            //    }
            //}

            //List<UserWorldPosition> list = null;
            //list = JsonConvert.DeserializeObject<List<UserWorldPosition>>(message);
            //MyDebug.Log(dic.Count.ToString());
            //MyDebug.Log(message);
        };

        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        await websocket.Connect();
    }

    void Update() {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage() {
        //if (websocket.State == WebSocketState.Open) {
        //    string message = "THIS IS MESSAGE";
        //    await websocket.SendText(message);
        //}
    }

    private async void OnApplicationQuit() {
        await websocket.Close();
    }

    public IEnumerator SendPosition() {
        while (true) {
            yield return new WaitForSeconds(1f);
            StartCoroutine(SendPositionData());
        }
    }

    public IEnumerator SendPositionData() {
        //string url = string.Format("https://chatgolang.herokuapp.com/saveclientdata");
        string url = string.Format("https://chatgolang.herokuapp.com/saveclientdat?userid={0}&x={1}&y={2}",user_id,transform.position.x,transform.position.y);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        } else {
            //Debug.Log(www.downloadHandler.text);
        }
    }

}

//using NativeWebSocket;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//// Link github Native websocket: https://github.com/endel/NativeWebSocket
//public class WebsocketServer : MonoBehaviour {
//    public static WebsocketServer Instance;

//    public static string SERVER_URL = "ws://chatgolang.herokuapp.com/ws/main/1";

//    WebSocket websocket;

//    private void Awake() {
//        Instance = this;
//        DontDestroyOnLoad(gameObject);
//    }

//    async void Start() {
//        websocket = new WebSocket(SERVER_URL);

//        websocket.OnOpen += () => {
//            MyDebug.Log("Connection open!");
//        };

//        websocket.OnError += (e) => {
//            MyDebug.Log("Error! " + e);
//        };

//        websocket.OnClose += (e) => {
//            MyDebug.Log("Connection closed!");
//        };

//        websocket.OnMessage += (bytes) => {
//            var message = System.Text.Encoding.UTF8.GetString(bytes);

//            MyDebug.Log("Receive Message: " + message);
//        };

//        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

//        await websocket.Connect();
//    }

//    void Update() {
//#if !UNITY_WEBGL || UNITY_EDITOR
//        websocket.DispatchMessageQueue();
//#endif
//    }

//    async void SendWebSocketMessage() {
//        if (websocket.State == WebSocketState.Open) {
//            string message = "THIS IS MESSAGE";
//            await websocket.SendText(message);
//        }
//    }

//    private async void OnApplicationQuit() {
//        await websocket.Close();
//    }
//}