using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCultistProjectile : MonoBehaviour {
    
    public Vector3 angle;

    private float speed = 12.0f;

    private bool started = false;

    private float lifetime;

    public void go() {
        started = true;
    }

    void Update() {
        if(!started) {
            return;
        }

        transform.position = transform.position + (angle * Time.deltaTime * speed);

        lifetime += Time.deltaTime;
        if(lifetime > 8.0f) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Wall") {
            Destroy(gameObject);
        }
    }

}
