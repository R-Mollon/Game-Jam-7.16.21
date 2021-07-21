using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGreenGhost : MonoBehaviour {
    
    private SpriteRenderer spriteRenderer;
    private Sprite[] spriteSheet;

    public bool spawned = false;
    public bool dead = false;

    public float health;
    public float maxHealth = 100;

    private float attackCooldown = 20.0f;
    private float attackTimer = 15.0f;

    private float invulnerability = 0.0f;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteSheet = Resources.LoadAll<Sprite>("Sprites/Bosses/BigGreenGhost");

        health = maxHealth;

        StartCoroutine("Spawn");

        StartCoroutine("Attack");
    }


    IEnumerator Attack() {

        while(!spawned) {
            yield return null;
        }

        while(!dead) {
            attackTimer += Time.deltaTime;
            invulnerability -= Time.deltaTime;

            if(attackTimer >= attackCooldown) {
                // Select random attack
                attackTimer = 0.0f;
                int selectedAttack = UnityEngine.Random.Range(0, 2);

                if(selectedAttack == 0) {
                    StartCoroutine("BulletHellAttack");
                } else {
                    StartCoroutine("SpawnAttack");
                }
                
            }

            yield return null;
        }

        yield return null;
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "Player") {
            // Knock player back
            Vector3 force = transform.position - other.transform.position;
            force.Normalize();

            other.GetComponent<Rigidbody2D>().AddForce(-force * 10000);
            other.transform.Find("PlayerDamageCollider").GetComponent<PlayerDamage>().onDamage(1);
        }
        if(other.tag == "PlayerAttack" && invulnerability <= 0.0f) {
            Damage_Storage damage = other.GetComponent<Damage_Storage>();

            health -= damage.damage;
            invulnerability = 0.1f;

        }
    }


    IEnumerator BulletHellAttack() {
        spriteRenderer.sprite = spriteSheet[14];

        float timer = 0.0f;


        Vector3[][] offsets = new[] {
            new[] {
                new Vector3(0, 2f, 0), new Vector3(1.5f, 1.5f, 0), new Vector3(2f, 0, 0), new Vector3(1.5f, -1.5f, 0),
                new Vector3(0, -2f, 0), new Vector3(-1.5f, -1.5f, 0), new Vector3(-2f, 0, 0), new Vector3(-1.5f, 1.5f, 0),
            },
            new[] {
                new Vector3(0.7653669f, 1.847759f, 0), new Vector3(1.959844f, 0.8117942f, 0), new Vector3(1.847759f, -0.7653669f, 0), new Vector3(0.8117942f, -1.959844f, 0),
                new Vector3(-0.7653669f, -1.847759f, 0), new Vector3(-1.959844f, -0.8117942f, 0), new Vector3(-1.847759f, 0.7653669f, 0), new Vector3(-0.8117942f, 1.959844f, 0),
            },
        };

        GameObject projectile = Resources.Load<GameObject>("Prefabs/BigGreenGhostProjectile");
        Transform projectilesParent = GameObject.Find("Projectiles").transform;

        int attacks = 0;
        int attackAngle = 0;
        
        while(true) {
            timer += Time.deltaTime;

            if(timer >= 1.0f && timer < 2.0f) {
                spriteRenderer.sprite = spriteSheet[15];
            }

            if(timer >= 2.0f && timer < 2.5f) {
                spriteRenderer.sprite = spriteSheet[16];
            }

            if(timer >= 2.5f) {
                spriteRenderer.sprite = spriteSheet[17];
            }

            if(timer >= 2.0f && timer < 15.0f) {
                // Do attack

                if(timer >= 2.0f + ((float) attacks / 2)) {
                    attacks++;

                    for(int i = 0; i < offsets[attackAngle].Length; i++) {
                        Instantiate(projectile, transform.position + offsets[attackAngle][i], Quaternion.identity, projectilesParent);
                    }

                    attackAngle++;
                    if(attackAngle > 1) {
                        attackAngle = 0;
                    }
                }
            }

            if(timer >= 15.0f) {
                break;
            }

            yield return null;
        }

        spriteRenderer.sprite = spriteSheet[0];
        
        yield return null;
    }


    IEnumerator SpawnAttack() {
        spriteRenderer.sprite = spriteSheet[14];

        float timer = 0.0f;

        while(true) {
            timer += Time.deltaTime;

            if(timer >= 1.0f && timer < 2.0f) {
                spriteRenderer.sprite = spriteSheet[18];
            }

            if(timer >= 2.0f) {
                spriteRenderer.sprite = spriteSheet[19];

                GameObject[] ghosts = new[] {
                    Resources.Load<GameObject>("Prefabs/Enemies/Ghost"),
                    Resources.Load<GameObject>("Prefabs/Enemies/GreenGhost"),
                };

                // Spawn enemies
                Vector3[] offsets = new[] {
                    new Vector3(0, 4f, 0), new Vector3(3f, 3f, 0), new Vector3(4f, 0, 0), new Vector3(3f, -3f, 0),
                    new Vector3(0, -4f, 0), new Vector3(-3f, -3f, 0), new Vector3(-4f, 0, 0), new Vector3(-3f, 3f, 0),
                };

                for(int i = 0; i < offsets.Length; i++) {
                    GameObject enemy = ghosts[UnityEngine.Random.Range(0, 2)];
                    Instantiate(enemy, transform.position + offsets[i], Quaternion.identity);
                }

                attackTimer = 0.0f;

                break;
            }

            yield return null;
        }

        spriteRenderer.sprite = spriteSheet[0];

        yield return null;
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
