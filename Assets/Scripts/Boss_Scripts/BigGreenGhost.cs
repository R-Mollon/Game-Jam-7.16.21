using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGreenGhost : MonoBehaviour {
    
    private SpriteRenderer spriteRenderer;
    private Sprite[] spriteSheet;

    private bool spawned = false;

    public int health;
    private int maxHealth = 100;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteSheet = Resources.LoadAll<Sprite>("Sprites/Bosses/BigGreenGhost");

        health = maxHealth;

        StartCoroutine("Spawn");
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "Player") {
            // Knock player back
            Vector3 force = transform.position - other.transform.position;
            force.Normalize();

            other.GetComponent<Rigidbody2D>().AddForce(-force * 10000);
            other.transform.Find("PlayerDamageCollider").GetComponent<PlayerDamage>().onDamage(1);
        }
    }


    IEnumerator Spawn() {

        int animStage = 1;
        float timer = 0.0f;
        float maxTimer = 0.75f;

        while(!spawned) {
            timer += Time.deltaTime;

            if(timer >= maxTimer) {
                timer = 0f;
                animStage++;
            }

            if(animStage > 9) {
                animStage = 0;
                spawned = true;
            }

            spriteRenderer.sprite = spriteSheet[animStage];

            yield return null;
        }

        yield return null;
    }


}
