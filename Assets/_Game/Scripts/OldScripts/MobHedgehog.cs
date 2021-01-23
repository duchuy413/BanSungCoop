using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

//public class MobHedgehog : BattleBehavior
//{
//    bool hideInShield = false;

//    public override void OnEnable() {
//        base.OnEnable();
//        hideInShield = false;
//    }

//    public override void GetHit(HitParam hit) {

//        float dameTake = CalculateDame(hit);
//        GameObject flyingtext = GameSystem.LoadPool("TextDame", textName.transform.position);
//        flyingtext.GetComponent<TextMeshPro>().text = Convert.ToInt32(dameTake).ToString();

//        current.hp -= dameTake;

//        if (current.hp <= 0) {
//            Died(hit);
//            return;
//        }
//        else {
//            if (hideInShield == false) {
//                hideInShield = true;
//                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
//                GetComponent<MovementExecutor>().enabled = false;
//                //GetComponent<DMovement>().enabled = false;

//                StartCoroutine(IncreaseStat());
//            }
//        }
//    }

//    public IEnumerator IncreaseStat() {
//        yield return new WaitForSeconds(1f);
//        if (!died) {
//            current.hp = current.hp * 10;
//            current.maxhp = current.maxhp * 10;
//        }
//    }

//    private void Update() {
//    }

//}
