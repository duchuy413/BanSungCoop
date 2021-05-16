using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(FramesAnimator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class MobWorf : MonoBehaviour, IMob {

    public Vector3 Pivot {
        get {
            if (movingPivot == null) {
                Debug.LogError("Moving Pivot not set in object: " + gameObject);
            }
            if (stat.mobType == MobType.Crowded) {
                return movingPivot.transform.position;
            } else {
                return lastGetHitPosition;
            }
        }
    }

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
    private Vector3 lastGetHitPosition;

    private void Awake() {
        animator = GetComponent<FramesAnimator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb2d.gravityScale = GameManager.GRAVITY;
    }

    public virtual void OnEnable() {
        lastGetHitPosition = transform.position;
        gameObject.layer = LayerMask.NameToLayer("Monster");
        attackTarget = null;
        died = false;
        getHit = false;
        LoadLevel(level);
        hpValue.localScale = new Vector3(current.hp / current.maxhp, 1);
    }

    private void Update() {
        if (died)
            return;

        // if is a brutal monster, and be a lonely one, auto attack player when seen
        bool playerSeen = Vector3.Distance(transform.position, NetworkSystem.player.transform.position) < stat.visionRange;

        if (playerSeen && 
            stat.isBrutal && stat.mobType == MobType.Lonely && 
            (attackTarget == null || attackTarget.activeSelf == false)) {

            attackTarget = NetworkSystem.player;
            lastGetHitPosition = transform.position;
        }

        // check change state
        if (Vector3.Distance(transform.position, Pivot) > maxMovePivotRange) {
            state = MobState.Returning;
            getHit = false;
            attackTarget = null;
        }

        if (state == MobState.Returning) {

            if (getHit) {
                state = MobState.Chasing;
            }
            else if (Vector3.Distance(transform.position, Pivot) < minMovePivotRange) {
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

        // If attack target dies, then stop attacking
        if ((state == MobState.Attack || state == MobState.Chasing) && (attackTarget == null || attackTarget.activeSelf == false)) {
            state = MobState.Returning;
            getHit = false;
            attackTarget = null;
        }

        // update base on state
        if (state == MobState.Returning) {
            rb2d.velocity = (Pivot - transform.position) / (Vector3.Distance(Pivot, transform.position)) * stat.speed * 1.5f;
            //executor.enabled = false;
            getHit = false;
            animator.spritesheet = stat.run;

            if (rb2d.velocity.x > 0)
                spriteRenderer.flipX = true;
            else if (rb2d.velocity.x < 0)
                spriteRenderer.flipX = false;

            return;
        }

        if (state == MobState.Free) {
            //executor.enabled = false;

            if (Time.time > nextChangeMovement || Vector3.Distance(targetFreeMovement, transform.position) < 0.5f) {
                nextChangeMovement = Time.time + Random.Range(2f, 5f);
                targetFreeMovement = Pivot +  new Vector3(Random.Range (-maxMovePivotRange, maxMovePivotRange) * 0.5f, Random.Range(-maxMovePivotRange, maxMovePivotRange) * 0.25f);
                action = Random.Range(0, 5) == 0 ? MobAction.Run : MobAction.Go;
            }

            //executor.enabled = false;
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
            //executor.enabled = false;
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
            //executor.enabled = false;
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

    public CharacterStat GetBaseStat() {
        return stat;
    }

    public BattleStat GetCurrentStat() {
        return current;
    }

    public void SetStatData(CharacterStat stat) {
        this.stat = stat;
    }

    public void LoadLevel(int level) {
        textName.text = "lv" + level.ToString() + "." + stat.characterName;

        current = GameSystem.LoadLevel(stat, level);
    }

    public virtual void GetHit(HitParam hit) {
        if (!hit.targetTags.Contains(gameObject.tag)) {
            return;
        }

        lastGetHitPosition = transform.position;
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
        //executor.enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        yield return new WaitForSeconds(sec);

        //if (!died) {
        //    executor.enabled = true;
        //}
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
        gameObject.layer = LayerMask.NameToLayer("OnlyCollisionGround");
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

        if (gameObject.CompareTag("Monster")) {
            NetworkSystem.player.GetComponent<Player>().GainExp(current.currentExp);
            Gameplay.AddGold(current.goldGainWhenKill);
            Gameplay.AddFood(current.foodGainWhenKill);

            if (hitParam.owner.CompareTag("Pet")) {
                hitParam.owner.GetComponent<IMob>().GainExp(current.expGainWhenKill);
            }
        }

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

    public void GainExp(float amount) {
        current.currentExp += amount;
        if (current.currentExp > current.nextLvlExp) {
            level++;
            LoadLevel(level);
            //weapon.UpdateStat(this);
            LeanTween.delayedCall(2f, () => {
                GameSystem.TextFly("Level Up", transform.position + new Vector3(0, 1f), "blue");
            });
        }
        //textName.text = current.currentExp + "/" + current.nextLvlExp;
        GameSystem.TextFly("+" + (int)amount + "exp", transform.position + new Vector3(0, 1f), "blue");
    }
}
