using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Movement : MonoBehaviour {
    
    public int damage;

    public bool teleport = false;

    private float speed = 0.75f;
    private float teleportRadius = 8.0f;
    private float minTeleRadius = 5.0f;

    private float damageCooldown = 0.0f;
    private float maxDmgCooldown = 0.75f;

    private float health = 1.0f;
    private float maxHealth = 15.0f;
    
    private GameObject player;

    void Start() {
        player = GameObject.Find("Player");

        health = maxHealth;

        StartCoroutine("HoverAnimation");
    }


    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if(teleport) {
            // Pick a position around player
            Vector3 randomPosition;
            do {
                randomPosition = Random.insideUnitCircle * teleportRadius;
            } while(Vector3.Distance(randomPosition, player.transform.position) < minTeleRadius);

            // Teleport to position
            transform.position = player.transform.position + randomPosition;

            teleport = false;
        }

        if(damageCooldown > 0.0f) {
            damageCooldown -= Time.deltaTime;

            if(damageCooldown < 0.0f) {
                damageCooldown = 0.0f;
            }
        }
    }


    IEnumerator HoverAnimation() {

        int animStage = 0;
        float timer = 0.0f;
        float maxTimer = 0.5f;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite[] animSprites = Resources.LoadAll<Sprite>("Sprites/Ghost");

        while(true) {
            timer += Time.deltaTime;

            if(timer >= maxTimer) {
                timer = 0.0f;

                animStage++;

                if(animStage > 3) {
                    animStage = 0;
                }
            }

            spriteRenderer.sprite = animSprites[animStage];

            yield return null;
        }
    }


    void OnTriggerEnter2D(Collider2D other) {

        if(other.tag == "PlayerAttack" && damageCooldown <= 0.0f) {

            teleport = true;
            damageCooldown = maxDmgCooldown;
            health -= other.GetComponent<Damage_Storage>().damage;

            if(health <= 0) {
                Destroy(gameObject);
            }

        }

    }

}
