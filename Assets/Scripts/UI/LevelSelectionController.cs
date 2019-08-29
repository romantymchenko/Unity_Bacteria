using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private Button[] lvlButtons;

    public void OnBackButtonCLick()
    {
        gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnLevelSelect(int levelNumber)
    {
        PlayerProfile.Instance.currentLevel = levelNumber;
        SceneManager.LoadScene("Game");
    }
}
