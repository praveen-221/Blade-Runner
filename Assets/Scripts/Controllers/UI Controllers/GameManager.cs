using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public bool gameStartedFromMainMenu, gameRestartPlayerDied;
    [HideInInspector]
    public float score, health, level;
    [HideInInspector]
    public bool canPlayMusic = true;

    // Start is called before the first frame update
    void Start()
    {
        MakeSingleton();
    }

    // Make a single instance of the class to pass to different scenes
    // used instead of storing data redundantly
    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}