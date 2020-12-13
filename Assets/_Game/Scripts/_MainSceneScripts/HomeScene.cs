using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : MonoBehaviour
{
    public void StartHost() {
        NetworkSystem.Instance.StartHost();
    }
}
