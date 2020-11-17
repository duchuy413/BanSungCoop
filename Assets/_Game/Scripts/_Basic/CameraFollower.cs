using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CameraFollower : MonoBehaviour
{
    public static float MAX_SHOOT_RADIUS = 150f;
    public static CameraFollower Instance;

    public GameObject target;
    public float maxDistance = 4f;
    public float speed = 25f;

    private Vector3 vectorToTarget;
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.isKinematic = true;
        rigidbody2D.gravityScale = 0;
        Instance = this;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget < 3f)
        {
            target = null;
        }

        if (target != null)
            GoToTargetPosition(maxDistance);
    }

    void GoToTargetPosition(float maxDistanceToTarget)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= maxDistanceToTarget)
        {
            rigidbody2D.velocity = new Vector3(0f, 0f);
            return;
        }
        else
        {
            vectorToTarget = target.transform.position - transform.position;
            vectorToTarget = vectorToTarget * speed / distanceToTarget;
            rigidbody2D.velocity = vectorToTarget;
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
