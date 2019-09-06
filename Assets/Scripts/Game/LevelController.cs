using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public event System.Action OnLevelWin;
    public event System.Action OnLevelLost;

    [SerializeField]
    private GameParameters gameParameters;

    [SerializeField]
    private BactaController[] circlesOnStage;

    private void OnEnable()
    {
        circlesOnStage = GetComponentsInChildren<BactaController>();
        for (var i = 0; i < circlesOnStage.Length; i++)
        {
            circlesOnStage[i].OnTypeChanged += DoCalculations;
            circlesOnStage[i].enabled = true;
        }
    }

    private void OnDisable()
    {
        circlesOnStage = GetComponentsInChildren<BactaController>();
        for (var i = 0; i < circlesOnStage.Length; i++)
        {
            circlesOnStage[i].OnTypeChanged -= DoCalculations;
            circlesOnStage[i].enabled = false;
        }
    }

    private void DoCalculations()
    {
        var completedCirlesCount = 0;
        var currentFriendsCount = 0;
        var currentWholesCount = 0;
        for (var i = 0; i < circlesOnStage.Length; i++)
        {
            if (circlesOnStage[i].PlayerType == ObjectType.FRIEND)
            {
                currentFriendsCount++;
                if (circlesOnStage[i].Health == (int)gameParameters.maxHPValue)
                {
                    completedCirlesCount++;
                }
            }
            else if (circlesOnStage[i].PlayerType == ObjectType.HOLE)
            {
                currentWholesCount++;
            }
        }

        //check lost first
        if (currentFriendsCount == 0)
        {
            OnLevelLost();
        }
        else if (circlesOnStage.Length - currentWholesCount == completedCirlesCount)
        {
            for (var i = 0; i < circlesOnStage.Length; i++)
            {
                circlesOnStage[i].OnPointerExit(null);
            }
            OnLevelWin();
        }
    }
}
