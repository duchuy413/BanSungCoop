using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CameraFollower : MonoBehaviour
{
    public static float MAX_SHOOT_RADIUS = 150f;
    public static CameraFollower Instance;

    public float TOP = 22f;
    public float BOTTOM = -15f;
    public float LEFT = -35f;
    public float RIGHT = 30f;

    public GameObject target;
    public float maxDistance = 4f;
    public float speed = 25f;

    private Vector3 vectorToTarget;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
        rb2d.gravityScale = 0;
        Instance = this;
    }

    void LateUpdate()
    {
        if (target == null && NetworkSystem.player != null) {
            target = NetworkSystem.player;
            Vector3 pos = target.transform.position;
            transform.position = new Vector3(pos.x, pos.y, -10);
            ParallelBackground[] list = GetComponentsInChildren<ParallelBackground>();
            for (int i = 0; i < list.Length; i++) {
                list[i].enabled = true;
            }
        }

        if (target == null) {
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (target != null)
            GoToTargetPosition(maxDistance);
    }

    void GoToTargetPosition(float maxDistanceToTarget)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= maxDistanceToTarget)
        {
            rb2d.velocity = new Vector3(0f, 0f);
            return;
        } 
        else
        {
            vectorToTarget = target.transform.position - transform.position;
            vectorToTarget = vectorToTarget / distanceToTarget;

            if (target.transform.position.x < LEFT || target.transform.position.x > RIGHT) {
                vectorToTarget = new Vector3(0, vectorToTarget.y);
            }

            if (target.transform.position.y < BOTTOM || target.transform.position.y > TOP) {
                vectorToTarget = new Vector3(vectorToTarget.x, 0);
            }

            if (distanceToTarget < 5f) {
                rb2d.velocity = vectorToTarget * speed;
            } else if (distanceToTarget < 10f) {
                rb2d.velocity = vectorToTarget * 2 * speed;
            } else {
                rb2d.velocity = vectorToTarget * 5 * speed;
            }
        }
    }

    public void MoveTo(Transform target)
    {
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.y = target.position.y;
        transform.position = pos;
    }
}
