using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour {
    
    public void unPause() {
        Time.timeScale = 1.0f;

        GameObject.Find("HUD/PauseMenu").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("HUD/PauseMenu/Background/Return").SetActive(false);
        GameObject.Find("HUD/PauseMenu/Background/Quit").SetActive(false);

        GameObject.Find("Player").GetComponent<MovementScript>().paused = false;
    }

    public void quit() {
        SceneManager.LoadScene(0);
    }

}
