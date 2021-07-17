using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {
    
	public bool canAttack = true;
	
	
	private float attackCooldown = 0.0f;
	private float maxAttackCool  = 1.0f;
    private float swingPersist = 0.5f;
    private float lastAngle = 0.0f;


    private Transform swingTransform;
    private PolygonCollider2D swingCollider;
    private SpriteRenderer swingSprite;
    private MovementScript moveScript;

    void Start() {
        swingTransform = GameObject.Find("Player/SwingContainer").GetComponent<Transform>();
        swingCollider = GameObject.Find("Player/SwingContainer/SwingZone").GetComponent<PolygonCollider2D>();
        swingSprite = GameObject.Find("Player/SwingContainer/SwingZone/SwingSprite").GetComponent<SpriteRenderer>();

        moveScript = gameObject.GetComponent<MovementScript>();
    }

	
    void FixedUpdate() {

        if(attackCooldown > 0) {
            attackCooldown -= Time.deltaTime;
        }

        // Set cooldown back to 0 if it goes below
        if(attackCooldown <= 0) {
            attackCooldown = 0;
        }

    }

    void Update() {
        // Unity seems to dislike doing this async
        if(Input.GetMouseButtonDown(0) && attackCooldown <= 0) {
            doAttack();
            attackCooldown = maxAttackCool;
        }
    }


    void doAttack() {
		
		Vector3 clickPos = Input.mousePosition;

        float xDelta = (Screen.width / 2) - clickPos.x;
        float yDelta = clickPos.y - (Screen.height / 2);

        float angleRadians = Mathf.Atan2(xDelta, yDelta);
        float angle = (360 / (Mathf.PI * 2)) * angleRadians;
        lastAngle = angle;

        swingTransform.localRotation = Quaternion.Euler(0, 0, angle);

        swingCollider.enabled = true;
        swingSprite.enabled = true;

        moveScript.attacking = true;

        StartCoroutine("showAttack");
		
	}

    IEnumerator showAttack() {
        float time = 0f;

        while(time < this.swingPersist) {
            time += Time.deltaTime;

            // Face player towards swing
            if(lastAngle >= -45.0f && lastAngle <= 45.0f) {
                // Swinging up
                moveScript.playerDirection = 2;
            } else if(lastAngle >= -135.0f && lastAngle <= -45.0f) {
                // Swinging right
                moveScript.playerDirection = 1;
            } else if(lastAngle >= 45.0f && lastAngle <= 135.0f) {
                // Swinging left
                moveScript.playerDirection = 3;
            } else {
                // Swinging down
                moveScript.playerDirection = 0;
            }

            yield return null;
        }

        swingCollider.enabled = false;
        swingSprite.enabled = false;

        moveScript.attacking = false;

        yield return null;
    }

}
