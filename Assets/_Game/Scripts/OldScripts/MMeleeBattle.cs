using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMeleeBattle : DBattle
{
    public DMovement movement;
    public DMovementExecutor executor;
    public FramesAnimator animator;
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;

    public bool getHit = false;

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
            rigidbody2D.velocity = velocity;

            if (velocity.x > 0)
                spriteRenderer.flipX = true;
            else if (velocity.x < 0)
                spriteRenderer.flipX = false;

        }
        else {
            movement.enabled = false;
            executor.enabled = false;
            animator.spritesheet = stat.attack;
            rigidbody2D.velocity = Vector2.zero;

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
