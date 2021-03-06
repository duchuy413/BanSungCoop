﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MySyncPosition : NetworkBehaviour {
    [System.Serializable]
    class PlayerSnapshot {
        public Vector3 pos;
        public string state;
        public string direction;
    }

    [SyncVar(hook = nameof(UpdateState))]
    private string json;

    public MyNetworkPuppet puppet;
    PlayerSnapshot current;
    PlayerAnimationUpdate animUpdater;

    Player player;
    float updateTime;
    float scale = 1f;

    void Start() {
        player = GetComponent<Player>();
        animUpdater = GetComponent<PlayerAnimationUpdate>();
        current = new PlayerSnapshot();
        scale = Mathf.Abs(transform.localScale.x);

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

            GameSystem.CopyComponent(GetComponent<PlayerAnimationUpdate>(), go);
            animUpdater = puppet.GetComponent<PlayerAnimationUpdate>();

            InputSystem.listPuppet.Add(puppet);
        }

        InvokeRepeating("TransmitPosition", 0.2f, 0.2f);
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
        if (isLocalPlayer) {
            return;
        }

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
            CmdUpdatePos(JsonUtility.ToJson(current));
        }
    }
}
