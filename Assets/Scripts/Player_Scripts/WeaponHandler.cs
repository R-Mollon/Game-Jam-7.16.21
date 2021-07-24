using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {
    
	public bool canAttack = true;
	
	
	private float attackCooldown = 0.0f;
	public float maxAttackCool  = 1.0f;
    private float swingPersist = 0.1f;
    private float lastAngle = 0.0f;


    private Transform swingTransform;
    private PolygonCollider2D swingCollider;
    private SpriteRenderer swingSprite;
    private MovementScript moveScript;

    private Sprite[] swingSprites;

    private AudioSource swingSound;

    void Start() {
        swingTransform = GameObject.Find("Player/SwingContainer").GetComponent<Transform>();
        swingCollider = GameObject.Find("Player/SwingContainer/SwingZone").GetComponent<PolygonCollider2D>();
        swingSprite = GameObject.Find("Player/SwingContainer/SwingZone/SwingSprite").GetComponent<SpriteRenderer>();

        swingSprites = Resources.LoadAll<Sprite>("Sprites/SwingAnimation");

        swingSound = GameObject.Find("Player/SwingContainer/SwingZone").GetComponent<AudioSource>();

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
        if(Input.GetMouseButton(0) && !moveScript.paused) {
            if(attackCooldown <= 0 && canAttack) {
                doAttack();
                attackCooldown = maxAttackCool;
            }
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

        swingSound.Play();

        StartCoroutine("showAttack");
		
	}

    IEnumerator showAttack() {
        float time = 0f;

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

        while(time < swingPersist) {
            time += Time.deltaTime;

            int animFrame = (int) Mathf.Clamp(((time / swingPersist) * 5), 0, 5);
            swingSprite.sprite = swingSprites[animFrame];

            yield return null;
        }

        swingCollider.enabled = false;
        swingSprite.enabled = false;

        moveScript.attacking = false;

        yield return null;
    }

}
