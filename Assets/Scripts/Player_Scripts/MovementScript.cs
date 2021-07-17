using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {
   
    private float playerSpeed = 8.0f;
    private int playerDirection = 0;

    private Rigidbody2D rigidBody;
    private Camera camera;
    private SpriteRenderer playerSprite;
    private Sprite[] playerSpriteList;

    public bool cameraLocked = true;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerSpriteList = Resources.LoadAll<Sprite>("Sprites/Player");
    }


    void Update() {

        Vector2 direction = new Vector2(0, 0);

        if(Input.GetKey(KeyCode.W)) {
            direction.y = 1;
            playerDirection = 2;
        }
        if(Input.GetKey(KeyCode.A)) {
            direction.x = -1;
            playerDirection = 4;
        }
        if(Input.GetKey(KeyCode.S)) {
            direction.y = -1;
            playerDirection = 0;
        }
        if(Input.GetKey(KeyCode.D)) {
            direction.x = 1;
            playerDirection = 1;
        }

        rigidBody.MovePosition(rigidBody.position + (direction * playerSpeed * Time.deltaTime));

        // Move camera to center on player if camera is locked
        if(cameraLocked) {
            camera.transform.position = new Vector3(rigidBody.position.x, rigidBody.position.y, -10);
        }

        playerSprite.sprite = playerSpriteList[playerDirection];

    }

}
