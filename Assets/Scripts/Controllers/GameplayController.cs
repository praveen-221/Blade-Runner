using System.Collections;
using System.Collections.Generic;
using TMPro;        // for UI Text elements 
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;       // for using unity UI elements like buttons etc...

public class GameplayController : MonoBehaviour
{
    public static GameplayController gamePlayInstance;

    [HideInInspector]
    public bool canAddScore;
    private float score, health = 3, level;

    [SerializeField]
    private AudioSource audioSrc;
    private BGScroller bgScroller;
    private TextMeshProUGUI scoreText, healthText, levelText;
    private GameObject pausePanel;

    void Awake()
    {
        MakeInstance();

        scoreText = GameObject.Find(Tags.SCORE_TEXT).GetComponent<TextMeshProUGUI>();
        healthText = GameObject.Find(Tags.HEALTH_TEXT).GetComponent<TextMeshProUGUI>();
        levelText = GameObject.Find(Tags.LEVEL_TEXT).GetComponent<TextMeshProUGUI>();

        bgScroller = GameObject.Find(Tags.BG_GAME_OBJ).GetComponent<BGScroller>();
        pausePanel = GameObject.Find(Tags.PAUSE_PANEL);
        pausePanel.SetActive(false);
    }

    void Start()
    {
        if(GameManager.instance.canPlayMusic)
        {
            audioSrc.Play();
        }
        // StartCoroutine(IncreaseScore(1));
    }

    void FixedUpdate()
    {
        // IncrementScore(0.1f);
    }

    void OnEnable ()
    {
        // Scubscribing to the event of sceneLoded given by the SceneManager delegate
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        gamePlayInstance = null;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == Tags.GAMEPLAY_SCENE)
        {
            if(GameManager.instance.gameStartedFromMainMenu)
            {
                GameManager.instance.gameStartedFromMainMenu = false;
                score = 0;
                health = 3;
                // level = 1;
            } else if(GameManager.instance.gameRestartPlayerDied)
            {
                GameManager.instance.gameRestartPlayerDied = false;
                score = GameManager.instance.score;
                health = GameManager.instance.health;
                // level = GameManager.instance.level;
            }
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
            if (healthText != null)
            {
                healthText.text = health.ToString();
            }
            /*if (levelText != null)
                levelText.text = level.ToString();*/
        }
    }

    void MakeInstance()
    {
        if(gamePlayInstance == null)
        {
            gamePlayInstance = this;
        }
    }

    public void TakeDamage()
    {
        health--;
        if(health >= 0)
        {
            // restart the game
            StartCoroutine(PlayerDied(Tags.GAMEPLAY_SCENE));
            if(healthText != null)
            {
                healthText.text = health.ToString();
            } 
        } else
        {
            StartCoroutine(PlayerDied(Tags.MAINMENU_SCENE));
        }
    }

    public void IncrementHealth()
    {
        health++;
        if(healthText != null)
        {
            healthText.text = health.ToString();
        }
    }

    public void IncrementScore(float scoreValue) 
    {
        if (canAddScore)
        {
            score += scoreValue;
            if (scoreText != null)
            {
                // Debug.Log("Scoretext value: " + scoreText.text + " & Score: " + score);
                scoreText.text = score.ToString();
            }
        }
        // StartCoroutine(IncreaseScore(scoreValue));
    }

    /*IEnumerator IncreaseScore(float scoreValue)
    {
        yield return new WaitForSeconds(1f);
        if(canStartScore)
        {
            score += scoreValue;
            if(scoreText != null)
            {
                Debug.Log("Scoretext value: " + scoreText.text + " & Score: " + score);
                scoreText.text = score.ToString();
            }
        }
        StartCoroutine(IncreaseScore(1));
    }*/

    IEnumerator PlayerDied(string sceneName)
    {
        canAddScore = false;
        bgScroller.canScroll = false;

        GameManager.instance.score = score;
        GameManager.instance.health = health;
        GameManager.instance.gameRestartPlayerDied = true;

        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        canAddScore = false;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;        // pause everything or freezes the time
    }

    public void ResumeGame()
    {
        canAddScore = true;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void HomeBtn()
    {
        // since time freezed during pause we need to revert back or use Unscaled Time in Animation Inspector in Main Menu scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(Tags.MAINMENU_SCENE);
    }

    public void ReplayBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Tags.GAMEPLAY_SCENE);
    }
}
