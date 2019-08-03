using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BactaController))]
public class BactaControllerHelper : Editor
{
    private void OnSceneGUI()
    {
        BactaController bactaController = target as BactaController;
        var angle = Mathf.Deg2Rad * bactaController.StartAngle;

        Handles.DrawLine(
            bactaController.transform.position,
            bactaController.transform.position + new Vector3(Mathf.Cos(angle), -Mathf.Sin(angle), 0) * 3f
        );
    }
}
