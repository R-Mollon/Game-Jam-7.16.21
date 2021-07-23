using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle_Attack : MonoBehaviour {

    public Vector3 angle;
    private float speed = 8.0f;

    private float lifetime = 0.0f;
    public float maxLifetime = 8.0f;

    private bool doMovement = false;
    private bool started = false;

    private Transform rotater;
    private Transform pentagram;
    private Rigidbody2D rigidBody;
    private SpriteRenderer pentagramRenderer;

    public void go() {
        rotater = transform.GetChild(0);
        pentagram = transform.GetChild(1);
        pentagramRenderer = pentagram.gameObject.GetComponent<SpriteRenderer>();

        rigidBody = GetComponent<Rigidbody2D>();

        started = true;
    }


    void Update() {

        if(!started)
            return;

        if(!doMovement) {

            if(rotater.childCount > 0) {

                Cultist_Projectile script = rotater.GetChild(0).gameObject.GetComponent<Cultist_Projectile>();

                pentagramRenderer.color = new Color32(255, 255, 255, (byte) (Mathf.Clamp((script.maxAnimStage - script.animStage - 6), 0, 30) * 8));

                if(script.doneSpawning) {
                    doMovement = true;
                    pentagramRenderer.color = new Color32(255, 255, 255, 0);
                    StartCoroutine("Move");
                }
            }
        }

        lifetime += Time.deltaTime;
        if(lifetime > maxLifetime) {
            Destroy(gameObject);
        }
    }


    IEnumerator Move() {

        while(true) {
            rotater.Rotate(0, 0, Time.deltaTime * 100.0f);
            //Vector2 upTransform = new Vector2(transform.up.x, transform.up.y);
            //rigidBody.MovePosition(rigidBody.position + (upTransform * speed * Time.deltaTime));
            transform.position = transform.position + (angle * Time.deltaTime * speed);

            yield return null;
        }

    }

}
