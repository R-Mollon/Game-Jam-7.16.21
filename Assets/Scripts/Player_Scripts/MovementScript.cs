using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {
   
    private float playerSpeed = 1.0f;

    private Rigidbody2D rigidBody;
    private Camera camera;

    public bool cameraLocked = true;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
    }


    void FixedUpdate() {

        Vector2 direction = new Vector2(0, 0);

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

        // Move camera to center on player if camera is locked
        if(cameraLocked) {
            camera.transform.position = rigidBody.position;
        }

    }

}
