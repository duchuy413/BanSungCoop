using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHammer : MonoBehaviour, IAttack {
    public static float ATTACK_TIME_RATE = 0.2f; //between 0 and 1, lower is faster
    public static float DELAY_AFTER_ATTACK = 0.6f;
    public static float ANTICIPATE_HAND_RANGE = 0.2f;

    public static float ROTATE_ORIGINAL = -170f;
    public static float ROTATE_START_ATTACK = 190f;

    public WeaponStat weaponStat;

    Player player;
    Vector3 originalHand;

    bool isAttacking = false;
    float nextAttack = 0;
    float attackTime;

    public void Attack() {
        throw new System.NotImplementedException();
    }

    public void AttackButtonDown() {
        isAttacking = true;
    }

    public void AttackButtonUp() {
        isAttacking = false;
    }

    public void Init(Player player) {
        this.player = player;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = weaponStat.sprite;
        originalHand = transform.parent.localPosition;
        attackTime = weaponStat.attackCountDown * ATTACK_TIME_RATE;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, ROTATE_ORIGINAL);
    }

    public HitParam GetHitParam() {
        HitParam hit = new HitParam();
        hit.dame = 20f;
        hit.direction = player.direction;
        hit.owner = player.gameObject;
        hit.ownerTag = player.tag;
        hit.targetTags = new List<string>() { "Monster" };
        return hit;
    }

    private void Update() {
        if (isAttacking && Time.time > nextAttack) {
            nextAttack = Time.time + weaponStat.attackCountDown;

            transform.localRotation = Quaternion.Euler(0, 0, ROTATE_START_ATTACK);
            transform.parent.localPosition = originalHand + new Vector3(ANTICIPATE_HAND_RANGE, 0);
            Vector3 targetHandPosition = originalHand + new Vector3(- ANTICIPATE_HAND_RANGE, 0);

            if (isAttacking) {
                LeanTween.rotateZ(gameObject, 0, attackTime).setEaseInExpo();
                LeanTween.moveLocalX(transform.parent.gameObject, targetHandPosition.x, attackTime);
                LeanTween.delayedCall(attackTime + DELAY_AFTER_ATTACK, () => {
                    transform.localRotation = Quaternion.Euler(0, 0, ROTATE_ORIGINAL);
                    transform.parent.localPosition = originalHand;
                });
            }
        }
    }

    public WeaponStat GetWeaponStat() {
        return weaponStat;
    }
}
