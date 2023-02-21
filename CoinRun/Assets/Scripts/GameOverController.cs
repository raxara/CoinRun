using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{

    public static GameOverController INSTANCE;

    public Transform parent;

    private void Awake()
    {
        INSTANCE = this;
    }

    private void Start()
    {
        Display(false);
    }

    public void onReplayClick()
    {
        LevelManager.INSTANCE.LoadGame();
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
}
