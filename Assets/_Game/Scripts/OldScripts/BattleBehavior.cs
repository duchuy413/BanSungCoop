﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class BattleBehavior : MonoBehaviour {
    public CharacterStat stat;
    public int level;
    public GameObject attackObj;
    public TextMeshPro textName;
    public bool isAnimating = false;

    private MovementExecutor executor;
    private FramesAnimator animator;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    private HitParam hitParam;
    private BattleStat current;
    private bool died;

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
        current = new BattleStat();
        LoadLevel(level);

        executor = GetComponent<MovementExecutor>();
        if (executor != null) {
            executor.enabled = true;
            GetComponent<FramesAnimator>().enabled = true;
        }
    }

    private void LoadLevel(int level) {
        Debug.Log("THIS IS LOAD LEVEL");

        textName.text = "lv" + level.ToString() + "." + stat.characterName;

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

        Debug.Log("CURRENT HP: " + current.hp);
    }

    public virtual void GetHit(HitParam hit) {
        float dameTake = CalculateDame(hit);
        GameObject flyingtext = GameSystem.LoadPool("textdame", textName.transform.position);
        flyingtext.GetComponent<TextMeshPro>().text = Convert.ToInt32(dameTake).ToString();

        Debug.Log("CURRENT: " + current);
        Debug.Log("CURRENT HP: " + current.hp);
        Debug.Log(dameTake);

        current.hp -= dameTake;

        if (current.hp <= 0) {
            Died(hit);
            return;
        } else {
            StartCoroutine(PauseMovement(0.1f));
        }
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

        Invoke("Disappear", 2f);
    }

    public void Disappear() {
        gameObject.SetActive(false);
    }
}

