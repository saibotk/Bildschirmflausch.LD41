using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    GameObject score;
    GameObject pauseMenu;

    [SerializeField]
    NotificationManager notifications;

    [SerializeField]
    GameObject gameOverPanel;

    [SerializeField]
    HealthbarController healthcontroller;

    [SerializeField]
    int mainMenuSceneIndex = 0;
    int firstSceneIndex = 1;


	void Update() {
        if (Input.GetKey(KeyCode.R) && GameController.instance.GameEnded()) {
            LoadSceneByIndex(firstSceneIndex);
        }
	}

    public void ShowPauseMenu() {
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu() {
        pauseMenu.SetActive(false);
    }

    public void LoadSceneByIndex(int index) {
        Debug.Log("Loaded scene " + index);
        SceneManager.LoadScene(index);
    }

    public void QuitGame() {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void ShowGameOverUI() {
        if (gameOverPanel != null) {
            Debug.Log("Loaded Canvas");
            gameOverPanel.SetActive(true);

        } else {
            Debug.Log("No game over panel assigned");
        }
    }

    public void InitHealthController(Player ply) {
        healthcontroller.SetPlayer(ply);
    }

    public NotificationManager GetNotificationManager() {
        return notifications;
    }
}

