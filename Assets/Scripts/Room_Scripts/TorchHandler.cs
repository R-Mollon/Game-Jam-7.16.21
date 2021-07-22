using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchHandler : MonoBehaviour {
    
    public int direction = 0;

    private Light torchLight;

    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;

    void Start() {
        torchLight = transform.Find("Point Light").GetComponent<Light>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Sprites/Torch");
    }


    public void light() {
        transform.Find("Point Light").gameObject.SetActive(true);
        transform.Find("ParticleEmitter").gameObject.SetActive(true);

        spriteRenderer.sprite = sprites[direction + 1];

        StartCoroutine("TorchFlicker");
    }


    IEnumerator TorchFlicker() {
        float timer = 0.0f;
        float colorTimer = 0.0f;

        bool direction = true;
        bool colorDirection = true;

        while(true) {
            if(direction) {
                timer += Time.deltaTime;
            } else {
                timer -= Time.deltaTime;
            }

            if(colorDirection) {
                colorTimer += Time.deltaTime;
            } else {
                colorTimer -= Time.deltaTime;
            }

            if(timer >= 1.0f) {
                direction = false;
            }
            if(timer <= -1.0f) {
                direction = true;
            }

            if(colorTimer >= 5.0f) {
                colorDirection = false;
            }
            if(colorTimer <= 0.0f) {
                colorDirection = true;
            }

            torchLight.range = 5 + (timer / 2f);
            torchLight.color = new Color(1f, 0.25f + (colorTimer / 15f), 0f, 1f);

            yield return null;
        }
    }

}
