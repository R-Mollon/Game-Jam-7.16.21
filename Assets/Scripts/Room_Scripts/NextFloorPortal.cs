﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextFloorPortal : MonoBehaviour {
    
    private Transform sprite;

    private bool generated = false;

    void Start() {
        sprite = transform.Find("Sprite");
    }

    void Update() {
        sprite.Rotate(0, 0, Time.deltaTime * 20.0f);
    }

    void OnTriggerEnter2D(Collider2D other) {

        if(generated) return;

        generated = true;

        // Set player to next floor
        GameObject.Find("FloorStorage").GetComponent<Floor_Storage>().floorNumber++;

        GameObject.Find("HUD/LoadingNewFloor").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("Player").transform.position = new Vector3(0, 0, -1);

        Generate_World genScript = GameObject.Find("World").GetComponent<Generate_World>();
        genScript.roomsToBoss += 5;
        genScript.Generate();

        AudioSource music = GameObject.Find("Music").GetComponent<AudioSource>();
        music.volume = 0.25f;
        music.Play();

        foreach(Transform child in GameObject.Find("ItemSpawnManager").transform) {
            Destroy(child.gameObject);
        }
        
        //Destroy(GameObject.Find("World"));
        //GameObject world = Instantiate(Resources.Load<GameObject>("Prefabs/World"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //world.name = "World";
        //world.GetComponent<Generate_World>().roomsToBoss += 5 * GameObject.Find("FloorStorage").GetComponent<Floor_Storage>().floorNumber;

        Destroy(gameObject);

    }


}
