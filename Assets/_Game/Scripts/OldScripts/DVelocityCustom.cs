using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVelocityCustom : MonoBehaviour
{
    bool animating = false;

    Rigidbody2D rb2d;
    float groundBase;
    bool isThrowing = false;
    float hValue = 0;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Throw(string direction, float verticalForce, float horizontalForce) {
        isThrowing = true;
        groundBase = transform.position.y;
        transform.gameObject.layer = GameConstants.LAYER_JUMPING;

        rb2d.velocity = new Vector3(rb2d.velocity.x, verticalForce);

        if (direction == "left")
            hValue = -horizontalForce;
        else
            hValue = horizontalForce;
    }

    private void Update() {
        if (isThrowing) {
            rb2d.velocity = new Vector2(hValue, rb2d.velocity.y - 10f * Time.deltaTime);

            if (transform.position.y < groundBase) {
                isThrowing = false;
                transform.gameObject.layer = GameConstants.LAYER_GROUND;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            }
        }
    }
}
