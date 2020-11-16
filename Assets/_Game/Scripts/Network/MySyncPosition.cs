using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MySyncPosition : NetworkBehaviour {
    class PositionSnapshot {
        public Vector3 pos;
        public float time;
        public string state;
    }

    [SyncVar(hook = nameof(UpdateState))]
    private string json;

    PositionSnapshot snapTmp;

    PlayerAnimationUpdate animUpdater;
    Player player;
    float timeCount;

    //pos2,time2 --> pos1,time1 --> now
    Vector3 _pos2;
    Vector3 _pos1;
    float _time2;
    float _time1;

    void Start() {
        player = GetComponent<Player>();
        animUpdater = GetComponent<PlayerAnimationUpdate>();
        snapTmp = new PositionSnapshot();
        _pos1 = transform.position;
        _pos2 = transform.position;

        if (!isLocalPlayer) {
            GetComponent<Rigidbody2D>().isKinematic = true;
        }

        InvokeRepeating("TransmitPosition", 0.1f, 0.1f);
    }

    void Update() {

        if (_time1 == 0 || _time2 == 0)
            return;

        timeCount += Time.deltaTime;

        //sync position
        if (!isLocalPlayer) {
            float rate = 0;
            if (_time1 != _time2) {
                rate = timeCount / (_time1 - _time2);
            }

            Vector3 newPos = Vector3.Lerp(_pos2, _pos1, rate);

            if (newPos.x > transform.position.x) {
                GetComponent<SpriteRenderer>().flipX = true;
            } else if (newPos.x < transform.position.x) {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            transform.position = newPos;
        }
    }

    public void UpdateState(string jsonOld, string jsonNew) {
        PositionSnapshot snap = JsonUtility.FromJson<PositionSnapshot>(jsonNew);
        _pos2 = _pos1;
        _pos1 = snap.pos;
        _time2 = _time1;
        _time1 = snap.time;

        player.state = snap.state;
        animUpdater.UpdateAnim(snap.state);
        timeCount = 0;
    }

    [Command]
    public void CmdUpdatePos(string newJson) {
        json = newJson;
    }

    [ClientCallback]
    public void TransmitPosition() {
        if (isLocalPlayer) {
            snapTmp.pos = transform.position;
            snapTmp.state = player.state;
            snapTmp.time = Time.time;
            CmdUpdatePos(JsonUtility.ToJson(snapTmp));
        }
    }
}
