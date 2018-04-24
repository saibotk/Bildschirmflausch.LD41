using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    GameObject score;
    GameObject pauseMenu;

    [SerializeField]
    NotificationManager notifications;

    [SerializeField]
    GameObject restartUIPanel;

    [SerializeField]
    HealthbarController healthcontroller;

    [SerializeField]
    BrakeBarController brakeBarController;

    [SerializeField]
    GameObject pausedUIPanel;

    [SerializeField]
    int mainMenuSceneIndex = 0;
    int firstSceneIndex = 1;


	void Update() {
        if (Input.GetAxis("Reset") > 0 && GameController.instance.GameEnded()) {
            Restart();
        }
	}

    public void ShowPauseMenu() {
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu() {
        pauseMenu.SetActive(false);
    }

	public void Restart() {
		LoadSceneByIndex(firstSceneIndex);
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
        ShowRestartUI(false);
    }

    public void ShowWinUI() {
        ShowRestartUI(true);
    }

    void ShowRestartUI(bool won) {
        string headerText = won ? "You won!" : "Game Over";
        if (restartUIPanel != null) {
            restartUIPanel.SetActive(true);
            restartUIPanel.GetComponentInChildren<Text>().text = headerText;
        } else {
            Debug.Log("No restart panel assigned");
        }
    }

    public void InitHealthController(Player ply) {
        healthcontroller.SetPlayer(ply);
    }

    public void InitBrakeController(Player ply) {
        brakeBarController.SetPlayer(ply);
    }

    public NotificationManager GetNotificationManager() {
        return notifications;
    }

    public void showPauseUI(bool show) {
        if (pausedUIPanel != null) {
            pausedUIPanel.SetActive(show);
        } else {
            Debug.Log("Paused UI not assigned");
        }
    }
}
