using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Damage : MonoBehaviour {

    private float maxHealth = 20.0f;
    public float health = 1.0f;

    private float damageTime = 0.0f;
    private float maxDmgTime = 0.5f;
    
    private SpriteRenderer spriteRenderer;
    
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
        Destroy(gameObject);
    }


    void Start() {
        health = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update() {

        if(damageTime > 0.0f) {
            damageTime -= Time.deltaTime;

            spriteRenderer.color = new Color(1, (255 - (damageTime * 400)) / 255f, (255 - (damageTime * 400)) / 255f, 1f);
        }

        if(damageTime <= 0.0f) {
            damageTime = 0.0f;
        }

    }

}
