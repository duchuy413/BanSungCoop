using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MySyncPosition : NetworkBehaviour
{
    [SyncVar]
    public Vector3 desiredPos;
    [SyncVar]
    public string state;

    Rigidbody2D rb2d;
    Player player;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        MyDebug.Log("Calling Start in My SyncPosition");
    }

    void Update() {
        MyDebug.Log("---------------Update-----------------------");
        MyDebug.Log("Calling Update in My SyncPosition");
        MyDebug.Log("isLocalPlayer current is: " + isLocalPlayer);
        MyDebug.Log("isClient is " + isClient);
        //everyone update there position 
        if (isLocalPlayer && isClient) {
            MyDebug.Log("Sending position to server: Calling CmdUpdatePos");
            MyDebug.Log("Curent position: " + transform.position);
            MyDebug.Log("Current state: " + player.state);
            CmdUpdatePos(transform.position, player.state);
            return;
        }

        MyDebug.Log("Syncing position ");
        MyDebug.Log("current position is : " + transform.position);
        MyDebug.Log("desired Pos is: " + transform.position);

        //if this is not local, then sync the position
        float dif = desiredPos.x - transform.position.x;
        if (Mathf.Abs(dif) < 0.1f) {
            rb2d.velocity = new Vector3(0, rb2d.velocity.y);
            return;
        }

        player.state = state;

        if (player.state == "go") {
            rb2d.velocity = new Vector3(Mathf.Sign(dif) * Player.SPEED, rb2d.velocity.y);
        } else if (player.state == "run") {
            rb2d.velocity = new Vector3(Mathf.Sign(dif) * Player.SPEED * 1.5f, rb2d.velocity.y);
        } else {
            rb2d.velocity = new Vector3(Mathf.Sign(dif) * Player.SPEED, rb2d.velocity.y);
        }

        MyDebug.Log("---------------Update-----------------------");
    }

    [Command]
    public void CmdUpdatePos(Vector3 pos, string state) {
        MyDebug.Log("Updating value to server");
        MyDebug.Log("Current desirepos - state: " + desiredPos + " -- " + this.state);
        MyDebug.Log("New desirepos - state: " + desiredPos + " -- " + state);

        //update position, after this call, position will sync between clients
        desiredPos = pos;
        this.state = state;
    }
}
