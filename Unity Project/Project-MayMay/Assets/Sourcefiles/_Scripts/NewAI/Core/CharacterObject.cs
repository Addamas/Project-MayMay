using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterObject", menuName = "Settings/Character", order = 1)]
public class CharacterObject : ScriptableObject
{
    public int critVal = 10;
    public int movementFramesUntilNewCheck = 500;
    public float interactDistance;
}
