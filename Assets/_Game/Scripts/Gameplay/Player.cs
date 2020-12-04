using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : NetworkBehaviour {
    public static float SPEED = 8f;
    public static float JUMP_FORCE = 1500f;
    public static float SHOOT_RATE = 0.1f;
    public static float DASH_FORCE = 5000f;

    public string state = "";
    public string direction = "left";
    public bool isShooting = false;

    public Rigidbody2D rb2d;
    public PlayerCommand playerCommand;
    public GameObject cameraPos;
    public Transform hand;
    public Weapon weapon;
    public WeaponStat weaponStat;

    bool isRunning = true;
    float nextShoot = 0f;
    int jumpCount;
    float scale = 1f;
    float hp = 500f;
    float dame = 20f;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        playerCommand = GetComponent<PlayerCommand>();
        jumpCount = 2;
        scale = transform.localScale.x;
        LoadWeapon("longgun");
    }

    public override void OnStartLocalPlayer() {
        if (isLocalPlayer) {
            NetworkSystem.player = gameObject;
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
                if (isRunning) {
                    state = "run";
                } else {
                    state = "go";
                }
            }

        } else if (Joystick.Instance.Horizontal < 0 || Input.GetKeyDown(KeyCode.LeftArrow)) {
            direction = "left";

            if (isRunning) {
                rb2d.velocity = new Vector3(-SPEED * 1.5f, rb2d.velocity.y);
            } else {
                rb2d.velocity = new Vector3(-SPEED, rb2d.velocity.y);
            }

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

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Joystick.Instance.input = new Vector3(-1, 0);
        } 
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Joystick.Instance.input = new Vector3(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            ShootStart();
        }

        if (Input.GetKeyUp(KeyCode.C)) {
            ShootEnd();
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            Dash();
        }

        //check shooting
        if (isShooting && Time.time > nextShoot) {
            nextShoot = Time.time + SHOOT_RATE;
            playerCommand.SpawnBullet(GetHitParam());
        }

        if (direction == "left") {
            transform.localScale = new Vector3(scale, scale);
        } else {
            transform.localScale = new Vector3(-scale, scale);
        }
    }

    public HitParam GetHitParam() {
        HitParam hit = new HitParam();
        hit.dame = 20f;
        hit.direction = direction;
        hit.owner = gameObject;
        hit.ownerTag = tag;
        hit.startPos = weapon.barrel.position;
        hit.targetTags = new List<string>() { "Player" };
        return hit;
    }

    public void Jump() {
        if (jumpCount <= 0)
            return;

        jumpCount--;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(0, JUMP_FORCE));
        state = "jump";
        StartCoroutine(Fall(jumpCount));

        AudioSystem.Instance.PlaySound("Sound/player/dash");
    }

    public IEnumerator Fall(int jumpId) {
        yield return new WaitForSeconds(0.35f);
        if (jumpCount == jumpId) {
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

    public void SetVisible(bool visible) {
        GetComponent<SpriteRenderer>().enabled = visible;
        hand.GetComponent<SpriteRenderer>().enabled = visible;
        weapon.GetComponent<SpriteRenderer>().enabled = visible;
    }

    public void Die() {
        StartCoroutine(DieCoroutine());
    }

    public IEnumerator DieCoroutine() {
        if (isLocalPlayer) {
            SetVisible(false);
        } else {
            GetComponent<MySyncPosition>().puppet.SetVisible(false);
        }
        
        AudioSystem.Instance.PlaySound("Sound/player/dead");
        yield return new WaitForSeconds(3f);

        if (isLocalPlayer) {
            SetVisible(true);
        } else {
            GetComponent<MySyncPosition>().puppet.SetVisible(true);
            hp = 500f;
        }
    }

    public void LoadWeapon(string s) {
        string path = "Weapon/" + s + "/" + s;
        weaponStat = Resources.Load<WeaponStat>(path);
        GetComponent<PlayerCommand>().weaponStat = weaponStat;
        weapon.Init();
    }

    /// <summary>
    /// only call from local player
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet")) {
            HitParam hit = collision.GetComponent<Bullet>().hitParam;

            Rigidbody2D body = rb2d;

            if (!isLocalPlayer) {
                body = GetComponent<MySyncPosition>().puppet.GetComponent<Rigidbody2D>();
            }

            if (hit.direction == "right") {
                body.AddForce(new Vector2(weaponStat.forceBack*2, 0));
            } else {
                body.AddForce(new Vector2(-weaponStat.forceBack*2, 0));
            }

            hp -= hit.dame;
            if (hp < 0) {
                playerCommand.Die();
            }
        }
    }
}
