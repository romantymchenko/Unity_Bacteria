using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel;

    public void OnBackButtonCLick()
    {
        gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
