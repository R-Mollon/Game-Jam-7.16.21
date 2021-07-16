using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {
   
    private float playerSpeed = 1.0f;

    private Rigidbody2D rigidBody;

    public Vector2 direction;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate() {

        direction = new Vector2(0, 0);

        if(Input.GetKey(KeyCode.W)) {
            direction.y = 1;
        }
        if(Input.GetKey(KeyCode.A)) {
            direction.x = -1;
        }
        if(Input.GetKey(KeyCode.S)) {
            direction.y = -1;
        }
        if(Input.GetKey(KeyCode.D)) {
            direction.x = 1;
        }

        rigidBody.MovePosition(rigidBody.position + (direction * playerSpeed * Time.deltaTime));

    }

}
