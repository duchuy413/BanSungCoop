using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLongGun : MonoBehaviour, IAttack
{
    public WeaponStat weaponStat;
    public Transform barrel;

    Player player;
    bool isAttacking = false;
    float nextAttack = 0;

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
    }

    public HitParam GetHitParam() {
        HitParam hit = new HitParam();
        hit.dame = 20f;
        hit.direction = player.direction;
        hit.owner = player.gameObject;
        hit.ownerTag = player.tag;
        hit.startPos = barrel.position;
        hit.targetTags = new List<string>() { "Monster" };
        return hit;
    }

    private void Update() {
        if (isAttacking && Time.time > nextAttack) {
            nextAttack = Time.time + weaponStat.attackCountDown;

            HitParam hit = GetHitParam();
            GameObject bullet = GameSystem.LoadPool(weaponStat.bulletName, hit.startPos);

            bullet.GetComponent<Bullet>().hitParam = hit;
            bullet.GetComponent<TrailRenderer>().Clear();
            float scale = bullet.transform.localScale.x;

            if (hit.direction == "right") {
                hit.owner.GetComponent<Rigidbody2D>().AddForce(new Vector2(-weaponStat.forceBack, 0));
                bullet.transform.localScale = new Vector3(scale, scale);
            } else if (hit.direction == "left") {
                hit.owner.GetComponent<Rigidbody2D>().AddForce(new Vector2(weaponStat.forceBack, 0));
                bullet.transform.localScale = new Vector3(-scale, scale);
            }
        }
    }

    public WeaponStat GetWeaponStat() {
        return weaponStat;
    }

    public void UpdateStat(Player player) {
        transform.GetComponentInChildren<DameOnContact>().hit = GetHitParam();
    }
}
