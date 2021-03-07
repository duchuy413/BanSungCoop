using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DameOnContact : MonoBehaviour
{
    public HitParam hit;
    public IAttack weapon;
    //public List<string> targets;

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    if (hit.targetTags.Contains(collision.gameObject.tag)) {
    //        SendMessage("GetHit", hit, SendMessageOptions.DontRequireReceiver);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    //if (targets.Contains(collision.tag)){
    //    //    SendMessage("GetHit", hit, SendMessageOptions.DontRequireReceiver);
    //    //}
    //    if (hit.targetTags.Contains(collision.tag)) {
    //        SendMessage("GetHit", hit, SendMessageOptions.DontRequireReceiver);
    //    }
    //}
}
