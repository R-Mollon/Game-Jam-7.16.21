using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCultistAttack : MonoBehaviour {
    
    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;

    private bool activated = false;

    private float timer = 0.0f;


    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Sprites/BlueCultistAttack");
    }

    
    void Update() {
        timer += Time.deltaTime;

        if(timer >= 1.2f && !activated) {
            activated = true;
            spriteRenderer.sprite = sprites[1];
            GetComponent<CapsuleCollider2D>().enabled = true;
        }

        if(timer >= 1.4f && timer < 1.5f) {
            spriteRenderer.sprite = sprites[2];
        }
        if(timer >= 1.5f && timer < 1.6f) {
            spriteRenderer.sprite = sprites[3];
        }
        if(timer >= 1.6f && timer < 1.7f) {
            spriteRenderer.sprite = sprites[4];
        }
        if(timer >= 1.7f && timer < 1.8f) {
            spriteRenderer.sprite = sprites[5];
        }
        if(timer >= 1.8f && timer < 1.9f) {
            spriteRenderer.sprite = sprites[0];
        }
        if(timer >= 1.9f && timer < 2.0f) {
            Destroy(gameObject);
        }
    }

}
