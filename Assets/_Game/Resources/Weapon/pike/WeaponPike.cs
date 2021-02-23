using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPike : MonoBehaviour, IAttack {
    public static float ATTACK_RANGE_CHANGE = -0.6f;
    public static float DELAY_TIME_FAST_RATE = 0.5f;

    public static float ATTACK_RANGE_CHANGE_BIG = -0.75f;
    public static float DELAY_TIME_BIG_RATE = 1.5f;

    public static float ATTACK_RANGE_CHANGE_BIGGEST = -1.2f;
    public static float DELAY_TIME_BIGGEST_RATE = 2.5f;

    public static float RETURN_TIME = 0.1f;
    public static float RETURN_TIME_SPEED = 0.5f; //between 0 and 1, lower is faster

    public WeaponStat weaponStat;

    Player player;
    Vector3 attackPos;
    Vector3 originalHand;

    bool isAttacking = false;
    float nextAttack = 0;
    int attackCount;

    public void Attack() {
        throw new System.NotImplementedException();
    }

    public void AttackButtonDown() {
        isAttacking = true;
    }

    public void AttackButtonUp() {
        isAttacking = false;
        attackCount = 0;
        nextAttack = 0;
    }

    public void Init(Player player) {
        this.player = player;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = weaponStat.sprite;
        transform.GetChild(0).GetComponent<DameOnContact>().hit = GetHitParam();
        originalHand = transform.parent.localPosition;
        RETURN_TIME = weaponStat.attackCountDown * RETURN_TIME_SPEED;
    }

    public HitParam GetHitParam() {
        HitParam hit = new HitParam();
        hit.dame = player.current.dame;
        hit.direction = player.direction;
        hit.owner = player.gameObject;
        hit.ownerTag = player.tag;
        hit.targetTags = new List<string>() { "Monster" };
        return hit;
    }

    private void Update() {
        if (isAttacking && Time.time > nextAttack) {
            attackCount++;

            if (transform.parent.localPosition != originalHand) {
                transform.parent.localPosition = originalHand;
            }

            if (attackCount > 7) {
                attackCount = 0;
            }

            bool isBigAttack = attackCount == 7;
            bool isFastAttack = attackCount == 4 || attackCount == 5 || attackCount == 6;
            bool isBiggestAttack = attackCount == 0;

            Debug.Log(attackCount);

            if (isBigAttack) {
                isBigAttack = true;
                nextAttack = Time.time + weaponStat.attackCountDown * DELAY_TIME_BIG_RATE;
            } else if (isFastAttack) {
                nextAttack = Time.time + weaponStat.attackCountDown * DELAY_TIME_FAST_RATE;
            } else if (isBiggestAttack) {
                nextAttack = Time.time + weaponStat.attackCountDown * DELAY_TIME_BIGGEST_RATE;
            } else {
                nextAttack = Time.time + weaponStat.attackCountDown;
            }

            if (isBigAttack) {
                attackPos = originalHand + new Vector3(ATTACK_RANGE_CHANGE_BIG, 0);
                transform.parent.localPosition = attackPos;

                float delay = weaponStat.attackCountDown * (DELAY_TIME_BIG_RATE - 1f);

                LeanTween.delayedCall(delay, () => {
                    LeanTween.moveLocalX(transform.parent.gameObject, originalHand.x, RETURN_TIME);
                });
            } else if (isBiggestAttack) {
                attackPos = originalHand + new Vector3(ATTACK_RANGE_CHANGE_BIGGEST, 0);
                transform.parent.localPosition = attackPos;

                float delay = weaponStat.attackCountDown * (DELAY_TIME_BIGGEST_RATE - 1f);

                LeanTween.delayedCall(delay, () => {
                    LeanTween.moveLocalX(transform.parent.gameObject, originalHand.x, RETURN_TIME);
                });
            } else {
                attackPos = originalHand + new Vector3(ATTACK_RANGE_CHANGE, 0);
                transform.parent.localPosition = attackPos;
                LeanTween.moveLocalX(transform.parent.gameObject, originalHand.x, RETURN_TIME);
            }
        }
    }

    public WeaponStat GetWeaponStat() {
        return weaponStat;
    }
}
