using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(MovementExecutor))]
[RequireComponent(typeof(FramesAnimator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class MobWorf : MonoBehaviour
{
    public CharacterStat stat;
    public int level;
    public TextMeshPro textName;
    public Transform hpValue;

    private MovementExecutor executor;
    private FramesAnimator animator;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    private HitParam hitParam;
    private BattleStat current;
    private bool died;
    private bool getHit = false;
    private float nextAttack = 0f;

    private void Awake() {
        executor = GetComponent<MovementExecutor>();
        animator = GetComponent<FramesAnimator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb2d.gravityScale = GameManager.GRAVITY;
    }

    public virtual void OnEnable() {
        gameObject.layer = LayerMask.NameToLayer("Monster");

        died = false;
        getHit = false;
        LoadLevel(level);

        executor = GetComponent<MovementExecutor>();
        if (executor != null) {
            executor.enabled = true;
            GetComponent<FramesAnimator>().enabled = true;
        }
        //LeanTween.scale(gameObject, new Vector3(10f, 10f), 3f).setLoopPingPong();
    }

    private void Update() {
        if (died)
            return;

        if (GameSystem.GetPlayerDistance(transform) > stat.visionRange && getHit == false) {
            executor.enabled = true;
        } else if (GameSystem.GetPlayerDistance(transform) > stat.attackRange) {
            executor.enabled = false;
            animator.spritesheet = stat.run;

            Vector3 velocity = GameSystem.GoToTargetVector(transform.position, NetworkSystem.player.transform.position, stat.speed);
            //if (!stat.canFly) {
            //    velocity.y = rb2d.velocity.y;
            //}

            rb2d.velocity = velocity;

            if (velocity.x > 0)
                spriteRenderer.flipX = true;
            else if (velocity.x < 0)
                spriteRenderer.flipX = false;
        } else {
            executor.enabled = false;
            animator.spritesheet = stat.attack;
            rb2d.velocity = Vector2.zero;

            if (transform.position.x > NetworkSystem.player.transform.position.x)
                spriteRenderer.flipX = false;
            else if (transform.position.x < NetworkSystem.player.transform.position.x)
                spriteRenderer.flipX = true;

            if (Time.time > nextAttack) {
                nextAttack = Time.time + stat.attackCountDown;
                HitParam hit = new HitParam {
                    targetTags = new List<string> { "Player" },
                    dame = current.dame,
                };

                NetworkSystem.player.SendMessage("GetHit", hit, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void LoadLevel(int level) {
        textName.text = "lv" + level.ToString() + "." + stat.characterName;

        current = new BattleStat();
        current.speed = stat.speed;
        current.baseExp = stat.baseExp;
        current.currentExp = stat.baseExp * Mathf.Pow(1.1f, level);
        current.nextLvlExp = stat.baseExp * Mathf.Pow(1.1f, level + 1);
        current.hp = stat.hp * Mathf.Pow(1.1f, level);
        current.maxhp = stat.hp * Mathf.Pow(1.1f, level);
        current.dame = stat.dame * Mathf.Pow(1.1f, level);
        current.attackRange = stat.attackRange;
        current.visionRange = stat.visionRange;
        current.attackCountDown = stat.attackCountDown;
    }

    public virtual void GetHit(HitParam hit) {
        getHit = true;
        //Debug.Log(current);
        //Debug.Log(current.hp);

        float dameTake = CalculateDame(hit);
        GameSystem.TextFly(Convert.ToInt32(dameTake).ToString(), transform.position);

        //GameObject flyingtext = GameSystem.LoadPool("textdame", textName.transform.position);
        //flyingtext.GetComponent<TextMeshPro>().text = Convert.ToInt32(dameTake).ToString();

        current.hp -= dameTake;

        if (current.hp <= 0) {
            current.hp = 0;
            Died(hit);
            return;
        } else {
            StartCoroutine(PauseMovement(0.1f));
        }

        hpValue.localScale = new Vector3(current.hp / current.maxhp, 1);
    }

    IEnumerator PauseMovement(float sec) {
        executor.enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        yield return new WaitForSeconds(sec);

        if (!died) {
            executor.enabled = true;
        }
    }

    public float CalculateDame(HitParam hit) {
        return hit.dame;
    }

    public void ApplyDame(GameObject target) {
        HitParam hit = new HitParam();
        hit.dame = current.dame;
        hit.owner = gameObject;
        hit.ownerTag = gameObject.tag;

        target.SendMessage("GetHit", hit, SendMessageOptions.DontRequireReceiver);
    }

    public void Died(HitParam hitParam) {
        died = true;
        executor.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("OnlyCollisionGround");
        GetComponent<MovementExecutor>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<FramesAnimator>().spritesheet = stat.die;

        if (hitParam.direction == "left") {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-700f, 700f));
        } else if (hitParam.direction == "right") {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(700f, 700f));
        }

        GameSystem.TextFly(current.baseExp.ToString(), transform.position, "blue");

        Invoke("Disappear", 2f);
    }

    public void Disappear() {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DameOnContact dame = collision.GetComponentInChildren<DameOnContact>();
        if (dame != null && dame.hit.targetTags.Contains(gameObject.tag)) {
            GetHit(dame.hit);
        }
    }
}
