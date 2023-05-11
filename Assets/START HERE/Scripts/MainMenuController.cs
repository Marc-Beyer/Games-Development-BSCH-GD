using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour{

    public static MainMenuController inst;

    public bool CanEscape = true;

    [SerializeField] private GameObject mainMenuObj;
    [SerializeField] private GameObject creditsObj;

    private void Awake() {
        inst = this;
        DontDestroyOnLoad(gameObject);
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update() {
        if (CanEscape && Input.GetKeyDown(KeyCode.Escape)) {
            OpenMainMenu();
        }
    }

    public void OpenMainMenu() {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void LoadScene(int nr) {
        mainMenuObj.SetActive(false);
        SceneManager.LoadScene(nr);
    }

    public void ToggleCredits() {
        creditsObj.SetActive(!creditsObj.activeInHierarchy);
    }
}
