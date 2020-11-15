using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : NetworkBehaviour {
    public static float SPEED = 10f;
    public static float JUMP_FORCE = 1500f;
    public static float SHOOT_RATE = 0.2f;

    public string state = "";
    public string direction = "left";
    public Rigidbody2D rb2d;
    public Transform barrel;

    bool isShooting = false;
    bool isRunning = false;
    float nextShoot = 0f;
    int jumpCount;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        jumpCount = 2;
    }

    public override void OnStartLocalPlayer() {
        if (isLocalPlayer) {
            NetworkSystem.player = gameObject;
            CameraFollower.Instance.target = gameObject;
            InputSystem.Instance.player = this;
        }
    }

    void Update() {
        if (!isLocalPlayer)
            return;

        if (Joystick.Instance.Horizontal > 0 || Input.GetKeyDown(KeyCode.RightArrow)) {
            direction = "right";

            if (isRunning) {
                rb2d.velocity = new Vector3(SPEED * 1.5f, rb2d.velocity.y);
            } else {
                rb2d.velocity = new Vector3(SPEED, rb2d.velocity.y);
            }

            if (state != "jump" && state != "fall") {
                if (isRunning)
                    state = "run";
                else
                    state = "go";
            }

        } else if (Joystick.Instance.Horizontal < 0 || Input.GetKeyDown(KeyCode.LeftArrow)) {
            direction = "left";

            if (isRunning) {
                rb2d.velocity = new Vector3(-SPEED * 1.5f, rb2d.velocity.y);
            } else {
                rb2d.velocity = new Vector3(-SPEED, rb2d.velocity.y);
            }

            if (state != "jump" && state != "fall") {
                if (isRunning)
                    state = "run";
                else
                    state = "go";
            }
        } else {
            rb2d.velocity = new Vector3(0, rb2d.velocity.y);
            if (state != "jump" && state != "fall")
                state = "stand";
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.Z)) {
            isRunning = false;
        }

        if (isShooting && Time.time > nextShoot) {
            nextShoot = Time.time + SHOOT_RATE;
            GameObject bullet = Gameplay.LoadPool("bullet", barrel.position);
        }
    }

    public void Jump() {
        if (jumpCount <= 0)
            return;

        jumpCount--;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(0, JUMP_FORCE));
        state = "jump";
        StartCoroutine(Fall(jumpCount));
    }

    public IEnumerator Fall(int jumpId) {
        yield return new WaitForSeconds(0.35f);
        if (jumpCount == jumpId) {
            state = "fall";
        }
    }

    public void ShootStart() {
        isShooting = true;
    }

    public void ShootEnd() {
        isShooting = false;
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

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("THIS IS TRIGGER ENTER 2D");
        if (collision.gameObject.CompareTag("Ground")) {
            if (Mathf.Abs(Joystick.Instance.Horizontal) > 0 || Input.GetKeyDown(KeyCode.RightArrow)) {
                if (isRunning)
                    state = "run";
                else
                    state = "go";
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
