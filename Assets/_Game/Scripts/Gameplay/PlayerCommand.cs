using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCommand : NetworkBehaviour
{
    public WeaponStat weaponStat;

    public void Dash(Vector2 force) {
        CmdDash(force);
    }

    public void Attack(HitParam hit) {
        CmdAttack(hit);
    }

    [Command]
    public void CmdDash(Vector2 force) {
        GetComponent<Rigidbody2D>().AddForce(force);
        GameSystem.LoadPool("Effect/dash", transform.position);
        RpcDash(force);
    }

    [ClientRpc]
    public void RpcDash(Vector2 force) {
        GetComponent<Rigidbody2D>().AddForce(force);
        GameSystem.LoadPool("Effect/dash", transform.position);
    }

    [Command]
    public void CmdAttack(HitParam hit) {
        MyDebug.Log("Calling CMD Attack");
        SpawnBullet(hit);
        RpcAttack(hit);
    }

    [ClientRpc]
    public void RpcAttack(HitParam hit) {
        MyDebug.Log("Calling RPC Attack");
        SpawnBullet(hit);
    }

    public void SpawnBullet(HitParam hit) {
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
