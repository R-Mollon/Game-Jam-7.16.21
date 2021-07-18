using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist_Projectile : MonoBehaviour {
    
    public int damage = 1;

    public bool doneSpawning = false;

    public int animStage = 0;
    public int maxAnimStage;
    private float stageCounter = 0.0f;
    private float maxStageTime = 0.05f;

    private SpriteRenderer sprite;
    private Sprite[] spriteList;
    private CircleCollider2D circleCollider;

    void Start() {
        sprite = GetComponent<SpriteRenderer>();
        spriteList = Resources.LoadAll<Sprite>("Sprites/Projectile");
        circleCollider = GetComponent<CircleCollider2D>();
        maxAnimStage = spriteList.Length;
    }

    void Update() {

        sprite.sprite = spriteList[animStage];

        if(animStage < maxAnimStage - 1) {

            stageCounter += Time.deltaTime;

            if(stageCounter >= maxStageTime) {
                stageCounter = 0f;
                animStage++;
            }
        } else {

            doneSpawning = true;

        }

    }

    void OnTriggerEnter2D(Collider2D other) {

        if(other.tag == "Wall") {
            Destroy(gameObject);
        }

    }

}
