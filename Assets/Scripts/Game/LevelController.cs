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

    [SerializeField]
    private List<Collision2D> frameCollisions = new List<Collision2D>();

    public void RegisterCollision(Collision2D collision)
    {
        frameCollisions.Add(collision);
    }

    private void OnEnable()
    {
        circlesOnStage = GetComponentsInChildren<BactaController>();
        for (var i = 0; i < circlesOnStage.Length; i++)
        {
            circlesOnStage[i].LevelController = this;
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
        if (frameCollisions.Count > 0)
        {
            ProcessCollisions();
        }

        DoCalculations();
        frameCollisions.Clear();
    }

    private void ProcessCollisions()
    {
        while (frameCollisions.Count > 0)
        {
            var current = frameCollisions[0];
            frameCollisions.RemoveAt(0);

            //clean duplicate
            for (var i = frameCollisions.Count - 1; i >= 0; i--)
            {
                if (frameCollisions[i].collider == current.otherCollider && frameCollisions[i].otherCollider == current.collider)
                {
                    frameCollisions.RemoveAt(i);
                }
            }

            //process collision
            var controller1 = current.collider.transform.parent.GetComponent<BactaController>();
            var controller2 = current.otherCollider.transform.parent.GetComponent<BactaController>();

            var health1 = controller1.Health;
            var health2 = controller2.Health;

            //always process enemy first
            if (controller1.PlayerType == ObjectType.FRIEND)
            {
                controller1.DoImpact(controller2.PlayerType, health2);
                controller2.DoImpact(controller1.PlayerType, health1);
            }
            else
            {
                controller2.DoImpact(controller1.PlayerType, health1);
                controller1.DoImpact(controller2.PlayerType, health2);
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
