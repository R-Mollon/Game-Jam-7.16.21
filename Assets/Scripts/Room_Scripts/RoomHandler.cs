using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour {
    
    private bool roomEntered = false;
    private bool enemiesSpawned = false;
    private bool roomCompleted = false;

    public GameObject[] doors;
    public GameObject[] torches;
    private Sprite[] doorSprites;
    private Sprite[] vertSprites;

    private int floorNum = 0;

    GameObject[] enemySets;

    Coroutine closeDoor;

    void Start() {
        doorSprites = Resources.LoadAll<Sprite>("Sprites/Door");
        vertSprites = Resources.LoadAll<Sprite>("Sprites/DoorVertical");

        floorNum = GameObject.Find("FloorStorage").GetComponent<Floor_Storage>().floorNumber;

        if(floorNum < 2) {
            enemySets = Resources.LoadAll<GameObject>("Prefabs/EnemySets/Floor" + floorNum);
        } else {
            enemySets = Resources.LoadAll<GameObject>("Prefabs/EnemySets/Floor1");
        }
    }

    public void onEnter() {
        if(roomEntered) {
            return;
        }

        roomEntered = true;

        // 1 in 10 chance to be a chest room instead of having enemies
        int roomType = Random.Range(0, 10);
        if(roomType == 7) {
            Instantiate(Resources.Load<GameObject>("prefabs/Chest"), transform.position, Quaternion.identity);
            // Light torches
            for(int i = 0; i < torches.Length; i++) {
                torches[i].GetComponent<TorchHandler>().light();
            }
            return;
        }

        for(int i = 0; i < doors.Length; i++) {
            doors[i].GetComponent<Collider2D>().enabled = true;
        }

        closeDoor = StartCoroutine("DoorCloseAnimation");

        // Select a random enemy set to spawn
        int setToSpawn = Random.Range(0, enemySets.Length);
        Instantiate(enemySets[setToSpawn], transform.position, Quaternion.identity, transform.Find("Enemies"));
        enemiesSpawned = true;
    }


    void Update() {
        if(!roomEntered || !enemiesSpawned) {
            return;
        }

        if(transform.Find("Enemies").GetChild(0).childCount == 0 && !roomCompleted) {
            // All enemies killed

            StopCoroutine(closeDoor);

            roomCompleted = true;

            for(int i = 0; i < doors.Length; i++) {
                doors[i].GetComponent<Collider2D>().enabled = false;
            }

            StartCoroutine("DoorOpenAnimation");

            // Spawn item
            GameObject spawnManager = GameObject.Find("ItemSpawnManager");
            GameObject itemToSpawn = spawnManager.GetComponent<ItemSpawnManager>().genRandomRoomReward();

            Instantiate(itemToSpawn, transform.position, Quaternion.identity, spawnManager.transform);


            // Light torches
            for(int i = 0; i < torches.Length; i++) {
                torches[i].GetComponent<TorchHandler>().light();
            }

        }
    }


    IEnumerator DoorCloseAnimation() {
        float timer = 0.0f;

        List<Sprite[]> sprites = new List<Sprite[]>();
        for(int i = 0; i < doors.Length; i++) {
            if(doors[i].GetComponent<DoorStorage>().vertical) {
                sprites.Add(vertSprites);
            } else {
                sprites.Add(doorSprites);
            }
        }

        while(timer < 1.0f) {
            timer += Time.deltaTime;

            if(timer >= 0.5f && timer < 0.6f) {
                for(int i = 0; i < doors.Length; i++) {
                    doors[i].GetComponent<SpriteRenderer>().sprite = sprites[i][1];
                }
            }

            if(timer >= 0.75f) {
                for(int i = 0; i < doors.Length; i++) {
                    doors[i].GetComponent<SpriteRenderer>().sprite = sprites[i][0];
                }
            }

            yield return null;
        }

        for(int i = 0; i < doors.Length; i++) {
            doors[i].GetComponent<SpriteRenderer>().sprite = sprites[i][0];
        }

        yield return null;
    }


    IEnumerator DoorOpenAnimation() {
        float timer = 0.0f;

        List<Sprite[]> sprites = new List<Sprite[]>();
        for(int i = 0; i < doors.Length; i++) {
            if(doors[i].GetComponent<DoorStorage>().vertical) {
                sprites.Add(vertSprites);
            } else {
                sprites.Add(doorSprites);
            }
        }

        while(timer < 1.0f) {
            timer += Time.deltaTime;

            if(timer >= 0.5f && timer < 0.6f) {
                for(int i = 0; i < doors.Length; i++) {
                    doors[i].GetComponent<SpriteRenderer>().sprite = sprites[i][1];
                }
            }

            if(timer >= 0.75f) {
                for(int i = 0; i < doors.Length; i++) {
                    doors[i].GetComponent<SpriteRenderer>().sprite = sprites[i][2];
                }
            }

            yield return null;
        }

        for(int i = 0; i < doors.Length; i++) {
            doors[i].GetComponent<SpriteRenderer>().sprite = sprites[i][2];
        }

        yield return null;
    }

}
