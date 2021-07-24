using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour {
    
    private Dictionary<string, bool> collectedItems;

    void Start() {
        collectedItems = new Dictionary<string, bool>();

        collectedItems.Add("BroadSword", false);
        collectedItems.Add("Chalice", false);
        collectedItems.Add("CrystalHeart", false);
        collectedItems.Add("Dumbbell", false);
        collectedItems.Add("EnchantedSword", false);
        collectedItems.Add("Feather", false);
        collectedItems.Add("GreenSludge", false);
        collectedItems.Add("Medkit", false);
        collectedItems.Add("Pill", false);
        collectedItems.Add("Shoes", false);
    }


    public void resetItems() {
        collectedItems["BroadSword"] = false;
        collectedItems["Chalice"] = false;
        collectedItems["CrystalHeart"] = false;
        collectedItems["Dumbbell"] = false;
        collectedItems["EnchantedSword"] = false;
        collectedItems["Feather"] = false;
        collectedItems["GreenSludge"] = false;
        collectedItems["Medkit"] = false;
        collectedItems["Pill"] = false;
        collectedItems["Shoes"] = false;
    }


    public GameObject genRandomRoomReward() {

        int randomSelection = Random.Range(0, 1000);

        if(randomSelection < 750) {
            // 75% chance to get nothing
            return new GameObject();
        }

        if(randomSelection < 875) {
            // 50% chance for the item to be a heart
            return Resources.Load<GameObject>("Prefabs/Items/HeartPickup");
        }

        // If item is not a heart, item should be a key
        return Resources.Load<GameObject>("Prefabs/Items/KeyPickup");
    }


    public GameObject genRandomChestItem() {

        int attempts = 0;

        while(attempts < 20) {
            int randomSelection = Random.Range(0, 600);

            string itemName;
            if(randomSelection < 100) {
                itemName = "Feather";
                if(!collectedItems[itemName]) {
                    collectedItems[itemName] = true;
                    return Resources.Load<GameObject>("Prefabs/Items/" + itemName + "Item");
                }
            }

            if(randomSelection < 200) {
                itemName = "CrystalHeart";
                if(!collectedItems[itemName]) {
                    collectedItems[itemName] = true;
                    return Resources.Load<GameObject>("Prefabs/Items/" + itemName + "Item");
                }
            }

            if(randomSelection < 300) {
                itemName = "BroadSword";
                if(!collectedItems[itemName]) {
                    collectedItems[itemName] = true;
                    return Resources.Load<GameObject>("Prefabs/Items/" + itemName + "Item");
                }
            }

            if(randomSelection < 400) {
                itemName = "Shoes";
                if(!collectedItems[itemName]) {
                    collectedItems[itemName] = true;
                    return Resources.Load<GameObject>("Prefabs/Items/" + itemName + "Item");
                }
            }

            if(randomSelection < 500) {
                itemName = "Dumbbell";
                if(!collectedItems[itemName]) {
                    collectedItems[itemName] = true;
                    return Resources.Load<GameObject>("Prefabs/Items/" + itemName + "Item");
                }
            }

            if(randomSelection < 600) {
                itemName = "Pill";
                if(!collectedItems[itemName]) {
                    collectedItems[itemName] = true;
                    return Resources.Load<GameObject>("Prefabs/Items/" + itemName + "Item");
                }
            }

            attempts++;
        }

        // Make item a heart if no other item is selected
        return Resources.Load<GameObject>("Prefabs/Items/HeartPickup");
    }


}
