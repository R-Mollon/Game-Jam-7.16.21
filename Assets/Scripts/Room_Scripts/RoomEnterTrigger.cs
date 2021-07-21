using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterTrigger : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "Player") {
            transform.parent.parent.gameObject.GetComponent<RoomHandler>().onEnter();
        }
    }

}
