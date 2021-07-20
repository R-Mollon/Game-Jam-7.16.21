using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGhost_Projectile : MonoBehaviour {

    private float speed = 2.0f;
    private Transform player;

    private float lifetime;
    
    void Start() {
        player = GameObject.Find("Player").transform;
    }

    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        lifetime += Time.deltaTime;

        if(lifetime > 8.0f) {
            Destroy(gameObject);
        }
    }

}
