using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green_Cultist : MonoBehaviour {

    public float health = 1f;
    public float maxHealth = 15f;

    private float dmgCool = 0.0f;
    private float maxDmgCool = 0.2f;
    
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        health = maxHealth;

        StartCoroutine("Attack");
    }


    void Update() {
        if(dmgCool > 0.0f) {
            dmgCool -= Time.deltaTime;

            spriteRenderer.color = new Color(1, (maxDmgCool - dmgCool) * 5, (maxDmgCool - dmgCool) * 5, 1);
        }
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


    IEnumerator Attack() {

        float cooldown = Random.Range(0, 4);
        float maxCooldown = 4.0f;

        while(true) {
            cooldown -= Time.deltaTime;

            if(cooldown <= 0.0f) {

                float timer = 0.0f;
                cooldown = maxCooldown;

                while(true) {
                    lineRenderer.SetPosition(0, transform.position - (Vector3.up * 0.2f));
                    lineRenderer.SetPosition(1, GameObject.Find("Player").transform.position);

                    timer += Time.deltaTime;

                    if(timer >= 2.0f) {
                        GameObject projectile = Instantiate(Resources.Load<GameObject>("Prefabs/GreenCultistProjectile"), transform.position, Quaternion.identity);
                        GreenCultistProjectile script = projectile.GetComponent<GreenCultistProjectile>();

                        lineRenderer.SetPosition(0, transform.position);
                        lineRenderer.SetPosition(1, transform.position);

                        script.angle = Vector3.Normalize(GameObject.Find("Player").transform.position - transform.position);
                        script.go();

                        break;
                    }

                    yield return null;
                }

            }

            yield return null;
        }

    }


}
