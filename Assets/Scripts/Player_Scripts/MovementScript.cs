using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {
   
    private float playerSpeed = 10.0f;
    public int playerDirection = 0;
    public bool attacking = false;
    public bool canMove = true;

    private int walkStage = 1;
    private float walkTimer = 0.0f;
    private float maxWalk = 20.0f;

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
        bool walking = false;

        if(Input.GetKey(KeyCode.W)) {
            walking = true;
            direction.y = 1;
            if(!attacking) {
                playerDirection = 2;
            }
        }
        if(Input.GetKey(KeyCode.A)) {
            walking = true;
            direction.x = -1;
           if(!attacking) {
                playerDirection = 3;
            }
        }
        if(Input.GetKey(KeyCode.S)) {
            walking = true;
            direction.y = -1;
            if(!attacking) {
                playerDirection = 0;
            }
        }
        if(Input.GetKey(KeyCode.D)) {
            walking = true;
            direction.x = 1;
            if(!attacking) {
                playerDirection = 1;
            }
        }

        if(canMove) {
            rigidBody.MovePosition(rigidBody.position + (direction * playerSpeed * Time.deltaTime));
        }

        // Move camera to center on player if camera is locked
        if(cameraLocked) {
            camera.transform.position = new Vector3(rigidBody.position.x, rigidBody.position.y, -10);
        }

        // Handle player sprite
        if(!walking) {
            walkStage = 1;
            playerSprite.sprite = playerSpriteList[playerDirection * 4];
        } else {

            playerSprite.sprite = playerSpriteList[(playerDirection * 4) + walkStage];

            walkTimer += Time.deltaTime * 40.0f;

            if(walkTimer >= (maxWalk - playerSpeed)) {
                walkStage++;
                walkTimer = 0.0f;
            }

            if(walkStage > 3) {
                walkStage = 0;
            }
        }

    }

}
