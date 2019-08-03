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
            for (var j = i + 1; j < circlesOnStage.Length; j++)
            {
                if ((circlesOnStage[i].transform.position - circlesOnStage[j].transform.position).magnitude < circlesOnStage[i].Radius + circlesOnStage[j].Radius)
                {
                    if (circlesOnStage[i].OtherControllers.IndexOf(circlesOnStage[j]) == -1)
                    {
                        //claim as first collision
                        var surfacePerpendicular = Vector2.Perpendicular(circlesOnStage[i].transform.position - circlesOnStage[j].transform.position);
                        circlesOnStage[i].Direction = Vector2.Reflect(circlesOnStage[i].Direction, Vector2.Perpendicular(surfacePerpendicular).normalized).normalized;
                        circlesOnStage[j].Direction = Vector2.Reflect(circlesOnStage[j].Direction, Vector2.Perpendicular(surfacePerpendicular).normalized).normalized;

                        circlesOnStage[i].OtherControllers.Add(circlesOnStage[j]);
                        circlesOnStage[j].OtherControllers.Add(circlesOnStage[i]);

                        var health1 = circlesOnStage[i].Health;
                        var health2 = circlesOnStage[j].Health;

                        //always process enemy first
                        if (circlesOnStage[i].PlayerType == ObjectType.FRIEND)
                        {
                            circlesOnStage[i].DoImpact(circlesOnStage[j].PlayerType, health2);
                            circlesOnStage[j].DoImpact(circlesOnStage[i].PlayerType, health1);
                        }
                        else
                        {
                            circlesOnStage[j].DoImpact(circlesOnStage[i].PlayerType, health1);
                            circlesOnStage[i].DoImpact(circlesOnStage[j].PlayerType, health2);
                        }
                    }
                }
            }

            //emulate wall hit
            if (circlesOnStage[i].Direction.x > 0 && circlesOnStage[i].transform.position.x + circlesOnStage[i].Radius >= viewportHalfWidth - .25f)
            {
                circlesOnStage[i].Direction.x = -circlesOnStage[i].Direction.x;
            }
            if (circlesOnStage[i].Direction.x < 0 && circlesOnStage[i].transform.position.x - circlesOnStage[i].Radius <= -viewportHalfWidth + .25f)
            {
                circlesOnStage[i].Direction.x = -circlesOnStage[i].Direction.x;
            }
            if (circlesOnStage[i].Direction.y > 0 && circlesOnStage[i].transform.position.y + circlesOnStage[i].Radius >= 5f - .25f)
            {
                circlesOnStage[i].Direction.y = -circlesOnStage[i].Direction.y;
            }
            if (circlesOnStage[i].Direction.y < 0 && circlesOnStage[i].transform.position.y - circlesOnStage[i].Radius <= -5f + .25f)
            {
                circlesOnStage[i].Direction.y = -circlesOnStage[i].Direction.y;
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
            OnLevelLost();
        }
        else if (currentFriendsCount == circlesOnStage.Length && completedCirlesCount == currentFriendsCount)
        {
            OnLevelWin();
        }
    }
}
