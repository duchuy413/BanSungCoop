using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MySyncPosition : NetworkBehaviour {
    class PlayerSnapshot {
        public Vector3 pos;
        public string state;
        public float hp;
        public float dame;
    }

    [SyncVar(hook = nameof(UpdateState))]
    private string json;

    PlayerSnapshot current;

    PlayerAnimationUpdate animUpdater;
    Player player;
    float updateTime;

    //pos2,time2 --> pos1,time1 --> now
    Vector3 _pos2;
    Vector3 _pos1;
    float _time2;
    float _time1;
    float scale = 1f;

    void Start() {
        player = GetComponent<Player>();
        animUpdater = GetComponent<PlayerAnimationUpdate>();
        current = new PlayerSnapshot();
        _pos1 = transform.position;
        _pos2 = transform.position;
        scale = transform.localScale.x;

        if (!isLocalPlayer) {
            GetComponent<Rigidbody2D>().isKinematic = true;
        }

        InvokeRepeating("TransmitPosition", 0.1f, 0.1f);
    }

    void Update() {

        if (_time1 == 0 || _time2 == 0)
            return;

        //sync position
        if (!isLocalPlayer) {
            float rate = 0;
            if (_time1 != _time2) {
                rate = (Time.time - updateTime) / (_time1 - _time2);
            }

            Vector3 newPos = Vector3.Lerp(_pos2, _pos1, rate);

            if (newPos.x > transform.position.x) {
                transform.localScale = new Vector3(scale, scale);
            } else if (newPos.x < transform.position.x) {
                transform.localScale = new Vector3(-scale, scale);
            }

            if (transform.position != newPos) {
                if (!isLocalPlayer) {
                    MyDebug.Log("CLIENT POS: " + newPos.x + " - " + newPos.y + " time: " + (Time.time - updateTime).ToString() + "   oldtime-newtime:  " + _time2 + " - " + _time1);
                }
            }

            transform.position = newPos;

            
        }
    }

    public void UpdateState(string jsonOld, string jsonNew) {
        current = JsonUtility.FromJson<PlayerSnapshot>(jsonNew);

        _pos2 = _pos1;
        _pos1 = current.pos;
        _time2 = _time1;
        _time1 = Time.time;

        player.state = current.state;
        animUpdater.UpdateAnim(current.state);
        updateTime = Time.time;

        //if (transform.position != current.pos) {

        //}
        if (transform.position != current.pos) {
            if (!isLocalPlayer) {
                MyDebug.Log("RECEIVE SERVER POS: " + current.pos.x + " - " + current.pos.y + " - time: " + (_time1 - _time2).ToString());
                MyDebug.Log("RECEIVE SERVER TIME: " + Time.time.ToString());
            }
        }
    }

    [Command]
    public void CmdUpdatePos(string newJson) {
        json = newJson;
    }

    [ClientCallback]
    public void TransmitPosition() {
        if (isLocalPlayer) {
            current.pos = transform.position;
            current.state = player.state;
            CmdUpdatePos(JsonUtility.ToJson(current));
        }
    }
}
