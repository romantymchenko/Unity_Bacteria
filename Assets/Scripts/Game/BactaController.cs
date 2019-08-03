using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BactaController : MonoBehaviour
{
	public ObjectType PlayerType => playerType;

    public int Health => (int)healthPoints;

    public float Radius => circleOutlinerTransform.localScale.x / 2;

    public Vector2 Direction;

    public float StartAngle;

    [SerializeField]
    public List<BactaController> OtherControllers = new List<BactaController>();

    [SerializeField]
    private GameParameters gameParameters;

    [SerializeField]
    private ObjectType playerType = ObjectType.FRIEND;

    [SerializeField]
    private Transform circleTransform;
    private SpriteRenderer circleTransformRenderer;

    [SerializeField]
    private Transform circleOutlinerTransform;

    [SerializeField]
    private Transform circleOutlinerMaskTransform;

    [SerializeField]
    private TextMesh labelText;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float healthPoints;

    [SerializeField]
    private bool isGrowing;

    private Rigidbody2D core;

    public void StartTouchingPhase()
    {
        if (playerType != ObjectType.FRIEND) return;

        StartGrow();
    }

    public void StopTouchingPhase()
	{
        StopGrowing();
    }

    public void DoImpact(ObjectType otherType, int otherHealth)
    {
        if (playerType == otherType) return;
        if (playerType == ObjectType.HOLE) return;

        if (playerType == ObjectType.FRIEND)
        {
            switch (otherType)
            {
                case ObjectType.HOLE:
                    if (isGrowing)
                    {
                        //friend(curr)->hole
                        StopGrowing();
                        ChangeSkin(otherType);
                    }
                    break;
                case ObjectType.ENEMY_SIMPLE:
                case ObjectType.ENEMY_GROW:
                    if (isGrowing)
                    {
                        //friend(curr)->hole
                        StopGrowing();
                        ChangeSkin(otherType);
                    }
                    else
                    {
                        healthPoints -= otherHealth / 3f;
                        if (Health <= 0)
                        {
                            healthPoints = 10;
                            ChangeSkin(otherType);
                        }
                    }
                    break;
                case ObjectType.ENEMY_DOUBLE:
                    if (isGrowing)
                    {
                        //friend(curr)->hole
                        StopGrowing();
                        ChangeSkin(otherType);
                    }
                    else
                    {
                        healthPoints -= otherHealth / 3f * 2f;
                        if (Health <= 0)
                        {
                            healthPoints = 10;
                            ChangeSkin(otherType);
                        }
                    }
                    break;
            }
        }
        else if (otherType == ObjectType.FRIEND)
        {
            //receive damage
            healthPoints -= otherHealth / 4f;
            if (Health <= 0)
            {
                healthPoints = 10;
                ChangeSkin(otherType);
            }
        }

        UpdateSize();
    }

    private void Awake()
    {
        circleTransformRenderer = circleTransform.GetComponent<SpriteRenderer>();
        core = GetComponent<Rigidbody2D>();
        ChangeSkin(playerType);

        var angleInRad = Mathf.Deg2Rad * StartAngle;
        Direction = new Vector2(Mathf.Cos(angleInRad), -Mathf.Sin(angleInRad));
        UpdateSize();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OtherControllers.Remove(collision.transform.parent.GetComponent<BactaController>());
    }

    private void Update()
    {
        if (isGrowing)
        {
            healthPoints += Time.deltaTime * gameParameters.growingSpeed;
            healthPoints = Mathf.Min(healthPoints, gameParameters.maxHPValue);
            UpdateSize();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        var nextPosition = Direction * Time.deltaTime * speed;
        nextPosition.x += transform.position.x;
        nextPosition.y += transform.position.y;
        core.MovePosition(nextPosition);
    }

    private void StartGrow()
    {
        if (isGrowing) return;

        isGrowing = true;
        circleTransformRenderer.color = gameParameters.growingColor;
    }

    private void StopGrowing()
    {
        if (!isGrowing) return;

        isGrowing = false;
        circleTransformRenderer.color = Color.white;
    }

    private void UpdateSize()
    {
        var hpCoeff = (healthPoints - gameParameters.minHPValue) / (gameParameters.maxHPValue - gameParameters.minHPValue);
        var currDiameter = gameParameters.minHPDiameter + (gameParameters.maxHPDiameter - gameParameters.minHPDiameter) * hpCoeff;

        circleTransform.localScale = Vector3.one * currDiameter;
        circleOutlinerMaskTransform.localScale = circleTransform.localScale;
        circleOutlinerTransform.localScale = circleTransform.localScale + Vector3.one * 0.1f;

        labelText.text = Health + "";
    }

    private void ChangeSkin(ObjectType objectType)
    {
        playerType = objectType;

        switch(objectType)
        {
            case ObjectType.FRIEND:
                circleTransformRenderer.color = gameParameters.friendFill;
                break;
            case ObjectType.HOLE:
                circleTransformRenderer.color = gameParameters.holeFill;
                break;
            case ObjectType.ENEMY_SIMPLE:
                circleTransformRenderer.color = gameParameters.enemy1Fill;
                break;
            case ObjectType.ENEMY_GROW:
                circleTransformRenderer.color = gameParameters.enemy2Fill;
                break;
            case ObjectType.ENEMY_DOUBLE:
                circleTransformRenderer.color = gameParameters.enemy3Fill;
                break;
        }
    }
}
