using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class MobMelee : BattleBehavior
//{
//    bool getHit = false;

//    public override void OnEnable() {
//        base.OnEnable();
//        getHit = false;
//    }

//    private void Update() {
//        if (died)
//            return;

//        if (GameSystem.GetPlayerDistance(transform) > stat.visionRange && getHit == false) {
//            executor.enabled = true;
//        }
//        else if (GameSystem.GetPlayerDistance(transform) > stat.attackRange) {
//            executor.enabled = false;
//            animator.spritesheet = stat.run;

//            Vector3 velocity = GameSystem.GoToTargetVector(transform.position, NetworkSystem.player.transform.position, stat.speed);
//            if (!stat.canFly) {
//                velocity.y = rb2d.velocity.y;
//            }
            
//            rb2d.velocity = velocity;

//            if (velocity.x > 0)
//                spriteRenderer.flipX = true;
//            else if (velocity.x < 0)
//                spriteRenderer.flipX = false;
//        }
//        else {
//            executor.enabled = false;
//            animator.spritesheet = stat.attack;
//            rb2d.velocity = Vector2.zero;

//            if (transform.position.x > NetworkSystem.player.transform.position.x)
//                spriteRenderer.flipX = false;
//            else if (transform.position.x < NetworkSystem.player.transform.position.x)
//                spriteRenderer.flipX = true;
//        }
//    }

//    public override void GetHit(HitParam hit) {
//        base.GetHit(hit);
//        getHit = true;
//    }
//}
