using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{

    public string message;

    private float coolDown = 5.0f;

    private float lastShout;

    protected override void OnCollide(Collider2D coll)
    {        
        if(Time.time - lastShout > coolDown)
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(message, 35, Color.white, transform.position, Vector3.zero, coolDown);
        }
    }
}
