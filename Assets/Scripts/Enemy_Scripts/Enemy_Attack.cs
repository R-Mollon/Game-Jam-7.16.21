using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour {
    
    public float attackCooldown = 2.0f;

    private GameObject circleAttackPrefab;

    void Start() {

        circleAttackPrefab = Resources.Load<GameObject>("Prefabs/CircleAttack");

        StartCoroutine("Attack");
    }


    IEnumerator Attack() {

        float cooldown = 0f;

        while(true) {

            if(cooldown > 0.0f) {
                cooldown -= Time.deltaTime;
            } else {
                cooldown = attackCooldown;
                doCircleAttack();
            }

            yield return null;
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
