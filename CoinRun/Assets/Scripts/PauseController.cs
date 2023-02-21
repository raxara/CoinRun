using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    public static PauseController INSTANCE;

    public Transform parent;

    private void Awake()
    {
        INSTANCE = this;
    }

    private void Start()
    {
        Display(false);
    }

    public void OnResumeClick()
    {
        LevelManager.INSTANCE.ResumeGame();
    }

    public void OnMainMenuClick()
    {
        LevelManager.INSTANCE.LoadMainMenu();
    }

    public void Display(bool display)
    {
        if (display)
        {
            LevelManager.openMenu = this.gameObject;
        }
        else
        {
            if (LevelManager.openMenu == this.gameObject)
            {
                LevelManager.openMenu = null;
            }
        }
        parent.gameObject.SetActive(display);
    }

    private void Update()
    {
        if (!LevelManager.gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LevelManager.INSTANCE.PauseGame();
            }
        }
    }
}
