using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FramesAnimator))]
[RequireComponent(typeof(Rigidbody2D))]
public class DMovement : MonoBehaviour {
    public string state;
    public string direction;
    public CharacterStat data;

    [HideInInspector]
    public FramesAnimator animator;
    [HideInInspector]
    public Rigidbody2D rb2d;
    [HideInInspector]
    public SpriteRenderer renderer;
    public string stateStatus;
    public float speedValue;

    public virtual void Awake() {
        animator = GetComponent<FramesAnimator>();
        renderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        //rb2d.gravityScale = 0f;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Update() {
        //float 
        //animator.spritesheet = ConvertStringToSprites(state, direction);
        if (state == "") {
            return;
        }

        stateStatus = state.Substring(0, state.IndexOf("_"));

        speedValue = 0;

        if (stateStatus == "run") {
            animator.spritesheet = data.run;
            speedValue = data.speed * 1.5f;
        } else if (stateStatus == "go") {
            speedValue = data.speed;
            animator.spritesheet = data.go;

        } else if (stateStatus == "stand") {
            animator.spritesheet = data.stand;
            speedValue = 0;
        }

        direction = state.Substring(state.IndexOf("_") + 1);
        rb2d.velocity = GetVelocity(direction, speedValue);

        if (rb2d.velocity.x > 0)
            renderer.flipX = true;
        else if (rb2d.velocity.x < 0)
            renderer.flipX = false;
    }

    public Vector3 GetVelocity(string direction, float speedvalue) {

        Vector3 velocity;

        switch (direction) {
            case "up":
                velocity = new Vector3(0, speedvalue);
                break;
            case "down":
                velocity = new Vector3(0, -speedvalue);
                break;
            case "left":
                velocity = new Vector3(-speedvalue, 0);
                break;
            case "right":
                velocity = new Vector3(speedvalue, 0);
                break;
            case "upleft":
                velocity = new Vector3(-speedvalue, speedvalue);
                break;
            case "leftup":
                velocity = new Vector3(-speedvalue, speedvalue);
                break;
            case "upright":
                velocity = new Vector3(speedvalue, speedvalue);
                break;
            case "rightup":
                velocity = new Vector3(speedvalue, speedvalue);
                break;
            case "downleft":
                velocity = new Vector3(-speedvalue, -speedvalue);
                break;
            case "leftdown":
                velocity = new Vector3(-speedvalue, -speedvalue);
                break;
            case "downright":
                velocity = new Vector3(speedvalue, -speedvalue);
                break;
            case "rightdown":
                velocity = new Vector3(speedvalue, -speedvalue);
                break;
            default:
                velocity = rb2d.velocity;
                break;
        }

        if (!data.canFly) {
            velocity.y = GetComponent<Rigidbody2D>().velocity.y;
        }
        return velocity;
    }

    public virtual Vector3 ConvertDirection(string state, float value) {
        string direction = state.Substring(state.IndexOf("_") + 1);
        if (direction == "up") return new Vector3(0f, value);
        else if (direction == "down") return new Vector3(0f, -value);
        else if (direction == "left") return new Vector3(-value, 0f);
        else if (direction == "right") return new Vector3(value, 0f);
        else return new Vector3(0f, 0f);
    }

    public void FaceToGameObject(GameObject obj) {
        float player_posx, player_posy, npc_posx, npc_posy;

        player_posx = obj.transform.position.x;
        player_posy = obj.transform.position.y;
        npc_posx = transform.position.x;
        npc_posy = transform.position.y;

        float distance_x = Mathf.Abs(player_posx - npc_posx);
        float distance_y = Mathf.Abs(player_posy - npc_posy);

        //player and npc standing on vertical line
        if (distance_x < distance_y) {
            if (player_posy < npc_posy) {
                //player is below the npc
                obj.GetComponent<DMovement>().state = "stand_up";
                state = "stand_down";
            } else {
                //player is above the npc
                obj.GetComponent<DMovement>().state = "stand_down";
                state = "stand_up";
            }
        } else {
            //player and npc standing on horizontal line
            if (player_posx < npc_posx) {
                //player is on the left of npc
                obj.GetComponent<DMovement>().state = "stand_right";
                state = "stand_left";
            } else {
                //player is on the right of npc
                obj.GetComponent<DMovement>().state = "stand_left";
                state = "stand_right";
            }
        }
        obj.GetComponent<DMovement>().Update();
        Update();
    }
}

