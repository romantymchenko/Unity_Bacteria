using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levelPrefabs;

    [SerializeField]
    private GameObject activeLevel;

    [SerializeField]
    private GrowRaycastController raycastController;

    [SerializeField]
    private GameObject introPopup;

    [SerializeField]
    private GameObject winPopup;

    [SerializeField]
    private GameObject lostPopup;

    [SerializeField]
    private Text levelNumberText;

    [SerializeField]
    private GamePhase phase;

    public void OnBackToMainMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Awake()
    {
        ChangePhase(GamePhase.INTRO);
    }

    private void Update()
    {
        if (phase == GamePhase.INTRO)
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                ChangePhase(GamePhase.GAME);
            }
        }
    }

    private void ChangePhase(GamePhase newPhase)
    {
        switch(newPhase)
        {
            case GamePhase.INTRO:
                //setup into UI
                introPopup.SetActive(true);
                levelNumberText.text = string.Format("Level {0}", 1);
                winPopup.SetActive(false);
                lostPopup.SetActive(false);
                //setup interaction
                raycastController.enabled = false;
                //instantiate level prefab
                activeLevel = Instantiate(levelPrefabs[0]);
                //link to level-c callbacks
                var c = activeLevel.GetComponent<LevelController>();
                c.enabled = false;
                c.OnLevelWin += OnWin;
                c.OnLevelLost += OnLost;
                break;
            case GamePhase.GAME:
                introPopup.SetActive(false);
                raycastController.enabled = true;
                activeLevel.GetComponent<LevelController>().enabled = true;
                break;
            case GamePhase.WIN:
                //stop level
                raycastController.enabled = false;
                activeLevel.GetComponent<LevelController>().enabled = false;
                //setup UI
                winPopup.SetActive(true);
                break;
            case GamePhase.LOST:
                //stop level
                raycastController.enabled = false;
                activeLevel.GetComponent<LevelController>().enabled = false;
                //setup UI
                lostPopup.SetActive(true);
                break;
            case GamePhase.DISPOSE:

                break;
        }

        phase = newPhase;
    }

    private void OnWin()
    {
        ChangePhase(GamePhase.WIN);
    }

    private void OnLost()
    {
        ChangePhase(GamePhase.LOST);
    }
}

public enum GamePhase
{
    INTRO = 0,
    GAME,
    WIN,
    LOST,
    DISPOSE
}
