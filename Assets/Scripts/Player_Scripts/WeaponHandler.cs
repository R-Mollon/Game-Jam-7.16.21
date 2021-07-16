using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {
    
	public bool canAttack = true;
	
	
	private float attackCooldown = 0.0f;
	private float maxAttackCool  = 1.0f;
    private float swingPersist = 0.5f;


    private Transform swingTransform;
    private CapsuleCollider2D swingCollider;
    private SpriteRenderer swingSprite;

    void Start() {
        swingTransform = GameObject.Find("Player/SwingContainer").GetComponent<Transform>();
        swingCollider = GameObject.Find("Player/SwingContainer/SwingZone").GetComponent<CapsuleCollider2D>();
        swingSprite = GameObject.Find("Player/SwingContainer/SwingZone/SwingSprite").GetComponent<SpriteRenderer>();
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
        // Unity seems to dislike me doing this async
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

        swingTransform.localRotation = Quaternion.Euler(0, 0, angle);

        swingCollider.enabled = true;
        swingSprite.enabled = true;

        StartCoroutine("showAttack");
		
	}

    IEnumerator showAttack() {
        float time = 0f;

        while(time < this.swingPersist) {
            time += Time.deltaTime;

            yield return null;
        }

        swingCollider.enabled = false;
        swingSprite.enabled = false;

        yield return null;
    }

}
