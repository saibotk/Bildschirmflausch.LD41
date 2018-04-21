using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    public UIController()
    {
        UIController.instance = this;
    }

    private GameObject score;
    private GameObject pauseMenu;
    private GameObject GameOverPanel;
    // Use this for initialization
    void Start () {
	}
	// Update is called once per frame
	void Update () {
	}
    public static UIController instance;

    public void quitGame()
    {
        Application.Quit();
    }
    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }



}

