using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            GameManager.instance.SaveState();
            SceneManager.LoadScene(2);
        }
    }
}
