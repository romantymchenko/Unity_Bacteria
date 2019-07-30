using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BactaController : MonoBehaviour
{
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
    private Vector2 direction;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float healthPoints;

    [SerializeField]
    private bool isGrowing;

    private Rigidbody2D rigidbody2D;

    public void StartTouchingPhase()
    {
        if (playerType != ObjectType.FRIEND) return;

        StartGrow();
    }

    public void StopTouchingPhase()
	{
        StopGrowing();
    }

    private void Awake()
    {
        circleTransformRenderer = circleTransform.GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        ChangeSkin(playerType);

    }

    private void Start()
    {
        direction = Random.insideUnitCircle.normalized;
        UpdateSize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherType = collision.transform.parent.GetComponent<ObjectTypeStorage>().objectType;
        if (otherType == ObjectType.WALL)
        {
            direction = Vector2.Reflect(direction, collision.transform.right);
            return;
        }

        //get reflection using closest point
        direction = Vector2.Reflect(direction, (new Vector2(transform.position.x, transform.position.y) - collision.ClosestPoint(transform.position)).normalized);

        if (playerType == otherType) return;

        if (playerType == ObjectType.FRIEND)
        {
            switch(otherType)
            {
                case ObjectType.HOLE:
                    if (isGrowing)
                    {
                        //friend(curr)->hole
                        StopGrowing();
                        ChangeSkin(ObjectType.HOLE);
                    }
                    break;
                case ObjectType.ENEMY_SIMPLE:
                case ObjectType.ENEMY_GROW:

                    break;
                case ObjectType.ENEMY_DOUBLE:

                    break;
            }
        }
    }

    private void Update()
    {
        HandleMovement();

        if (isGrowing)
        {
            healthPoints += Time.deltaTime * gameParameters.growingSpeed;
            healthPoints = Mathf.Min(healthPoints, gameParameters.maxHPValue);
            UpdateSize();
        }
    }

    private void HandleMovement()
    {
        var nextPosition = direction * Time.deltaTime * speed;
        nextPosition.x += transform.position.x;
        nextPosition.y += transform.position.y;
        rigidbody2D.MovePosition(nextPosition);
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

        labelText.text = (int)healthPoints + "";
    }

    private void ChangeSkin(ObjectType objectType)
    {
        playerType = objectType;
        GetComponent<ObjectTypeStorage>().objectType = objectType;

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
