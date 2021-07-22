using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestManager : MonoBehaviour {

    private MovementScript playerScript;

    private CanvasGroup unlockedText;
    private CanvasGroup notUnlockedText;

    private SpriteRenderer chestSpriteRenderer;
    private Sprite[] chestSprites;

    private bool unlocked;

    void Start() {
        playerScript = GameObject.Find("Player").GetComponent<MovementScript>();

        unlockedText = GameObject.Find("HUD/Panel/Chests/Unlocked").GetComponent<CanvasGroup>();
        notUnlockedText = GameObject.Find("HUD/Panel/Chests/NoKey").GetComponent<CanvasGroup>();

        chestSpriteRenderer = GetComponent<SpriteRenderer>();
        chestSprites = Resources.LoadAll<Sprite>("Sprites/Chest");

        unlocked = false;
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "Player" && !unlocked) {
            if(playerScript.amntKeys > 0) {
                unlocked = true;
                playerScript.amntKeys--;
                chestSpriteRenderer.sprite = chestSprites[1];

                // Choose spot to spawn item
                Vector3 spawnSpot;
                if(other.transform.position.y > transform.position.y) {
                    spawnSpot = transform.position + Vector3.down * 1.5f;
                } else {
                    spawnSpot = transform.position + Vector3.up;
                }

                // Spawn chest item
                GameObject spawnManager = GameObject.Find("ItemSpawnManager");
                GameObject selectedItem = spawnManager.GetComponent<ItemSpawnManager>().genRandomChestItem();
                Instantiate(selectedItem, spawnSpot, Quaternion.identity, spawnManager.transform);

                unlockedText.alpha = 1;
                notUnlockedText.alpha = 0;
            } else {
                unlockedText.alpha = 0;
                notUnlockedText.alpha = 1;
            }

            StopAllCoroutines();

            StartCoroutine("TextFade");
        }
    }

    IEnumerator TextFade() {
        float timer = 5.0f;

        while(timer > 0.0f) {
            timer -= Time.deltaTime;

            if(unlocked) {
                unlockedText.alpha = timer / 5;
            } else {
                notUnlockedText.alpha = timer / 5;
            }

            yield return null;
        }

        yield return null;
    }

}
