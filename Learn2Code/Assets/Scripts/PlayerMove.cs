using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Mover
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
         
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }


    protected override void Death()
    {
        isAlive = false;
        base.Death();
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
        
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if(isAlive)
             UpdateMotor(new Vector3(x, y, 0));

        GetAnimatorMovement();
    }  

    public void OnLevelUp()
    {
        maxHitpoint++;
        hitPoint = maxHitpoint;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(int healingAmount)
    {
        if (hitPoint == maxHitpoint)
            return;
        
        hitPoint += healingAmount;
        if (hitPoint > maxHitpoint)
            hitPoint = maxHitpoint;
        GameManager.instance.ShowText("+ " + healingAmount.ToString() + "HP", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitpointChange();
    }

    public void Respawn()
    {
        Heal(maxHitpoint);
        isAlive = true;

        lastImmune = Time.time;

        pushDirection = Vector3.zero;
    }
}
