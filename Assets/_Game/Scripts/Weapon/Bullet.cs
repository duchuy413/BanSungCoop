using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public HitParam hitParam;

    public float speed = 30f;

    private void OnEnable() {
        Invoke("SetActiveFalse", 2f);
        GetComponent<AudioSource>().Play();
    }

    void Update() {
        //if (Mathf.Abs(transform.position.x) >20f)
        //    gameObject.SetActive(false);

        if (hitParam.direction == "left")
            transform.position -= new Vector3(speed * Time.deltaTime, 0);
        else if (hitParam.direction == "right")
            transform.position += new Vector3(speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (hitParam.targetTags.Contains(collision.gameObject.tag)) {
            collision.gameObject.SendMessage("GetHit", hitParam, SendMessageOptions.DontRequireReceiver);
            gameObject.SetActive(false);
        }
    }

    public void SetActiveFalse() {
        gameObject.SetActive(false);
    }
}
