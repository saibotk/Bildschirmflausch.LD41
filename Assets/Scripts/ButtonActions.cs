using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour {
    public void LoadSceneByIndex(int index) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public void QuitGame() {
        UnityEngine.Application.Quit();
    }
}
