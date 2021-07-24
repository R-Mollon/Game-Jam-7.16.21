using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour {

    private Image titleImage;
    private Sprite[] images;

    private float timer;
    private float maxTimer;
    
    void Start() {
        titleImage = GetComponent<Image>();

        images = Resources.LoadAll<Sprite>("Sprites/Logo");

        maxTimer = Random.Range(20, 50) / 10;

        Time.timeScale = 1.0f;
    }

    void Update() {
        timer += Time.deltaTime;

        if(timer > maxTimer) {
            titleImage.sprite = images[1];

            if(timer > maxTimer + 0.15f) {
                titleImage.sprite = images[2];
            }

            if(timer > maxTimer + 0.3f) {
                titleImage.sprite = images[3];
            }

            if(timer > maxTimer + 0.45f) {
                titleImage.sprite = images[0];
                timer = 0.0f;
                maxTimer = Random.Range(20, 50) / 10;
            }
        }
    }

}
