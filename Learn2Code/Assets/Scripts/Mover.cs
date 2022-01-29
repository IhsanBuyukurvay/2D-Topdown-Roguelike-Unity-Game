using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected Animator animator;
    protected RaycastHit2D raycastHit;
    protected float xSpeed = 1.0f;
    protected float ySpeed = 1.0f;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed , 0);

        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        
        moveDelta += pushDirection; //Bununla beraber rakibi ittik
        // sadece bu kod hata verdi sonsuza kadar pushluyor.
        // Bu kod ile pushladýðýmýz zaman veya pushlandýðýmýz zaman her geçen frame pushlamayý azaltacak
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoveryspeed);

        raycastHit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (raycastHit.collider == null)
        {
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        raycastHit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (raycastHit.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }


        
    }

    protected virtual void GetAnimatorMovement()
    {
        

        if (moveDelta.x != 0 || moveDelta.y != 0)
        {
            SetAnimatorMovement(moveDelta);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }

    }

    protected virtual void SetAnimatorMovement(Vector3 moveDelta)
    {
        animator.SetLayerWeight(1, 1);

        animator.SetFloat("xDir", moveDelta.x);
        animator.SetFloat("yDir", moveDelta.y);
    }
}
