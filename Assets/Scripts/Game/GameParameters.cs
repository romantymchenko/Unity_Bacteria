using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameParameters : ScriptableObject
{
    public float FriendBactaSpeed = 0;
    public float HoleBactaSpeed = 0;
    public float Enemy1BactaSpeed = 0;
    public float Enemy2BactaSpeed = 0;
    public float Enemy3BactaSpeed = 0;

    public float minHPDiameter = 1f;
    public float maxHPDiameter = 5f;

    public float minHPValue = 1f;
    public float maxHPValue = 50f;

    public Color growingColor;
    public float growingSpeed;

    public Color friendFill;
    public Color holeFill;
    public Color enemy1Fill;
    public Color enemy2Fill;
    public Color enemy3Fill;

    public LevelConfig[] levels;
}

[Serializable]
public struct LevelConfig
{
    public GameObject prefab;
}
