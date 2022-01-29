using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Collidable
{

    

    private float coolDown = 5.0f;

    private float lastShout;

    public Dialogue dialogue;

    public Animator animator;

    protected override void OnCollide(Collider2D coll)
    {

        animator.SetTrigger("show");

        if (Time.time - lastShout > coolDown)
        {
            lastShout = Time.time;
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            

        }
        else
            animator.SetTrigger("hide");
    }
}
