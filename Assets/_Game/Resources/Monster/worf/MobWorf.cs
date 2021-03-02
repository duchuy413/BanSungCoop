using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public enum MobState { Free, Returning, Chasing, Attack }
public enum MobAction { Go, Run }

[RequireComponent(typeof(MovementExecutor))]
[RequireComponent(typeof(FramesAnimator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class MobWorf : MonoBehaviour, IMob {
    public Transform movingPivot;
    public float maxMovePivotRange = 25f;
    public float minMovePivotRange = 5f;

    public MobState state = MobState.Free;
    public MobAction action = MobAction.Go;

    public CharacterStat stat;
    public int level;
    public TextMeshPro textName;
    public Transform hpValue;
    public GameObject attackTarget;

    private MovementExecutor executor;
    private FramesAnimator animator;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    private HitParam hitParam;
    private BattleStat current;
    private bool died;
    private bool getHit = false;
    private float nextAttack = 0f;

    private float nextChangeMovement;
    private Vector3 targetFreeMovement;


    private void Awake() {
        executor = GetComponent<MovementExecutor>();
        animator = GetComponent<FramesAnimator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb2d.gravityScale = GameManager.GRAVITY;
    }

    public virtual void OnEnable() {
        gameObject.layer = LayerMask.NameToLayer("Monster");
        attackTarget = null;
        died = false;
        getHit = false;
        LoadLevel(level);

        executor = GetComponent<MovementExecutor>();
        if (executor != null) {
            executor.enabled = true;
            GetComponent<FramesAnimator>().enabled = true;
        }
    }

    private void Update() {
        if (died)
            return;

        // check change state
        if (Vector3.Distance(transform.position, movingPivot.position) > maxMovePivotRange) {
            state = MobState.Returning;
            getHit = false;
            attackTarget = null;
        }

        if (state == MobState.Returning) {

            if (getHit) {
                state = MobState.Chasing;
            }
            else if (Vector3.Distance(transform.position, movingPivot.position) < minMovePivotRange) {
                rb2d.velocity = Vector2.zero;
                state = MobState.Free;
            }
        }

        if (getHit && state != MobState.Returning || attackTarget != null && attackTarget.activeSelf == true) {
            if (Vector3.Distance(transform.position, attackTarget.transform.position) > stat.attackRange) {
                state = MobState.Chasing;
            } else {
                state = MobState.Attack;
            }
        }

        if ((state == MobState.Attack || state == MobState.Chasing) && (attackTarget == null || attackTarget.activeSelf == false)) {
            state = MobState.Returning;
            getHit = false;
            attackTarget = null;
            //nextChangeMovement = Time.time + Random.Range(2f, 5f);
            //targetFreeMovement = movingPivot.position + new Vector3(Random.Range(-maxMovePivotRange, maxMovePivotRange) * 0.5f, Random.Range(-maxMovePivotRange, maxMovePivotRange) * 0.25f);
            //action = Random.Range(0, 5) == 0 ? MobAction.Run : MobAction.Go;
        }

        // update base on state
        if (state == MobState.Returning) {
            rb2d.velocity = (movingPivot.position - transform.position) / (Vector3.Distance(movingPivot.position, transform.position)) * stat.speed * 1.5f;
            executor.enabled = false;
            getHit = false;
            animator.spritesheet = stat.run;

            if (rb2d.velocity.x > 0)
                spriteRenderer.flipX = true;
            else if (rb2d.velocity.x < 0)
                spriteRenderer.flipX = false;

            return;
        }

        if (state == MobState.Free) {
            executor.enabled = false;

            if (Time.time > nextChangeMovement || Vector3.Distance(targetFreeMovement, transform.position) < 0.5f) {
                nextChangeMovement = Time.time + Random.Range(2f, 5f);
                targetFreeMovement = movingPivot.position +  new Vector3(Random.Range (-maxMovePivotRange, maxMovePivotRange) * 0.5f, Random.Range(-maxMovePivotRange, maxMovePivotRange) * 0.25f);
                action = Random.Range(0, 5) == 0 ? MobAction.Run : MobAction.Go;
            }

            executor.enabled = false;
            getHit = false;

            if (action == MobAction.Go) {
                rb2d.velocity = (targetFreeMovement - transform.position) / (Vector3.Distance(targetFreeMovement, transform.position)) * stat.speed;
                animator.spritesheet = stat.go;
            } else if (action == MobAction.Run) {
                rb2d.velocity = (targetFreeMovement - transform.position) / (Vector3.Distance(targetFreeMovement, transform.position)) * stat.speed * 1.5f;
                animator.spritesheet = stat.run;
            }

            if (rb2d.velocity.x > 0)
                spriteRenderer.flipX = true;
            else if (rb2d.velocity.x < 0)
                spriteRenderer.flipX = false;
        }

        if (state == MobState.Chasing) {
            executor.enabled = false;
            animator.spritesheet = stat.run;

            Vector3 velocity = GameSystem.GoToTargetVector(transform.position, attackTarget.transform.position, stat.speed) * 1.5f;

            rb2d.velocity = velocity;

            if (velocity.x > 0)
                spriteRenderer.flipX = true;
            else if (velocity.x < 0)
                spriteRenderer.flipX = false;

            return;
        }

        if (state == MobState.Attack) {
            executor.enabled = false;
            animator.spritesheet = stat.attack;
            rb2d.velocity = Vector2.zero;

            if (transform.position.x > attackTarget.transform.position.x)
                spriteRenderer.flipX = false;
            else if (transform.position.x < attackTarget.transform.position.x)
                spriteRenderer.flipX = true;

            if (Time.time > nextAttack) {
                nextAttack = Time.time + stat.attackCountDown;
                List<string> targetTags = gameObject.tag == "Pet" ? new List<string> { "Monster" } : new List<string> { "Player", "Pet" };
                HitParam hit = new HitParam();
                hit.targetTags = targetTags;
                hit.dame = current.dame;
                hit.owner = gameObject;

                attackTarget.SendMessage("GetHit", hit, SendMessageOptions.DontRequireReceiver);
            }

            return;
        }
    }

    public CharacterStat GetStatData() {
        return stat;
    }

    public void SetStatData(CharacterStat stat) {
        this.stat = stat;
    }

    public void LoadLevel(int level) {
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
        if (!hit.targetTags.Contains(gameObject.tag)) {
            return;
        }

        Debug.Log("Get hit from " + hit.owner.name);
        getHit = true;
        attackTarget = hit.owner;

        MonsterSpawner spawner = movingPivot.GetComponent<MonsterSpawner>();
        if (spawner != null) {
            Gameplay.Instance.AddAttackTargets(spawner.monsters);
        }

        if (hit.owner.tag == "Player" && Gameplay.Instance.pets.Count > 0) {
            MonsterSpawner spawner1 = movingPivot.GetComponent<MonsterSpawner>();
            if (spawner1 != null) {
                spawner1.AddAttackTargets(Gameplay.Instance.pets);
            }
        }

        float dameTake = CalculateDame(hit);
        GameSystem.TextFly(Convert.ToInt32(dameTake).ToString(), transform.position);

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

        LeanTween.delayedCall(1f, () => {
            GameSystem.TextFly(current.baseExp.ToString(), NetworkSystem.player.transform.position, "blue");
        });

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

    public GameObject GetGameObject() {
        return gameObject;
    }

    public void SetAttackTarget(GameObject attackTarget, bool forceChangeTarget = false) {
        if (this.attackTarget != null && !forceChangeTarget) {
            return;
        }

        if (attackTarget == null || attackTarget.activeSelf == false) {
            return;
        }

        this.attackTarget = attackTarget;
    }

    public void SetFollow(Transform pivot, float minDistance, float maxDistance) {
        movingPivot = pivot;
        this.minMovePivotRange = minDistance;
        this.maxMovePivotRange = maxDistance;
    }
}
