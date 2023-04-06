using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//singleton manager des niveaux et des menus de pause et de game over
public class LevelManager : MonoBehaviour
{

    private static LevelManager _INSTANCE;

    public static LevelManager INSTANCE
    {
        get
        {
            if (!exists)
            {
                createSelf();
            }
            return _INSTANCE;
        }
    }

    //ajoutée mais pas utilisée
    private bool inGame { get { return openMenu == null; } }

    public static GameObject openMenu;

    public static bool gameIsPaused { get { return Time.timeScale == 0; } }

    public static bool exists { get { return _INSTANCE != null; } }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (exists) return;
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
        }
        else if (_INSTANCE != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    static void createSelf()
    {
        GameObject g = new GameObject("LevelManager");
        g.AddComponent<LevelManager>().Init();
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void PauseGame()
    {
        PauseController.INSTANCE.Display(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        PauseController.INSTANCE.Display(false);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        GameOverController.INSTANCE.Display(true);
        Time.timeScale = 0;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}