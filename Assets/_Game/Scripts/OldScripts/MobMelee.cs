using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMelee : BattleBehavior
{
    private DMovement movement;
    private DMovementExecutor executor;
    private FramesAnimator animator;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    public bool getHit = false;

    private void Start() {
        movement = GetComponent<DMovement>();
        executor = GetComponent<DMovementExecutor>();
        animator = GetComponent<FramesAnimator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb2d.gravityScale = GameManager.GRAVITY;
    }

    public override void OnEnable() {
        base.OnEnable();
        getHit = false;
    }

    private void Update() {
        if (died)
            return;

        if (GameSystem.GetPlayerDistance(transform) > stat.visionRange && getHit == false) {
            movement.enabled = true;
            executor.enabled = true;
        }
        else if (GameSystem.GetPlayerDistance(transform) > stat.attackRange) {
            movement.enabled = false;
            executor.enabled = false;
            animator.spritesheet = stat.run;

            Vector3 velocity = GameSystem.GoToTargetVector(transform.position, NetworkSystem.player.transform.position, stat.speed);
            if (!stat.canFly) {
                velocity.y = rb2d.velocity.y;
            }
            
            rb2d.velocity = velocity;

            if (velocity.x > 0)
                spriteRenderer.flipX = true;
            else if (velocity.x < 0)
                spriteRenderer.flipX = false;

        }
        else {
            movement.enabled = false;
            executor.enabled = false;
            animator.spritesheet = stat.attack;
            rb2d.velocity = Vector2.zero;

            if (transform.position.x > NetworkSystem.player.transform.position.x)
                spriteRenderer.flipX = false;
            else if (transform.position.x < NetworkSystem.player.transform.position.x)
                spriteRenderer.flipX = true;

            Debug.Log(gameObject.name + "is attacking!");
        }
    }

    public override void GetHit(HitParam hit) {
        base.GetHit(hit);
        getHit = true;
    }

}
