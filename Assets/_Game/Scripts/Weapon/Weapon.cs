using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Transform barrel;
    public WeaponStat stat;

    public void Init() {
        GetComponent<SpriteRenderer>().sprite = stat.sprite;
        transform.localPosition = stat.localPosision;
    }

    public void Attack(HitParam hit) {
        GameObject bullet = GameSystem.LoadPool(stat.bulletName, barrel.position);

        bullet.GetComponent<Bullet>().hitParam = hit;
        bullet.GetComponent<TrailRenderer>().Clear();

        if (hit.direction == "right") {
            hit.owner.GetComponent<Rigidbody2D>().AddForce(new Vector2(-stat.forceBack, 0));
            bullet.transform.localScale = new Vector3(1, 1);
        } else if (hit.direction == "left") {
            hit.owner.GetComponent<Rigidbody2D>().AddForce(new Vector2(stat.forceBack, 0));
            bullet.transform.localScale = new Vector3(-1, 1);
        }
    }
}
