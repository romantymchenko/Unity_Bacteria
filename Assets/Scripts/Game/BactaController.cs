using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BactaController : MonoBehaviour
{
    [SerializeField]
    private GameParameters gameParameters;

    [SerializeField]
    private BactaType playerType = BactaType.FRIEND;

    [SerializeField]
    private Transform circleTransform;
    private SpriteRenderer circleTransformRenderer;

    [SerializeField]
    private Transform circleOutlinerTransform;

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
        if (playerType != BactaType.FRIEND) return;

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
    }

    private void Start()
    {
        direction = Random.insideUnitCircle;
        UpdateView();
    }

    private void Update()
    {
        HandleMovement();

        if (isGrowing)
        {
            healthPoints += Time.deltaTime * gameParameters.growingSpeed;
            healthPoints = Mathf.Min(healthPoints, gameParameters.maxHPValue);
            UpdateView();
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

    private void UpdateView()
    {
        var hpCoeff = (healthPoints - gameParameters.minHPValue) / (gameParameters.maxHPValue - gameParameters.minHPValue);
        var currDiameter = gameParameters.minHPDiameter + (gameParameters.maxHPDiameter - gameParameters.minHPDiameter) * hpCoeff;

        circleTransform.localScale = Vector3.one * currDiameter;
        circleOutlinerTransform.localScale = circleTransform.localScale + Vector3.one * 0.1f;

        labelText.text = (int)healthPoints + "";
    }
}

public enum BactaType
{
    FRIEND,
    HOLE,
    ENEMY_SIMPLE,
    ENEMY_GROW,
    ENEMY_DOUBLE
}