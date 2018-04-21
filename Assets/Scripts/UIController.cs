using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static UIController instance;

    private GameObject score;
    private GameObject pauseMenu;
    private GameObject GameOverPanel;

    public UIController()
    {
        UIController.instance = this;
    }
    public void quitGame()
    {
        Application.Quit();
    }
 
    
}

