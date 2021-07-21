using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Room Entered");
    }

}
