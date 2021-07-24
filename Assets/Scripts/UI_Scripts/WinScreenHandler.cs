using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreenHandler : MonoBehaviour {

    private bool chosen = false;
    
    public void continuePlaying() {
        if(chosen) {
            return;
        }

        chosen = true;

        GameObject.Find("Player/SwingContainer/SwingZone").GetComponent<Damage_Storage>().damageMultiplier /= 1.25f;

        GameObject.Find("HUD/GameEnd/Power").SetActive(false);
        GameObject.Find("HUD/GameEnd/Accept").SetActive(false);

        GameObject.Find("ItemSpawnManager").GetComponent<ItemSpawnManager>().resetItems();

        // Set player to floor 0
        GameObject.Find("FloorStorage").GetComponent<Floor_Storage>().floorNumber = 0;

        GameObject.Find("HUD/GameEnd").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("HUD/Loading").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("Player").transform.position = new Vector3(0, 0, -1);

        Generate_World genScript = GameObject.Find("World").GetComponent<Generate_World>();
        genScript.roomsToBoss = 10;
        genScript.Generate();

        AudioSource music = GameObject.Find("Music").GetComponent<AudioSource>();
        music.volume = 0.25f;
        music.Play();

        AudioListener.volume = 1;

        foreach(Transform child in GameObject.Find("ItemSpawnManager").transform) {
            Destroy(child.gameObject);
        }

        chosen = false;
    }

    public void acceptVictory() {
        if(chosen) {
            return;
        }

        chosen = true;

        SceneManager.LoadScene(0);
    }

}
