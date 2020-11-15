using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MySyncPosition : NetworkBehaviour
{
    [SyncVar]
    private Vector3 pastPosition2;
    [SyncVar]
    private Vector3 pastPosition1;
    [SyncVar]
    private float pastTime2 = 0f;
    [SyncVar]
    private float pastTime1 = 0f;
    [SyncVar]
    public string state;

    Vector3 _pos2;
    Vector3 _pos1;
    float _time2;
    float _time1;

    Rigidbody2D rb2d;
    Player player;
    float lastPastTime1;
    float timeCount;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        pastPosition1 = transform.position;
        pastPosition2 = transform.position;
        lastPastTime1 = 0;

        if (!isLocalPlayer) {
            GetComponent<Rigidbody2D>().isKinematic = true;
        }

        InvokeRepeating("TransmitPosition",0.1f, 0.1f);
    }

    void Update() {
        //everyone update there position 
        //if (isLocalPlayer && isClient) {

        //    CmdUpdatePos(transform.position, player.state);
        //    return;
        //}

        if (pastTime1 == 0 || pastTime2 == 0)
            return;

        //check for time changed from server
        if (lastPastTime1 != pastTime1 && pastTime1 != pastTime2) {
            lastPastTime1 = pastTime1;
            timeCount = 0;
            _time1 = pastTime1;
            _time2 = pastTime2;
            _pos1 = pastPosition1;
            _pos2 = pastPosition2;
            Debug.Log(pastTime1.ToString() + " - " + pastTime2.ToString());
        }
        timeCount += Time.deltaTime;

        if (!isLocalPlayer) {
            player.state = state;
            float rate = 0;
            if (pastTime1 != pastTime2) {
                rate = timeCount / (_time1 - _time2);
            }

            transform.position = Vector3.Lerp(_pos2, _pos1, rate);
        }





        //if this is not local, then sync the position
        //float dif = desiredPos.x - transform.position.x;
        //if (Mathf.Abs(dif) < 0.1f) {
        //    rb2d.velocity = new Vector3(0, rb2d.velocity.y);
        //    return;
        //}



        //if (player.state == "go") {
        //    rb2d.velocity = new Vector3(Mathf.Sign(dif) * Player.SPEED, rb2d.velocity.y);
        //} else if (player.state == "run") {
        //    rb2d.velocity = new Vector3(Mathf.Sign(dif) * Player.SPEED * 1.5f, rb2d.velocity.y);
        //} else {
        //    rb2d.velocity = new Vector3(Mathf.Sign(dif) * Player.SPEED, rb2d.velocity.y);
        //}
    }

    [Command]
    public void CmdUpdatePos(Vector3 pos, string state) {
        //update position, after this call, position will sync between clients
        pastPosition2 = pastPosition1;
        pastPosition1 = pos;
        pastTime2 = pastTime1;
        pastTime1 = Time.time;
        this.state = state;
    }

    [ClientCallback]
    public void TransmitPosition() {
        if (isLocalPlayer) {
            CmdUpdatePos(transform.position, player.state);
        }
    }
}
