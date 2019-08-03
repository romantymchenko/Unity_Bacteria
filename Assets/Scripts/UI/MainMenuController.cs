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
        //gameObject.SetActive(false);
        //levelSelection.SetActive(true);

        SceneManager.LoadScene("Game");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
