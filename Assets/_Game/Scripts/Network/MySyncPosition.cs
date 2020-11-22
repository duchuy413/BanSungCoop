using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MySyncPosition : NetworkBehaviour {
    class PlayerSnapshot {
        public Vector3 pos;
        public string state;
        public float time;
        public float hp;
        public float dame;
    }

    public float EXPECT_TRANSFER_TIME = 1f;

    [SyncVar(hook = nameof(UpdateState))]
    private string json;

    PlayerSnapshot current;

    PlayerAnimationUpdate animUpdater;
    MyNetworkPuppet puppet;
    Player player;
    float updateTime;

    //pos2,time2 --> pos1,time1 --> now
    Vector3 _pos2;
    Vector3 _pos1;
    float _time2;
    float _time1;
    float scale = 1f;
    //bool updated = false;

    float TIME_DIFF = -1;

    void Start() {
        player = GetComponent<Player>();
        animUpdater = GetComponent<PlayerAnimationUpdate>();
        current = new PlayerSnapshot();
        _pos1 = transform.position;
        _pos2 = transform.position;
        scale = transform.localScale.x;

        if (!isLocalPlayer) {
            GetComponent<Rigidbody2D>().isKinematic = true;

            GetComponent<SpriteRenderer>().enabled = false;
            player.hand.GetComponent<SpriteRenderer>().enabled = false;
            player.weapon.GetComponent<SpriteRenderer>().enabled = false;

            GameObject go = GameSystem.LoadPool("puppet", transform.position);
            puppet = go.GetComponent<MyNetworkPuppet>();
            puppet.target = gameObject;

            GameSystem.CopyComponent(GetComponent<PlayerAnimationUpdate>(), go);
            animUpdater = puppet.GetComponent<PlayerAnimationUpdate>();

            InputSystem.listPuppet.Add(puppet);
        }

        InvokeRepeating("TransmitPosition", 0.1f, 0.1f);
    }

    void Update() {

        //sync position
        if (!isLocalPlayer) {
            //Debug.Log("TIME DIFF: " + TIME_DIFF);

            //Debug.Log("Server - client: " + current.time + " - " + Time.time);

            if (Time.time - TIME_DIFF > current.time) {
                _pos2 = _pos1;
                _pos1 = current.pos;
                _time2 = _time1;
                _time1 = Time.time;

                player.state = current.state;
                animUpdater.UpdateAnim(current.state);
                updateTime = Time.time;

            }

            return;

            if (_time1 == 0 || _time2 == 0)
                return;

            float rate = 0;
            if (_time1 != _time2) {
                rate = (Time.time - updateTime) / (_time1 - _time2);
            }

            Vector3 newPos = Vector3.Lerp(_pos2, _pos1, rate);
            MyDebug.Log("CLIENT pos - time - timerange: (" + newPos.x + " , " + newPos.y + " ) ");
            MyDebug.Log("Time/range: " + (Time.time - updateTime).ToString() + " / " + (_time1 - _time2).ToString());
            MyDebug.Log("Rate: " + rate);

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

        if (TIME_DIFF == -1) {
            TIME_DIFF = Time.time - current.time;
        } else {
            TIME_DIFF = TIME_DIFF * 0.8f + (Time.time - current.time) * 0.2f;
        }
        //_pos2 = _pos1;
        //_pos1 = current.pos;
        //_time2 = _time1;
        //_time1 = Time.time;

        //player.state = current.state;
        //animUpdater.UpdateAnim(current.state);
        //updateTime = Time.time;

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
            current.time = Time.time;
            CmdUpdatePos(JsonUtility.ToJson(current));
        }
    }
}
