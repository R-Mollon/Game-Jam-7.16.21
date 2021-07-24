using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistBoss : MonoBehaviour {
    
    public float health = 1f;
    public float maxHealth = 150f;

    private float dmgCool = 0.0f;
    private float maxDmgCool = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Sprite[] redSprites;
    private Sprite[] blueSprites;
    private Sprite[] greenSprites;
    private LineRenderer lineRenderer;

    private bool attacking = false;
    private bool invulnerable = true;

    private int randomSprite = 0;

    private GameObject bluePortal;
    private Transform playerTransform;

    private GameObject greenProjectile;

    void Start() {
        health = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        redSprites = Resources.LoadAll<Sprite>("Sprites/Cultist");
        blueSprites = Resources.LoadAll<Sprite>("Sprites/BlueCultist");
        greenSprites = Resources.LoadAll<Sprite>("Sprites/GreenCultist");

        lineRenderer = GetComponent<LineRenderer>();

        bluePortal = Resources.Load<GameObject>("Prefabs/BlueCultistAttack");
        playerTransform = GameObject.Find("Player").transform;

        greenProjectile = Resources.Load<GameObject>("Prefabs/GreenCultistProjectile");

        StartCoroutine("Spawn");
    }

    
    IEnumerator Spawn() {
        float timer = 0.0f;

        while(timer <= 6.5f) {
            timer += Time.deltaTime;

            spriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp(timer / 6.5f, 0, 1));
            yield return null;
        }

        invulnerable = false;

        StartCoroutine("Attack");

        yield return null;
    }


    IEnumerator Death() {
        float timer = 6.5f;

        invulnerable = true;

        while(timer > 0.0f) {
            timer -= Time.deltaTime;

            spriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp(timer / 6.5f, 0, 1));
            yield return null;
        }

        GameObject.Find("BossMusic").GetComponent<AudioSource>().Stop();

        Transform itemSpawnParent = GameObject.Find("ItemSpawnManager").transform;

        Instantiate(Resources.Load<GameObject>("Prefabs/Items/ChaliceItem"), transform.position - transform.up, Quaternion.identity, itemSpawnParent);

        // Spawn next floor portal
        Transform playerTransform = GameObject.Find("Player").transform;
        if(playerTransform.position.y > transform.position.y) {
            Instantiate(Resources.Load<GameObject>("Prefabs/NextFloorPortal"), transform.position - (transform.up * 4), Quaternion.identity);
        } else {
            Instantiate(Resources.Load<GameObject>("Prefabs/NextFloorPortal"), transform.position + (transform.up * 3), Quaternion.identity);
        }

        Destroy(gameObject);

        yield return null;
    }


    void Update() {
        if(dmgCool > 0.0f) {
            dmgCool -= Time.deltaTime;

            spriteRenderer.color = new Color(1, (maxDmgCool - dmgCool) * 5, (maxDmgCool - dmgCool) * 5, 1);
        }
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "PlayerAttack" && dmgCool <= 0.0f && !invulnerable) {
            health -= (other.GetComponent<Damage_Storage>().damage * other.GetComponent<Damage_Storage>().damageMultiplier);
            dmgCool = maxDmgCool;

            if(health <= 0.0f) {
                StopAllCoroutines();
                StartCoroutine("Death");
            }
        }
    }


    IEnumerator Attack() {
        float atkTimer = 1.0f;
        float maxAtkTimer = 2.0f;

        while(true) {
            atkTimer -= Time.deltaTime;

            if(atkTimer <= 0.0f) {
                attacking = true;

                atkTimer = maxAtkTimer;

                StartCoroutine("HarlequinAttack");

                while(attacking) {
                    yield return null;
                }
            }

            yield return null;
        }

    }


    IEnumerator HarlequinAttack() {

        float timer = 0.0f;

        int timesChosen = 0;
        int randomSprite = 0;

        switch(randomSprite) {
            case 0: spriteRenderer.sprite = redSprites[1]; break;
            case 1: spriteRenderer.sprite = blueSprites[1]; break;
            case 2: spriteRenderer.sprite = greenSprites[1]; break;
        }

        // Choose a random sprite a few times
        while(timesChosen < 25) {
            timer += Time.deltaTime;
            float delay = Mathf.Abs(26 - timesChosen) * 0.01f;

            if(timer >= delay) {
                timer = 0.0f;

                int lastSprite = randomSprite;
                do {
                    randomSprite = Random.Range(0, 3);
                } while(lastSprite == randomSprite);

                switch(randomSprite) {
                    case 0: spriteRenderer.sprite = redSprites[1]; break;
                    case 1: spriteRenderer.sprite = blueSprites[1]; break;
                    case 2: spriteRenderer.sprite = greenSprites[1]; break;
                }

                timesChosen++;
            }

            yield return null;
        }

        switch(randomSprite) {
            case 0: spriteRenderer.sprite = redSprites[0]; break;
            case 1: spriteRenderer.sprite = blueSprites[0]; break;
            case 2: spriteRenderer.sprite = greenSprites[0]; break;
        }

        yield return new WaitForSeconds(1.0f);

        if(randomSprite == 0) {
            // Red cultist attack

            spriteRenderer.sprite = redSprites[4];

            Vector3 angle = Vector3.Normalize(GameObject.Find("Player").transform.position - transform.position);

            Vector3[] offsets = new[] {
                new Vector3(-2.2f, 1.6f, 0f), new Vector3(2.2f, 1.6f, 0f), new Vector3(3.6f, -2.7f, 0f),
                new Vector3(0f, -5f, 0f), new Vector3(-3.6f, -2.7f, 0f),
            };

            GameObject circleAttack = Resources.Load<GameObject>("Prefabs/CircleAttack");

            for(int i = 0; i < offsets.Length; i++) {
                GameObject attack = Instantiate(circleAttack, transform.position + offsets[i], Quaternion.identity);
                Circle_Attack attackScript =  attack.GetComponent<Circle_Attack>();

                attackScript.angle = angle;
                attackScript.go();
            }

            yield return new WaitForSeconds(1.0f);

            spriteRenderer.sprite = redSprites[0];
        }

        if(randomSprite == 1) {
            // Blue cultist attack

            float blueTimer = 0.0f;
            float blueAttackTimer = 0.0f;

            int blueAmimFrame = 3;
            int blueAttackCount = 0;

            spriteRenderer.sprite = blueSprites[2];
            yield return new WaitForSeconds(0.2f);

            spriteRenderer.sprite = blueSprites[3];
            
            while(blueAttackCount < 20) {
                blueAttackTimer += Time.deltaTime;

                if(blueAttackTimer > 0.5f) {
                    Instantiate(bluePortal, playerTransform.position, Quaternion.identity);

                    for(int i = 0; i < 5; i++) {
                        Vector2 randomPos = Vector3.Normalize(Random.insideUnitCircle) * Random.Range(1.0f, 5.0f);
                        Vector3 randomPosVect3 = new Vector3(randomPos.x, randomPos.y, 0);

                        Instantiate(bluePortal, playerTransform.position + randomPosVect3, Quaternion.identity);
                    }

                    blueAttackCount++;
                    blueAttackTimer = 0.0f;
                }

                if(blueAmimFrame < 6) {
                    blueTimer += Time.deltaTime;

                    if(blueTimer > 0.2f) {
                        blueTimer = 0.0f;
                        blueAmimFrame++;

                        spriteRenderer.sprite = blueSprites[blueAmimFrame];
                    }
                }

                yield return null;
            }

            yield return new WaitForSeconds(0.1f);

            while(blueAmimFrame > 2) {
                spriteRenderer.sprite = blueSprites[blueAmimFrame--];
                yield return new WaitForSeconds(0.1f); 
            }

            spriteRenderer.sprite = blueSprites[0];

        }

        if(randomSprite == 2) {
            // Green cultist attack

            int greenAttackCount = 0;
            float greenTimer = 0.0f;

            while(greenAttackCount < 5) {
                lineRenderer.SetPosition(0, transform.position - (Vector3.up * 0.2f));
                lineRenderer.SetPosition(1, GameObject.Find("Player").transform.position);

                greenTimer += Time.deltaTime;
                if(greenTimer >= 2.0f) {
                    int greenProjectileCount = 0;
                    greenTimer = 0.0f;

                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, transform.position);

                    Vector3 angle1 = Vector3.Normalize((playerTransform.position) - transform.position);
                    float angle1Radians = Mathf.Atan2(angle1.y, angle1.x);
                    float angle2Radians = angle1Radians + (Mathf.PI / 4);
                    float angle3Radians = angle1Radians - (Mathf.PI / 4);

                    Vector3 angle2 = Vector3.Normalize(new Vector3(angle1.x + Mathf.Cos(angle2Radians), angle1.y + Mathf.Sin(angle2Radians), -1));
                    Vector3 angle3 = Vector3.Normalize(new Vector3(angle1.x + Mathf.Cos(angle3Radians), angle1.y + Mathf.Sin(angle3Radians), -1));

                    while(greenProjectileCount < 5) {

                        
                        GameObject projectile1 = Instantiate(greenProjectile, transform.position, Quaternion.identity);
                        GreenCultistProjectile script1 = projectile1.GetComponent<GreenCultistProjectile>();
                        script1.angle = angle1;
                        
                        GameObject projectile2 = Instantiate(greenProjectile, transform.position, Quaternion.identity);
                        GreenCultistProjectile script2 = projectile2.GetComponent<GreenCultistProjectile>();
                        script2.angle = angle2;
                        
                        GameObject projectile3 = Instantiate(greenProjectile, transform.position, Quaternion.identity);
                        GreenCultistProjectile script3 = projectile3.GetComponent<GreenCultistProjectile>();
                        script3.angle = angle3;

                        script1.go();
                        script2.go();
                        script3.go();


                        greenProjectileCount++;
                        yield return new WaitForSeconds(0.05f);
                    }

                    greenAttackCount++;
                    yield return new WaitForSeconds(0.5f);
                }

                yield return null;;
            }
        }

        attacking = false;
        yield return null;
    }

}
