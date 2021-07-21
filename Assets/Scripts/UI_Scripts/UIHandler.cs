using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour {
    
    private GameObject camera;

    private SpriteRenderer door1;
    private SpriteRenderer door2;

    private Sprite[] doorSprites;

    void Start() {
        camera = GameObject.Find("Main Camera");

        door1 = GameObject.Find("Door").GetComponent<SpriteRenderer>();
        door2 = GameObject.Find("Door (1)").GetComponent<SpriteRenderer>();

        doorSprites = Resources.LoadAll<Sprite>("Sprites/Door");
    }

    public void playGame() {
        SceneManager.LoadScene(1);
    }


    public void controls() {
        StartCoroutine("MoveControls");
    }


    public void controlsBack() {
        StartCoroutine("MoveControlsBack");
    }


    IEnumerator MoveControls() {
        float timer = 0.0f;

        while(true) {
            timer += Time.deltaTime;

            if(timer < 0.25f) {
                door1.sprite = doorSprites[1];
                door2.sprite = doorSprites[1];
            }

            if(timer > 0.5f) {
                door1.sprite = doorSprites[2];
                door2.sprite = doorSprites[2];
            }

            if(timer > 0.75f) {
                break;
            }
        }

        while(true) {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, new Vector3(0, 16, -1), Time.deltaTime * 40.0f);

            if(camera.transform.position.y > 15.99999 && camera.transform.position.y < 16.00001) {
                break;
            }

            yield return null;
        }

        yield return null;
    }


    IEnumerator MoveControlsBack() {
        while(true) {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, new Vector3(0, 0, -1), Time.deltaTime * 40.0f);

            if(camera.transform.position.y > -0.00001 && camera.transform.position.y < 0.00001) {
                break;
            }

            yield return null;
        }

        float timer = 0.0f;

        while(true) {
            timer += Time.deltaTime;

            if(timer < 0.25f) {
                door1.sprite = doorSprites[1];
                door2.sprite = doorSprites[1];
            }

            if(timer > 0.5f) {
                door1.sprite = doorSprites[0];
                door2.sprite = doorSprites[0];
            }

            if(timer > 0.75f) {
                break;
            }
        }

        yield return null;
    }


    public void quitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
