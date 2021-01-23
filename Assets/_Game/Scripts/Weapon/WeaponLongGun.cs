using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLongGun : MonoBehaviour, IAttack
{
    public WeaponStat weaponStat;
    public HitParam hit;

    bool isAttacking = false;
    float nextAttack = 0;

    public void Attack(HitParam hit) {
        throw new System.NotImplementedException();
    }

    public void AttackButtonDown() {
        isAttacking = true;
    }

    public void AttackButtonUp() {
        isAttacking = false;
    }

    private void Update() {
        if (isAttacking && Time.time > nextAttack) {
            MyDebug.Log("Calling Spawn Bullet");
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
}
