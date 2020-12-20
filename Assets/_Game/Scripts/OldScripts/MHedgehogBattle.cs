using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class MHedgehogBattle : DBattle
{
    bool hideInShield = false;

    public override void OnEnable() {
        base.OnEnable();
        hideInShield = false;
    }

    public override void GetHit(DHitParam hit) {

        float dameTake = CalculateDame(hit);
        GameObject flyingtext = GameSystem.LoadPool("TextDame", textName.transform.position);
        flyingtext.GetComponent<TextMeshPro>().text = Convert.ToInt32(dameTake).ToString();

        current.hp -= dameTake;
        //healthBar.SetValue(current.hp, current.maxhp);

        if (current.hp <= 0) {
            Died(hit);
            return;
        }
        else {
            if (hideInShield == false) {

                Debug.Log("THIS IS BLABSLASKASJKA");
                hideInShield = true;
                //GetComponent<DAnimatorPrior>().StartPriorAnimation(stat.custom1,false);
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                GetComponent<DMovementExecutor>().enabled = false;
                GetComponent<DMovement>().enabled = false;

                StartCoroutine(IncreaseStat());
            }
        }
    }

    public IEnumerator IncreaseStat() {
        yield return new WaitForSeconds(1f);
        if (!died) {
            current.hp = current.hp * 10;
            current.maxhp = current.maxhp * 10;
            //healthBar.SetValue(current.hp, current.maxhp);
        }
    }

    private void Update() {
        //if (GameSystem.GetPlayerDistance(transform) > 5f && hideInShield == true) {
        //    OnEnable();
        //    hideInShield = false;
        //}
    }

}
