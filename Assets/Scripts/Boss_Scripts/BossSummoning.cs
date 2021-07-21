using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummoning : MonoBehaviour {
    
    public SpriteRenderer[] cultistRenderers;
    public ParticleSystem[] cultistParticles;
    private Sprite[] cultistSprites;

    private SpriteRenderer pentagramSprite;
    private Light spotLight;

    private GameObject[] bosses;

    void Start() {

        cultistSprites = Resources.LoadAll<Sprite>("Sprites/Cultist");
        bosses = Resources.LoadAll<GameObject>("Prefabs/Bosses");

        pentagramSprite = GetComponent<SpriteRenderer>();
        spotLight = transform.Find("Light").GetComponent<Light>();

        StartCoroutine("CultistSummonAnimation");
    }


    IEnumerator CultistSummonAnimation() {
        int cultistsAnim = 0;
        float cultistsTimer = 0.0f;
        float globalTimer = 0.0f;

        while(true) {
            cultistsTimer += Time.deltaTime;
            globalTimer += Time.deltaTime;

            if(cultistsTimer >= 0.5f) {
                cultistsTimer = 0.0f;
                cultistsAnim++;

                if(cultistsAnim > 3) {
                    cultistsAnim = 0;
                }
            }

             for(int i = 0; i < cultistRenderers.Length; i++) {
                cultistRenderers[i].sprite = cultistSprites[2 + cultistsAnim];
             }

            if(globalTimer >= 5.0f) {
                globalTimer = 6.5f;
                break;
            }

            yield return null;
        }

        Instantiate(bosses[0], transform.position + new Vector3(0, 1.7f, 0), Quaternion.identity);

        while(true) {
            cultistsTimer += Time.deltaTime;
            globalTimer -= Time.deltaTime;

            if(cultistsTimer >= 0.5f) {
                cultistsTimer = 0.0f;
                cultistsAnim++;

                if(cultistsAnim > 3) {
                    cultistsAnim = 0;
                }
            }

            float alpha = globalTimer / 6.5f;

            for(int i = 0; i < cultistRenderers.Length; i++) {
                spotLight.intensity = 1000 * alpha;
                pentagramSprite.color = new Color(1, 1, 1, alpha);
                cultistRenderers[i].sprite = cultistSprites[2 + cultistsAnim];
                cultistRenderers[i].color = new Color(1, 1, 1, alpha);
                cultistParticles[i].startColor = new Color(255, 0, 0, alpha);
            }

            yield return null;
        }
    }

}
