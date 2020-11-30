using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public enum END_OF_TIME_ACTION { 
        SetActiveFalse,
        Destroy,
        Nothing
    }

    public END_OF_TIME_ACTION endOfTimeAction = END_OF_TIME_ACTION.SetActiveFalse;
    public float sec = 1f;

    public void OnEnable() {
        StartCoroutine(TimeOut());
    }

    public IEnumerator TimeOut() {
        yield return new WaitForSeconds(sec);

        gameObject.SetActive(false);

        //if (endOfTimeAction == END_OF_TIME_ACTION.SetActiveFalse) {
        //    //if (gameObject.activeSelf == true) {
        //    //    gameObject.SetActive(false);
        //    //}
        //    gameObject.SetActive(false);
        //} else if (endOfTimeAction == END_OF_TIME_ACTION.Destroy) {
        //    if (gameObject != null) {
        //        Destroy(gameObject);
        //    }
        //}
    }
}
