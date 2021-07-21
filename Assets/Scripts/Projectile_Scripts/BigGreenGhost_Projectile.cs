using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGreenGhost_Projectile : MonoBehaviour {
    
    private float speed = 4.0f;
    private Vector3 direction;

    private float lifetime = 5.0f;

    private Transform bossTransform;

    void Start() {
        bossTransform = GameObject.FindGameObjectsWithTag("BigGreenGhost")[0].transform;
    }

    void Update() {
        
        transform.position = Vector2.MoveTowards(transform.position, bossTransform.position, -1 * speed * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if(lifetime < 0.0f) {
            Destroy(gameObject);
        }

    }

}
