using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue_Cultist : MonoBehaviour {
    
    public float health = 1f;
    public float maxHealth = 15f;

    private float dmgCool = 0.0f;
    private float maxDmgCool = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;

    void Start() {
        health = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Sprites/BlueCultist");

        StartCoroutine("Attack");
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "PlayerAttack" && dmgCool <= 0.0f) {
            health -= (other.GetComponent<Damage_Storage>().damage * other.GetComponent<Damage_Storage>().damageMultiplier);
            dmgCool = maxDmgCool;

            if(health <= 0) {
                Destroy(gameObject);
            }
        }
    }


    void Update() {
        if(dmgCool > 0.0f) {
            dmgCool -= Time.deltaTime;

            spriteRenderer.color = new Color(1, (maxDmgCool - dmgCool) * 5, (maxDmgCool - dmgCool) * 5, 1);
        }
    }


    IEnumerator Attack() {
        float cooldown = Random.Range(0, 3);
        float maxCooldown = 3.0f;

        while(true) {
            cooldown -= Time.deltaTime;

            if(cooldown <= 0.0f) {
                cooldown = maxCooldown;

                float timer = 0.0f;
                bool spawnedPortals = false;

                while(true) {
                    timer += Time.deltaTime;

                    if(timer < 0.2f) {
                        spriteRenderer.sprite = sprites[2];
                    }
                    if(timer >= 0.2f && timer < 0.4f) {
                        spriteRenderer.sprite = sprites[3];

                        if(!spawnedPortals) {
                            spawnedPortals = true;
                            GameObject portal = Resources.Load<GameObject>("Prefabs/BlueCultistAttack");
                            Transform playerTransform = GameObject.Find("Player").transform;

                            Vector2 randomPos = Vector3.Normalize(Random.insideUnitCircle) * Random.Range(1.0f, 1.5f);
                            Vector3 randomPosVect3 = new Vector3(randomPos.x, randomPos.y, 0);

                            Instantiate(portal, playerTransform.position, Quaternion.identity);
                            Instantiate(portal, playerTransform.position + randomPosVect3, Quaternion.identity);

                            GetComponent<AudioSource>().Play();
                        }
                    }
                    if(timer >= 0.6f && timer < 0.8f) {
                        spriteRenderer.sprite = sprites[3];
                    }
                    if(timer >= 0.8f && timer < 1.0f) {
                        spriteRenderer.sprite = sprites[4];
                    }
                    if(timer >= 1.0f && timer < 1.2f) {
                        spriteRenderer.sprite = sprites[5];
                    }
                    if(timer >= 1.2f && timer < 1.4f) {
                        spriteRenderer.sprite = sprites[6];
                    }
                    if(timer >= 1.9f && timer < 2.1f) {
                        spriteRenderer.sprite = sprites[5];
                    }
                    if(timer >= 2.1f && timer < 2.3f) {
                        spriteRenderer.sprite = sprites[4];
                    }
                    if(timer >= 2.3f && timer < 2.5f) {
                        spriteRenderer.sprite = sprites[3];
                    }
                    if(timer >= 2.5f && timer < 2.7f) {
                        spriteRenderer.sprite = sprites[2];
                    }
                    if(timer >= 2.7f) {
                        spriteRenderer.sprite = sprites[0];
                        break;
                    }

                    yield return null;
                }
            }

            yield return null;
        }
    }

}
