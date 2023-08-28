using System;
using UnityEngine;

/// <summary>
/// Create a scriptable enemy
/// </summary>
[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class ScriptableEnemy : ScriptableUnitBase
{
    public RuntimeAnimatorController animator;
    public ResistanceType resistanceType;
    public int propertyValue;

}

[Serializable]
public enum ResistanceType
{
    Sugar = 0,
    Fat = 1
}

