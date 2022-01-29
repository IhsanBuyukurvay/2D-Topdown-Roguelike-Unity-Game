using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    public int xpValue = 1;

    public float triggerLength = 1;
    public float chaseLength = 5;

    private bool chasing;
    private bool collidingWithPlayer;
    
    private Transform playerTransform;
    private Vector3 startingPosition;

    //Hitbox Düþmanlara silah verme hitboxlarý ile dmg atsýnlar
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        playerTransform = GameObject.Find("Player").transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        // Playerý takip etmek için range girdi mi kontrol et
        if  (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            

            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                chasing = true;
            

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

        //Playerla colliderlar overlaplýyor mu checkle
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if(hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }

            hits[i] = null;

        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + "XP", 30, Color.blue, transform.position, Vector3.up * 40, 1.0f);
    }
}
