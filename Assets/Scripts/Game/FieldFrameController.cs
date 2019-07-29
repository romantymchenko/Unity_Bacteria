using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldFrameController : MonoBehaviour
{
    [SerializeField]
    private Transform left;

    [SerializeField]
    private Transform right;

    [SerializeField]
    private Transform top;

    [SerializeField]
    private Transform bottom;


    private void Awake()
    {
        var aspect = (float)Screen.width / Screen.height;
        var viewportHalfWidth = 5 * aspect;

        left.localPosition = Vector3.left * viewportHalfWidth;
        left.localScale = Vector3.one * 0.5f + Vector3.up * 9.5f;

        right.localPosition = Vector3.right * viewportHalfWidth;
        right.localScale = left.localScale;

        top.localPosition = Vector3.up * 5;
        top.localScale = Vector3.one * 0.5f + Vector3.right * (viewportHalfWidth * 2 - 0.5f);

        bottom.localPosition = Vector3.down * 5;
        bottom.localScale = top.localScale;
    }
}
