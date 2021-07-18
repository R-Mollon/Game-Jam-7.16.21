﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour {
    
    public float attackCooldown = 2.0f;

    private GameObject circleAttackPrefab;
    private SpriteRenderer enemySprite;
    private Sprite[] spriteList;

    private bool attacking = false;
    private int attackStage = 0;
    private float attackTime = 0.0f;
    private float maxAttTime = 0.5f;

    void Start() {

        circleAttackPrefab = Resources.Load<GameObject>("Prefabs/CircleAttack");
        enemySprite = GetComponent<SpriteRenderer>();
        spriteList = Resources.LoadAll<Sprite>("Sprites/Cultist");

        StartCoroutine("Attack");
    }


    IEnumerator Attack() {

        float cooldown = 0f;

        while(true) {

            if(cooldown > 0.0f) {
                cooldown -= Time.deltaTime;
            } else {
                cooldown = attackCooldown;
                attacking = true;
                doCircleAttack();
            }

            yield return null;
        }

    }


    void Update() {
        if(attacking) {
            
            if(attackStage > 4) {
                attackStage = 0;
                attacking = false;
                enemySprite.sprite = spriteList[0];
                return;
            }

            enemySprite.sprite = spriteList[2 + attackStage];

            attackTime += Time.deltaTime;

            if(attackTime >= maxAttTime) {
                attackTime = 0;
                attackStage++;
            }
        }
    }


    void doCircleAttack() {

        Transform attackParent = GameObject.Find("Projectiles").transform;
        
        GameObject attack = Instantiate(circleAttackPrefab, transform.position - (transform.up * 0.75f), Quaternion.Euler(0, 0, 0), attackParent);

        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = GameObject.Find("Player").transform.position;

        float xDelta = enemyPosition.x - playerPosition.x;
        float yDelta = playerPosition.y - enemyPosition.y;

        float angleRadians = Mathf.Atan2(xDelta, yDelta);
        float attackAngle = (360 / (Mathf.PI * 2)) * angleRadians;

        Circle_Attack attackScript = attack.GetComponent<Circle_Attack>();

        attackScript.angle = attackAngle;
        attackScript.go();

    }

}
