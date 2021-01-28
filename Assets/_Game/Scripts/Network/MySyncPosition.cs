using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MySyncPosition : NetworkBehaviour {
    class PlayerSnapshot {
        public Vector3 pos;
        public string state;
        public string direction;
        //public bool isShooting;
    }

    [SyncVar(hook = nameof(UpdateState))]
    private string json;

    public MyNetworkPuppet puppet;
    PlayerSnapshot current;
    PlayerAnimationUpdate animUpdater;

    Player player;
    float updateTime;
    float scale = 1f;

    //private void Awake() {
    //    transform.position = GameManager.startPosition;
    //}

    void Start() {
        player = GetComponent<Player>();
        animUpdater = GetComponent<PlayerAnimationUpdate>();
        current = new PlayerSnapshot();
        scale = Mathf.Abs(transform.localScale.x);

        //TODO: Restore puppet script
        if (!isLocalPlayer) {
            GetComponent<Rigidbody2D>().isKinematic = true;

            GetComponent<SpriteRenderer>().enabled = false;
            //player.t_hand.GetComponent<SpriteRenderer>().enabled = false;
            //player.t_weapon.GetComponent<SpriteRenderer>().enabled = false;

            GameObject go = GameSystem.LoadPool("puppet", transform.position);
            
            puppet = go.GetComponent<MyNetworkPuppet>();
            puppet.target = gameObject;
            puppet.player = GetComponent<Player>();
            puppet.LoadWeapon(GameManager.weapon);

            //TODO: Add again this
            //puppet.LoadWeapon(player.weaponStat);

            GameSystem.CopyComponent(GetComponent<PlayerAnimationUpdate>(), go);
            animUpdater = puppet.GetComponent<PlayerAnimationUpdate>();

            InputSystem.listPuppet.Add(puppet);
        }

        InvokeRepeating("TransmitPosition", 0.1f, 0.1f);
    }

    void Update() {
        if (!isLocalPlayer && current != null) {
            if (current.direction == "right") {
                puppet.transform.localScale = new Vector3(-scale, scale);
            } else if (current.direction == "left") {
                puppet.transform.localScale = new Vector3(scale, scale);
            }
        }
    }

    public void UpdateState(string jsonOld, string jsonNew) {
        current = JsonUtility.FromJson<PlayerSnapshot>(jsonNew);

        player.state = current.state;
        player.direction = current.direction;
        animUpdater.UpdateAnim(current.state);
        updateTime = Time.time;
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
            current.direction = player.direction;
            //current.isShooting = player.isShooting;
            CmdUpdatePos(JsonUtility.ToJson(current));
        }
    }
}
