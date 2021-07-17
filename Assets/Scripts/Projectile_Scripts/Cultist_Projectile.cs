using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist_Projectile : MonoBehaviour {
    
    public float angle = 0.0f;
    public float damage = 5.0f;

    private float speed = 20.0f;

    private int animStage = 0;
    private float stageCounter = 0.0f;
    private float maxStageTime = 0.1f;

    private SpriteRenderer sprite;
    private Sprite[] spriteList;
    private CircleCollider2D collider;
    private Rigidbody2D rigidBody;
    public Vector2 forwardVect;

    void Start() {
        sprite = GetComponent<SpriteRenderer>();
        spriteList = Resources.LoadAll<Sprite>("Sprites/Projectile");
        collider = GetComponent<CircleCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();

        transform.Rotate(0, 0, angle);
        forwardVect = new Vector2(transform.up.x, transform.up.y);
        transform.Rotate(0, 0, -angle);
    }

    void Update() {

        sprite.sprite = spriteList[animStage];

        if(animStage < spriteList.Length - 1) {

            stageCounter += Time.deltaTime;

            if(stageCounter >= maxStageTime) {
                stageCounter = 0f;
                animStage++;
            }
        } else {

            rigidBody.MovePosition(rigidBody.position + (forwardVect * speed * Time.deltaTime));

        }

    }

}
