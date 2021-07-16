using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Damage : MonoBehaviour {

    private float maxHealth = 10.0f;
    public float health = 1.0f;

    private float damageTime = 0.0f;
    private float maxDmgTime = 0.75f;
    
    
    void OnTriggerEnter2D(Collider2D collider) {

        if(collider.tag == "PlayerAttack") {
            if(damageTime <= 0.0f) {
                health -= collider.gameObject.GetComponent<Damage_Storage>().damage;

                damageTime = maxDmgTime;

                if(health <= 0.0f) {
                    onDeath();
                }
            }
        }

    }


    void onDeath() {
        this.gameObject.SetActive(false);
    }


    void Start() {
        health = maxHealth;
    }


    void Update() {

        if(damageTime > 0.0f) {
            damageTime -= Time.deltaTime;
        }

        if(damageTime <= 0.0f) {
            damageTime = 0.0f;
        }

    }

}
