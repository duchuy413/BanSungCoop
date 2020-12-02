using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MyNetworkPuppet : MonoBehaviour {
    public GameObject target;
    public Player player;
    public SpriteRenderer weaponImg;

    public Transform hand;
    public Weapon weapon;
    public Transform barrel;

    public float FAST_SPEED_1 = 2f;
    public float FAST_SPEED_2 = 5f;
    public float maxDistance = 4f;
    public float speed = 25f;

    private float scale;
    private Vector3 vectorToTarget;
    private Rigidbody2D rb2d;

    float nextShoot = 0f;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
        rb2d.gravityScale = 0;
        scale = Mathf.Abs(transform.localScale.x);
    }

    private void Update() {
        //check shooting
        if (player.isShooting && Time.time > nextShoot) {
            nextShoot = Time.time + Player.SHOOT_RATE;
            HitParam hit = player.GetHitParam();
            hit.startPos = barrel.position;
            player.GetComponent<PlayerCommand>().SpawnBullet(hit);
        }
    }

    void LateUpdate() {
        if (target == null)
            return;

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
        }
    }

    public void MoveTo(Transform target) {
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.y = target.position.y;
        transform.position = pos;
    }

    public void LoadWeapon(WeaponStat stat) {
        weaponImg.sprite = stat.sprite;
    }

    public void SetVisible(bool visible) {
        GetComponent<SpriteRenderer>().enabled = visible;
        hand.GetComponent<SpriteRenderer>().enabled = visible;
        weapon.GetComponent<SpriteRenderer>().enabled = visible;
    }
}
