using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPosition : NetworkBehaviour
{
    [SyncVar]
    public Vector3 desiredPos;

    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            CmdUpdatePos(transform.position);
        }
        else
        {
            Vector3 dif = desiredPos - transform.position;
            if (dif.magnitude < 0.1f)
                dif = new Vector3(0, 0);

            rb2d.velocity = dif.normalized * rb2d.velocity.magnitude;
        }
    }

    [Command]
    public void CmdUpdatePos(Vector3 pos)
    {
        desiredPos = pos;
    }
}
