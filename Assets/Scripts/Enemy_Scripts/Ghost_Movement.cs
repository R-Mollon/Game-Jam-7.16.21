using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Movement : MonoBehaviour {
    
    public int damage;

    public bool teleport = false;
    private bool attacking = false;

    public bool doesFire;
    public float maxAttkCooldown;

    public float speed;

    public string spritesName;

    private float teleportRadius = 8.0f;
    private float minTeleRadius = 5.0f;

    private float damageCooldown = 0.0f;
    private float maxDmgCooldown = 0.2f;

    private float health = 1.0f;
    private float maxHealth = 15.0f;
    
    private GameObject player;

    private SpriteRenderer spriteRenderer;

    void Start() {
        player = GameObject.Find("Player");

        spriteRenderer = GetComponent<SpriteRenderer>();

        health = maxHealth;

        StartCoroutine("HoverAnimation");

        if(doesFire) {
            StartCoroutine("Attack");
        }
    }


    void Update() {
        if(!attacking) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        if(teleport) {
            // Pick a position around player
            Vector3 randomPosition;
            do {
                randomPosition = Random.insideUnitCircle * teleportRadius;
            } while(Vector3.Distance(player.transform.position + randomPosition, player.transform.position) < minTeleRadius);

            // Teleport to position
            transform.position = player.transform.position + randomPosition;

            teleport = false;
        }

        if(damageCooldown > 0.0f) {
            damageCooldown -= Time.deltaTime;

            spriteRenderer.color = new Color(1, (255 - (damageCooldown * 1000)) / 255f, (255 - (damageCooldown * 1000)) / 255f, 35f / 255f);

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
        Sprite[] animSprites = Resources.LoadAll<Sprite>("Sprites/" + spritesName);

        while(true) {
            timer += Time.deltaTime;

            if(timer >= maxTimer) {
                timer = 0.0f;

                animStage++;

                if(animStage > 3) {
                    animStage = 0;
                }
            }

            if(!attacking) {
                spriteRenderer.sprite = animSprites[animStage];
            }

            yield return null;
        }
    }


    IEnumerator Attack() {
        float attackTimer = maxAttkCooldown;

        int animStage = 0;
        float animTimer = 0.0f;
        float maxAnimTimer = 0.25f;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite[] animSprites = Resources.LoadAll<Sprite>("Sprites/" + spritesName);

        while(true) {
            attackTimer += Time.deltaTime;

            if(attackTimer >= maxAttkCooldown) {
                animTimer += Time.deltaTime;
                attacking = true;

                if(animTimer >= maxAnimTimer) {
                    animStage++;
                    animTimer = 0.0f;

                    if(animStage > 3) {
                        attacking = false;
                        animStage = 0;
                        attackTimer = 0.0f;

                        Instantiate(Resources.Load<GameObject>("Prefabs/GreenGhostProjectile"), transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity, GameObject.Find("Projectiles").transform);
                    }
                }

                spriteRenderer.sprite = animSprites[4 + animStage];
            }

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
