using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerManager : MonoBehaviour {
    
    public bool staticPosition = false;

    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = (int) (-1 * transform.position.y);

        if(!staticPosition) {
            StartCoroutine("ChangeOrder");
        }
    }

    IEnumerator ChangeOrder() {
        while(true) {
            spriteRenderer.sortingOrder = (int) (-1 * transform.position.y);

            yield return null;
        }
    }

}
