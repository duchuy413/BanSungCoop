using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public HitParam hitParam;

    public float speed = 30f;

    void Update() {
        //float distance = Vector3.Distance(transform.position, DGameSystem.player.transform.position);
        if (Mathf.Abs(transform.position.x) >20f)
            gameObject.SetActive(false);

        if (hitParam.direction == "left")
            transform.position -= new Vector3(speed * Time.deltaTime, 0);
        else if (hitParam.direction == "right")
            transform.position += new Vector3(speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (hitParam.targetTags.Contains(collision.gameObject.tag)) {
            collision.gameObject.SendMessage("GetHit", hitParam);
            //DGameSystem.LoadPool("HitParticle", transform.position);
            gameObject.SetActive(false);
        }
    }
}
