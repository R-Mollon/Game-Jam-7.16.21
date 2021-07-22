using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour {
    
    public int playerHealth = 1;
    public int maxHealth = 1;

    private int previousHealth = 1;
    private int previousMaxHealth = 6;

    private float damageTime = 0.0f;
    private float maxDmgTime = 0.3f;

    private Sprite[] heartSprites;
    private Image[] heartSpritesImages;
    private Image screenPanel;

    private AudioSource damageSound;

    void Start() {

        heartSprites = Resources.LoadAll<Sprite>("Sprites/Hearts");

        string path = "HUD/Panel/Hearts/Heart";
        heartSpritesImages = new[] {
            GameObject.Find(path + "1").GetComponent<Image>(),
            GameObject.Find(path + "2").GetComponent<Image>(),
            GameObject.Find(path + "3").GetComponent<Image>(),
            GameObject.Find(path + "4").GetComponent<Image>(),
            GameObject.Find(path + "5").GetComponent<Image>(),
            GameObject.Find(path + "6").GetComponent<Image>(),
            GameObject.Find(path + "7").GetComponent<Image>(),
            GameObject.Find(path + "8").GetComponent<Image>(),
            GameObject.Find(path + "9").GetComponent<Image>(),
        };

        screenPanel = GameObject.Find("HUD/Panel").GetComponent<Image>();

        damageSound = GetComponent<AudioSource>();

        // Initialize hearts
        maxHealth = previousMaxHealth;
        playerHealth = maxHealth;
        previousHealth = playerHealth;

        for(int i = 0; i < ((int) Mathf.Floor(maxHealth / 2)); i++) {
            heartSpritesImages[i].enabled = true;
        }

    }


    void Update() {

        // Update heart sprites
        if(playerHealth != previousHealth) {
            for(int i = 0; i < ((int) Mathf.Floor(maxHealth / 2)); i++) {
                
                if(playerHealth > (i * 2) + 1) {
                    heartSpritesImages[i].sprite = heartSprites[0];
                } else if(playerHealth == (i * 2) + 1) {
                    heartSpritesImages[i].sprite = heartSprites[1];
                } else {
                    heartSpritesImages[i].sprite = heartSprites[2];
                }
                
            }

            previousHealth = playerHealth;
        }

        // Update amount of hearts on screen
        if(maxHealth > previousMaxHealth) {
            for(int i = 0; i < ((int) Mathf.Floor(maxHealth / 2)); i++) {
                heartSpritesImages[i].enabled = true;
            }
            previousMaxHealth = maxHealth;
        }

        if(maxHealth < previousMaxHealth) {
            for(int i = 0; i < heartSpritesImages.Length; i++) {
                if(i > (maxHealth / 2)) {
                    heartSpritesImages[i].enabled = false;
                }
            }
            previousMaxHealth = maxHealth;
        }

        // Tick down invincibility time
        if(damageTime > 0.0f) {

            screenPanel.color = new Color32(190, 0, 0, (byte) Mathf.Floor(damageTime * 255));

            damageTime -= Time.deltaTime;

            if(damageTime <= 0.0f) {
                screenPanel.color = new Color32(190, 0, 0, 0);
                damageTime = 0.0f;
            }
        }

    }


    void OnTriggerEnter2D(Collider2D collider) {

        if(damageTime <= 0.01f) {
            if(collider.tag == "EnemyAttack") {
                Damage_Storage damageScript = collider.GetComponent<Damage_Storage>();

                onDamage((int) Mathf.Floor(damageScript.damage));
                Destroy(collider.gameObject);
            }

            if(collider.tag == "Ghost") {
                Ghost_Movement script = collider.GetComponent<Ghost_Movement>();

                onDamage(script.damage);
                script.teleport = true;
            }
        }

        if(collider.tag == "ItemPickup") {
            ItemPickup item = collider.GetComponent<ItemPickup>();

            item.pickup();
        }

    }

    public void onDamage(int damage) {
        playerHealth -= damage;
        damageTime = maxDmgTime;
        damageSound.Play();
    }


}
