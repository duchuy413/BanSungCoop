using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class BattleBehavior : MonoBehaviour
{
    public CharacterStat stat;
    public int level;
    public GameObject attackObj;
    //public DHealthBar healthBar;
    public TextMeshPro textName;

    public bool isAnimating = false;

    [HideInInspector]
    public HitParam hitParam;

    [HideInInspector]
    public BattleStat current;

    [HideInInspector]
    public bool died;

    public virtual void OnEnable() {
        died = false;
        current = new BattleStat();
        LoadLevel(level);
        //gameObject.layer = GameConstants.LAYER_GROUND;

        DMovementExecutor executor = GetComponent<DMovementExecutor>();
        if (executor != null) {
            executor.enabled = true;
            GetComponent<DMovement>().enabled = true;
            GetComponent<FramesAnimator>().enabled = true;
        }
    }

    private void LoadLevel(int level)
    {
        //healthBar.SetValue(1);
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
    }

    public virtual void GetHit(HitParam hit)
    {
        Debug.Log("this is direction get from get hit: " + hit.direction);

        float dameTake = CalculateDame(hit);
        GameObject flyingtext = GameSystem.LoadPool("TextDame", textName.transform.position);
        flyingtext.GetComponent<TextMeshPro>().text = Convert.ToInt32(dameTake).ToString();

        current.hp -= dameTake;
        //healthBar.SetValue(current.hp, current.maxhp);

        if (current.hp <= 0) {
            Died(hit);
            Debug.Log("Player died");
            return;
        }
        else {
            StartCoroutine(PauseMovement(GetComponent<DMovementExecutor>(), GetComponent<DMovement>()));
        }

        //GetComponent<DAnimatorPrior>().StartPriorAnimation(stat.gethit);

        Debug.Log(gameObject.name + " get hit from " + hit.owner.name);
    }

    IEnumerator PauseMovement(DMovementExecutor executor, DMovement movement) {
        executor.enabled = false;
        movement.enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        yield return new WaitForSeconds(0.1f);

        if (!died) {
            executor.enabled = true;
            movement.enabled = true;
        }
    }

    public float CalculateDame(HitParam hit)
    {
        return hit.dame;
    }

    public void ApplyDame(GameObject target)
    {
        HitParam hit = new HitParam();
        hit.dame = current.dame;
        hit.owner = gameObject;
        hit.ownerTag = gameObject.tag;
        
        target.SendMessage("GetHit", hit,SendMessageOptions.DontRequireReceiver);
    }

    public void Died(HitParam hitParam) {

        DMovementExecutor executor = GetComponent<DMovementExecutor>();
            died = true;
            executor.enabled = false;
            GetComponent<DMovement>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            //GetComponent<DAnimatorPrior>().StartPriorAnimation(stat.die, false);
            GetComponent<DVelocityCustom>().Throw(hitParam.direction, 2f, 2f);

            Invoke("Disappear", 2f);
    }

    public void Disappear() 
    {
        gameObject.SetActive(false);
    }
}

