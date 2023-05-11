using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuConnector : MonoBehaviour{
    
    public void OpenMainMenu() {
        if (MainMenuController.inst == null) {
            SceneManager.LoadScene(0);
        } else {
            MainMenuController.inst.OpenMainMenu();
        }
    }
}
