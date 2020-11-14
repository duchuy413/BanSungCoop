using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : NetworkBehaviour {
    public static float SPEED = 10f;
    public static float JUMP_FORCE = 1000f;
    public static float SHOOT_RATE = 0.2f;

    public string state = "";
    public Rigidbody2D rb2d;
    public Transform barrel;

    bool isShooting = false;
    float nextShoot = 0f;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();    
    }

    public override void OnStartLocalPlayer() {
        if (isLocalPlayer) {
            NetworkSystem.player = gameObject;
            CameraFollower.Instance.target = gameObject;
        }
    }

    void Update() {
        if (Joystick.Instance.Horizontal > 0 || Input.GetKeyDown(KeyCode.RightArrow)) {
            GetComponent<SpriteRenderer>().flipX = false;
            rb2d.velocity = new Vector3(SPEED, rb2d.velocity.y);
        }
        else if (Joystick.Instance.Horizontal < 0 || Input.GetKeyDown(KeyCode.LeftArrow)) {
            GetComponent<SpriteRenderer>().flipX = true;
            rb2d.velocity = new Vector3(-SPEED, rb2d.velocity.y);
        }
        else
            rb2d.velocity = new Vector3(0, rb2d.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        if (isShooting && Time.time > nextShoot) {
            nextShoot = Time.time + SHOOT_RATE;

            //GameObject bullet = Instantiate(Resources.Load<GameObject>("bullet"), transform.position, Quaternion.identity);
            GameObject bullet = Gameplay.LoadPool("bullet", barrel.position);
            Debug.Log("SHOOT!");
        }
    }

    public void Jump() {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(0, JUMP_FORCE));
    }

    public void ShootStart() {
        isShooting = true;
    }

    public void ShootEnd() {
        isShooting = false;
    }

    public void UseSkill() {
        Debug.Log("USE SKILL!");
    }
}
