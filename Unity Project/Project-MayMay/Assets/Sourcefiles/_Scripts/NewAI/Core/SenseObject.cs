using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SenseObject", menuName = "Settings/Senses", order = 1)]
public class SenseObject : ScriptableObject {

    public float frequency, spotDistance;
}
