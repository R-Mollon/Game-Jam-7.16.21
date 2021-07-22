using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {
    
    public string itemType;
    public string itemName;
    public string itemDesc;

    public void pickup() {
        bool pickedUp = false;

        PlayerDamage damageScript = GameObject.Find("Player/PlayerDamageCollider").GetComponent<PlayerDamage>();
        MovementScript moveScript = GameObject.Find("Player").GetComponent<MovementScript>();
        WeaponHandler weaponScript = GameObject.Find("Player").GetComponent<WeaponHandler>();
        Damage_Storage playerDamage = GameObject.Find("Player/SwingContainer/SwingZone").GetComponent<Damage_Storage>();

        switch(itemType) {
            case "Heart":
                if(damageScript.playerHealth < damageScript.maxHealth) {
                    damageScript.playerHealth += 2;

                    if(damageScript.playerHealth > damageScript.maxHealth) {
                        damageScript.playerHealth = damageScript.maxHealth;
                    }

                    pickedUp = true;
                }
                break;
            case "Key":
                moveScript.amntKeys++;
                pickedUp = true;
                break;
            case "Speed":
                moveScript.playerSpeed += 1.0f;
                pickedUp = true;
                break;
            case "AllSpeed":
                moveScript.playerSpeed += 1.0f;
                weaponScript.maxAttackCool -= 0.1f;
                pickedUp = true;
                break;
            case "Medkit":
                damageScript.maxHealth += 2;
                damageScript.playerHealth = damageScript.maxHealth;
                pickedUp = true;
                break;
            case "BigSword":
                weaponScript.maxAttackCool += 0.3f;
                playerDamage.damage += 3.0f;
                pickedUp = true;
                break;
            case "EnchantedSword":
                weaponScript.maxAttackCool -= 0.2f;
                pickedUp = true;
                break;
            case "Chalice":
                damageScript.maxHealth += 4;
                damageScript.playerHealth += 4;
                pickedUp = true;
                break;
            case "CrystalHeart":
                damageScript.maxHealth += 2;
                damageScript.playerHealth += 2;
                pickedUp = true;
                break;
            case "Dumbbell":
                playerDamage.damage += 1.0f;
                pickedUp = true;
                break;
            case "GreenSludge":
                moveScript.playerSpeed += 1.0f;
                damageScript.maxHealth += 2;
                damageScript.playerHealth += 2;
                weaponScript.maxAttackCool -= 0.1f;
                playerDamage.damage += 1.0f;
                pickedUp = true;
                break;
        }

        // Enforce caps on things that need to be capped
        if(weaponScript.maxAttackCool < 0.1f) {
            weaponScript.maxAttackCool = 0.1f;
        }
        if(damageScript.maxHealth > 18) {
            damageScript.maxHealth = 18;
        }

        if(pickedUp) {
            GameObject.Find("HUD/Panel/ItemCollection").GetComponent<ItemCollection>().announcePickup(itemName, itemDesc);
            Destroy(gameObject);
        }
    }


}
