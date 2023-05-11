using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RockPaperScissorsController : MonoBehaviour {

    public static RockPaperScissorsController inst;

    [SerializeField] private int nrOfCharacter = 20;

    [SerializeField] private Transform playerSpawnTopLeft;
    [SerializeField] private Transform playerSpawnBottomRight;

    [SerializeField] private Transform enemySpawnTopLeft;
    [SerializeField] private Transform enemySpawnBottomRight;

    [SerializeField] private GameObject characterObj;

    [SerializeField] private Toggle rockToggle;
    [SerializeField] private Toggle paperToggle;
    [SerializeField] private Toggle scissorsToggle;
    [SerializeField] private Toggle wellToggle;

    [SerializeField] private GameObject rockGameObject;
    [SerializeField] private GameObject paperGameObject;
    [SerializeField] private GameObject scissorsGameObject;
    [SerializeField] private GameObject wellGameObject;

    [SerializeField] private Slider rockSlider;
    [SerializeField] private Slider papeSlider;
    [SerializeField] private Slider scissorsSlider;
    [SerializeField] private Slider wellSlider;

    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject restartBtn;
    [SerializeField] private GameObject toolTip;

    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private TextMeshProUGUI enemyText;

    [SerializeField] private AudioClipPlayer AudioClipPlayer;

    private int _playerLeft = 0;
    private int _enemiesLeft = 0;

    private int playerLeft {
        get => _playerLeft;
        set {
            _playerLeft = value;
            playerText.text = _playerLeft.ToString();
        }
    }
    private int enemiesLeft {
        get => _enemiesLeft; set {
            _enemiesLeft = value;
            enemyText.text = _enemiesLeft.ToString();
        }
    }

    private void Awake() {
        inst = this;
    }

    public void clickedRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void clickedStart() {
        startBtn.SetActive(false);

        rockGameObject.SetActive(false);
        paperGameObject.SetActive(false);
        scissorsGameObject.SetActive(false);
        wellGameObject.SetActive(false);

        rockToggle.interactable = false;
        paperToggle.interactable = false;
        scissorsToggle.interactable = false;
        wellToggle.interactable = false;


        Time.timeScale = 0;
        spawnPlayer();
        Time.timeScale = 1;

        restartBtn.SetActive(true);
        playerText.gameObject.SetActive(true);
        enemyText.gameObject.SetActive(true);
    }

    internal void Killed(bool isEnemy) {
        if (isEnemy) {
            enemiesLeft--;
        } else {
            playerLeft--;
        }

        AudioClipPlayer.PlayRdmSound();
    }

    private void spawnPlayer() {
        float value = 0;
        int spawnedCharacters = 0;

        if (rockToggle.isOn) {
            value += rockSlider.value;
        }
        if (paperToggle.isOn) {
            value += papeSlider.value;
        }
        if (scissorsToggle.isOn) {
            value += scissorsSlider.value;
        }
        if (wellToggle.isOn) {
            value += wellSlider.value;
        }

        if (rockToggle.isOn) {
            for (int i = 0; i < nrOfCharacter * (rockSlider.value / value); i++) {
                spawnInPlayerSpawn(RPSType.ROCK);
                spawnedCharacters++;
            }
        }
        if (paperToggle.isOn) {
            for (int i = 0; i < nrOfCharacter * (papeSlider.value / value); i++) {
                spawnInPlayerSpawn(RPSType.PAPER);
                spawnedCharacters++;
            }
        }
        if (scissorsToggle.isOn) {
            for (int i = 0; i < nrOfCharacter * (scissorsSlider.value / value); i++) {
                spawnInPlayerSpawn(RPSType.SCISSORS);
                spawnedCharacters++;
            }
        }
        if (wellToggle.isOn) {
            for (int i = 0; i < nrOfCharacter * (wellSlider.value / value); i++) {
                spawnInPlayerSpawn(RPSType.WELL);
                spawnedCharacters++;
            }
        }

        spawnEnemeies(spawnedCharacters);

        enemiesLeft = spawnedCharacters;
        playerLeft = spawnedCharacters;
    }

    private void spawnEnemeies(int nr) {
        for (int i = 0; i < nr; i++) {
            RPSCharacter character = Instantiate(
                                characterObj,
                                new Vector3(
                                    Random.Range(enemySpawnTopLeft.position.x, enemySpawnBottomRight.position.x),
                                    Random.Range(enemySpawnTopLeft.position.y, enemySpawnBottomRight.position.y),
                                    0
                                ),
                                Quaternion.identity
                            ).GetComponent<RPSCharacter>();

            character.IsEnemy = true;
            character.CharacterType = randomRPSType();
        }
    }

    private RPSType randomRPSType() {
        float nr = Random.Range(1, 300);
        if (nr < 100) return RPSType.ROCK;
        if (nr < 200) return RPSType.PAPER;
        return RPSType.SCISSORS;
    }

    private void spawnInPlayerSpawn(RPSType characterType) {
        RPSCharacter character = Instantiate(
                            characterObj,
                            new Vector3(
                                Random.Range(playerSpawnTopLeft.position.x, playerSpawnBottomRight.position.x),
                                Random.Range(playerSpawnTopLeft.position.y, playerSpawnBottomRight.position.y),
                                0
                            ),
                            Quaternion.identity
                        ).GetComponent<RPSCharacter>();

        character.IsEnemy = false;
        character.CharacterType = characterType;
    }

    public void ChangedRock() {
        rockGameObject.SetActive(rockToggle.isOn);
        toggleStartBtn();
    }

    public void ChangedPaper() {
        paperGameObject.SetActive(paperToggle.isOn);
        toggleStartBtn();
    }
    public void ChangedScissors() {
        scissorsGameObject.SetActive(scissorsToggle.isOn);
        toggleStartBtn();
    }
    public void ChangedWell() {
        wellGameObject.SetActive(wellToggle.isOn);
        toggleStartBtn();
    }
    private void toggleStartBtn() {
        startBtn.SetActive(rockToggle.isOn || paperToggle.isOn || scissorsToggle.isOn || wellToggle.isOn);
        toolTip.SetActive(!(rockToggle.isOn || paperToggle.isOn || scissorsToggle.isOn || wellToggle.isOn));
    }
}
