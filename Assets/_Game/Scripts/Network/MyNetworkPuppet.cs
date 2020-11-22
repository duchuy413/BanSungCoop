using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MyNetworkPuppet : MonoBehaviour {
    public static float MAX_SHOOT_RADIUS = 150f;

    public float FAST_SPEED_1 = 2f;
    public float FAST_SPEED_2 = 5f;

    public bool showDebug = true;

    public GameObject target;
    public float maxDistance = 4f;
    public float speed = 25f;
    private float scale;

    private Vector3 vectorToTarget;
    private Rigidbody2D rb2d;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
        rb2d.gravityScale = 0;
        scale = Mathf.Abs(transform.localScale.x);
    }

    private void Update() {
        //if (rb2d.velocity.x > 0) {
        //    transform.localScale = new Vector3(-scale, scale);
        //} else if (rb2d.velocity.x < 0) {
        //    transform.localScale = new Vector3(scale, scale);
        //}
    }

    void LateUpdate() {
        if (target == null)
            return;

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (target != null)
            GoToTargetPosition(maxDistance);
    }

    void GoToTargetPosition(float maxDistanceToTarget) {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= maxDistanceToTarget) {
            rb2d.velocity = new Vector3(0f, 0f);
            return;
        } else {
            vectorToTarget = target.transform.position - transform.position;
            vectorToTarget = vectorToTarget/ distanceToTarget;

            if (distanceToTarget <= 1f) {
                rb2d.velocity = vectorToTarget * speed;
            } else if (distanceToTarget <= 2f) {
                rb2d.velocity = vectorToTarget * speed * FAST_SPEED_1;
            } else {
                rb2d.velocity = vectorToTarget * speed * FAST_SPEED_2;
            }

            //if (showDebug) {
            //    Debug.Log("DISTANCE: " + distanceToTarget);
            //}
            
        }
    }

    public void MoveTo(Transform target) {
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.y = target.position.y;
        transform.position = pos;
    }
}
