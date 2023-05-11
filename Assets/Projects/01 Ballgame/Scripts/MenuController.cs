using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public static MenuController instance;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI endText;
    [SerializeField] GameObject endPanel;

    private GameController gameController;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        gameController = GameController.instance;
    }

    public void UpdateScore() {
        scoreText.text = $"Score: {gameController.Score}";
    }

    public void UpdateEnd() {
        Time.timeScale = 0;

        if (PlayerPrefs.GetInt("ball_highscore") < gameController.Score) PlayerPrefs.SetInt("ball_highscore", gameController.Score);

        endText.text = $"Highscore: {PlayerPrefs.GetInt("ball_highscore")}\nScore: {gameController.Score}";

        scoreText.gameObject.SetActive(false);
        endPanel.SetActive(true);
    }

    public void Retry() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
