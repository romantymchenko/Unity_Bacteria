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

    private float viewportHalfWidth;

    private void Awake()
    {
        var aspect = (float)Screen.width / Screen.height;
        viewportHalfWidth = 5 * aspect;

    }

    private void OnEnable()
    {
        circlesOnStage = GetComponentsInChildren<BactaController>();
        for (var i = 0; i < circlesOnStage.Length; i++)
        {
            circlesOnStage[i].enabled = true;
        }
    }

    private void OnDisable()
    {
        circlesOnStage = GetComponentsInChildren<BactaController>();
        for (var i = 0; i < circlesOnStage.Length; i++)
        {
            circlesOnStage[i].enabled = false;
        }
    }

    private void FixedUpdate()
    {
        HandleWallHit();
        DoCalculations();
    }

    private void HandleWallHit()
    {
        for (var i = 0; i < circlesOnStage.Length; i++)
        {
            //emulate wall hit
            if (circlesOnStage[i].NextFrameDirection.x > 0 && circlesOnStage[i].transform.position.x + circlesOnStage[i].Radius >= viewportHalfWidth - .25f)
            {
                circlesOnStage[i].NextFrameDirection.x = -circlesOnStage[i].NextFrameDirection.x;
            }
            if (circlesOnStage[i].NextFrameDirection.x < 0 && circlesOnStage[i].transform.position.x - circlesOnStage[i].Radius <= -viewportHalfWidth + .25f)
            {
                circlesOnStage[i].NextFrameDirection.x = -circlesOnStage[i].NextFrameDirection.x;
            }
            if (circlesOnStage[i].NextFrameDirection.y > 0 && circlesOnStage[i].transform.position.y + circlesOnStage[i].Radius >= 5f - .25f)
            {
                circlesOnStage[i].NextFrameDirection.y = -circlesOnStage[i].NextFrameDirection.y;
            }
            if (circlesOnStage[i].NextFrameDirection.y < 0 && circlesOnStage[i].transform.position.y - circlesOnStage[i].Radius <= -5f + .25f)
            {
                circlesOnStage[i].NextFrameDirection.y = -circlesOnStage[i].NextFrameDirection.y;
            }
        }
    }

    private void DoCalculations()
    {
        //check stage (we do this in every cycle cause participans count is very low)
        var completedCirlesCount = 0;
        var currentFriendsCount = 0;
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
        }

        //check lost first
        if (currentFriendsCount == 0)
        {
            //OnLevelLost();
        }
        else if (currentFriendsCount == circlesOnStage.Length && completedCirlesCount == currentFriendsCount)
        {
            OnLevelWin();
        }
    }
}
