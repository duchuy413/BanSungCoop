﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerWorld : MonoBehaviour {
    public static float SPEED = 8f;
    public static float JUMP_FORCE = 1500f;
    public static float SHOOT_RATE = 0.1f;
    public static float DASH_FORCE = 5000f;

    [HideInInspector]
    public CharacterStatRuntime currentStat;
    [HideInInspector]
    public string state = "";
    [HideInInspector]
    public string direction = "left";
    [HideInInspector]
    public bool isShooting = false;

    private Rigidbody2D rb2d;
    private PlayerCommand playerCommand;
    private IAttack weapon;

    float scale = 1f;
    bool isRunning = true;
    bool isGrounding = true;
    int jumpCount;

    private void Awake() {
        transform.position = GameManager.startPosition;
    }

    void Start() {
        playerCommand = GetComponent<PlayerCommand>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;

        jumpCount = 2;
        scale = transform.localScale.x;
        transform.position = new Vector3(0, -17f);
        CameraFollower.Instance.target = gameObject;
    }

    void Update() {
        if (Joystick.Instance.Horizontal > 0 || Input.GetKeyDown(KeyCode.RightArrow)) {
            direction = "right";

            if (state != "jump" && state != "fall") {
                if (isRunning) {
                    state = "run";
                } else {
                    state = "go";
                }
            }

        } else if (Joystick.Instance.Horizontal < 0 || Input.GetKeyDown(KeyCode.LeftArrow)) {
            direction = "left";

            if (state != "jump" && state != "fall") {
                if (isRunning) {
                    state = "run";
                } else {
                    state = "go";
                }
            }
        } else {
            rb2d.velocity = new Vector3(0, rb2d.velocity.y);
            if (state != "jump" && state != "fall")
                state = "stand";
        }

        if (isRunning) {
            rb2d.velocity = new Vector3(Joystick.Instance.Horizontal, Joystick.Instance.Vertical) * 1.5f;
        } else {
            rb2d.velocity = new Vector3(Joystick.Instance.Horizontal, Joystick.Instance.Vertical);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            Dash();
        }

        if (direction == "left") {
            transform.localScale = new Vector3(scale, scale);
        } else {
            transform.localScale = new Vector3(-scale, scale);
        }
    }

    public void Jump() {
        if (jumpCount <= 0)
            return;

        jumpCount--;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(0, JUMP_FORCE));
        state = "jump";
        isGrounding = false;
        gameObject.layer = LayerMask.NameToLayer("Jumping");
        StartCoroutine(Fall(jumpCount));

        AudioSystem.Instance.PlaySound("Sound/player/dash");
    }

    public IEnumerator Fall(int jumpId) {
        yield return new WaitForSeconds(0.35f);

        gameObject.layer = LayerMask.NameToLayer("Player");
        if (!isGrounding && jumpCount == jumpId) {
            state = "fall";
        }
    }

    public void ShootStart() {
        isShooting = true;
        playerCommand.ShootButtonPress("down");
    }

    public void ShootEnd() {
        isShooting = false;
        playerCommand.ShootButtonPress("up");
    }

    public void RunStart() {
        isRunning = true;
    }

    public void RunEnd() {
        isRunning = false;
    }

    public void UseSkill() {
        Debug.Log("USE SKILL!");
    }

    public void Dash() {
        if (direction == "left") {
            playerCommand.Dash(new Vector2(-DASH_FORCE, 0));
        } else if (direction == "right") {
            playerCommand.Dash(new Vector2(DASH_FORCE, 0));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            if (Mathf.Abs(Joystick.Instance.Horizontal) > 0 || Input.GetKeyDown(KeyCode.RightArrow)) {
                if (isRunning) {
                    state = "run";
                } else {
                    state = "go";
                }

                isGrounding = true;
            } else {
                state = "stand";
            }
            jumpCount = 2;

            //change sprite imediately after hit ground
            GetComponent<PlayerAnimationUpdate>().Update();
            GetComponent<SpriteRenderer>().sprite = GetComponent<FramesAnimator>().spritesheet[0];
        }
    }
}
