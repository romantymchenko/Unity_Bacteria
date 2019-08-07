using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject levelSelection;

    public void OnNewGameClick()
    {
        gameObject.SetActive(false);
        levelSelection.SetActive(true);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    private void Awake()
    {
        gameObject.SetActive(true);
        levelSelection.SetActive(false);

        if (PlayerProfile.Instance.LevelsPassed > 0)
        {
            //enable "continue" options

        }
    }
}
