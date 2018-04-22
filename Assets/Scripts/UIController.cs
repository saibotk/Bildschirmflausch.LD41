using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    private GameObject score;
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private HealthbarController healthcontroller;

    public void ShowPauseMenu() {
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu() {
        pauseMenu.SetActive(false);
    }

    public void LoadSceneByIndex(int index) {
        Debug.Log("Loaded scene " + index);
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public void QuitGame() {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void ShowGameOverUI() {
        if ( gameOverPanel != null ) {
            Canvas gameOverCanvas = gameOverPanel.GetComponent<Canvas>();
            Debug.Log("Loading Canvas");

            if ( gameOverCanvas != null ) {
                Debug.Log("Loaded Canvas");
                gameOverCanvas.enabled = true;
            } else {
                Debug.Log("Gameover panel has no Canvas");
            }
        } else {
            Debug.Log("No game over panel assigned");
        }
    }

    public void InitHealthController(Player ply) {
        healthcontroller.SetPlayer(ply);
    }
}

