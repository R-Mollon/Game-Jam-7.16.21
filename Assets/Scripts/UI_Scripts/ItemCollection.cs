using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollection : MonoBehaviour {

    private CanvasGroup render;
    private Text itemName;
    private Text itemDesc;

    void Start() {
        render = GameObject.Find("HUD/Panel/ItemCollection").GetComponent<CanvasGroup>();
        itemName = GameObject.Find("HUD/Panel/ItemCollection/ItemName").GetComponent<Text>();
        itemDesc = GameObject.Find("HUD/Panel/ItemCollection/ItemDesc").GetComponent<Text>();
    }
    
    public void announcePickup(string name, string description) {
        render.alpha = 1;

        itemName.text = name;
        itemDesc.text = description;

        StopAllCoroutines();
        StartCoroutine("FadeAway");
    }


    IEnumerator FadeAway() {
        float timer = 0.0f;
        float maxTimer = 8.0f;

        while(timer < maxTimer) {
            timer += Time.deltaTime;

            if(timer > 4.0f) {
                render.alpha = (maxTimer - timer) / maxTimer;
            }

            yield return null;
        }

        render.alpha = 0;

        yield return null;
    }

}
