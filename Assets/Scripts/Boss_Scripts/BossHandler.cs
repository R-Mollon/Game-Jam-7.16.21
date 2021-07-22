using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHandler : MonoBehaviour {
    
    private CanvasGroup showBar;
    private Text bossName;
    private Image bossBar;

    private GameObject mainCamera;

    private MovementScript moveScript;
    private WeaponHandler attackScript;

    private AudioSource worldMusic;
    private AudioSource bossMusic;

    private bool started = false;

    void Start() {
        showBar = GameObject.Find("HUD/Panel/BossBar").GetComponent<CanvasGroup>();
        bossName = GameObject.Find("HUD/Panel/BossBar/Name").GetComponent<Text>();
        bossBar = GameObject.Find("HUD/Panel/BossBar/BarGreen").GetComponent<Image>();
        mainCamera = GameObject.Find("Main Camera");

        worldMusic = GameObject.Find("Music").GetComponent<AudioSource>();
        bossMusic = GameObject.Find("BossMusic").GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(!started && other.name == "Player") {
            moveScript = other.GetComponent<MovementScript>();
            attackScript = other.GetComponent<WeaponHandler>();

            started = true;

            StartCoroutine("StartFight");
        }
    }


    IEnumerator StartFight() {
        moveScript.canMove = false;
        attackScript.canAttack = false;
        moveScript.cameraLocked = false;

        Transform spawnSpot = GameObject.Find("Room-Middle").transform;

        GameObject bossSpawner = Resources.Load<GameObject>("Prefabs/BossSummoning");

        Instantiate(bossSpawner, spawnSpot.position, Quaternion.identity);

        float timer = 11.75f;

        float worldStartVolume = worldMusic.volume;
        bool musicSwitched = false;

        while(true) {
            timer -= Time.deltaTime;

            if(!musicSwitched) {
                if(worldMusic.volume > 0.0f) {
                    worldMusic.volume -= worldStartVolume * Time.deltaTime * 2;
                } else {
                    worldMusic.Stop();
                    bossMusic.Play();
                    musicSwitched = true;
                }
            }

            mainCamera.transform.position = Vector2.MoveTowards(mainCamera.transform.position, spawnSpot.position, Time.deltaTime * 30.0f);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -1);

            if(timer <= 0.0f) {
                break;
            }
        
            yield return null;
        }


        moveScript.canMove = true;
        attackScript.canAttack = true;
        moveScript.cameraLocked = true;

        StartCoroutine("FillBar");

        yield return null;
    }


    IEnumerator ShowHealth() {

        BigGreenGhost ghostBoss = GameObject.FindGameObjectsWithTag("BigGreenGhost")[0].GetComponent<BigGreenGhost>();

        while(true) {
            float healthPercentage = ghostBoss.health / ghostBoss.maxHealth;

            bossBar.rectTransform.sizeDelta = new Vector2(395f * healthPercentage, 15);
            bossBar.rectTransform.localPosition = new Vector3((395f - (395f * healthPercentage)) / -2, -196, 0);

            if(ghostBoss.health <= 0) {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(6.0f);

        showBar.alpha = 0;

    }


    IEnumerator FillBar() {
        float timer = 0.0f;

        bossName.text = "Nightmare Ghost";
        showBar.alpha = 1;

        while(true) {
            timer += Time.deltaTime;

            if(timer < 1.0f) {
                bossBar.rectTransform.sizeDelta = new Vector2(395f * timer, 15);
                bossBar.rectTransform.localPosition = new Vector3((395f - (395f * timer)) / -2, -196, 0);
            } else {
                bossBar.rectTransform.sizeDelta = new Vector2(395f, 15);
                bossBar.rectTransform.localPosition = new Vector3(0, -196, 0);
                break;
            }

            yield return null;
        }

        StartCoroutine("ShowHealth");

        yield return null;
    }

}
