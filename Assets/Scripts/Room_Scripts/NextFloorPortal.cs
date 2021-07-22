using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextFloorPortal : MonoBehaviour {
    

    void OnTriggerEnter2D(Collider2D other) {

        // Set player to next floor
        GameObject.Find("FloorStorage").GetComponent<Floor_Storage>().floorNumber++;
        Generate_World worldGen = GameObject.Find("World").GetComponent<Generate_World>();

        worldGen.roomsToBoss += 5;
        GameObject.Find("HUD/LoadingNewFloor").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("Player").transform.position = new Vector3(0, 0, -1);
        worldGen.Generate();

    }


}
