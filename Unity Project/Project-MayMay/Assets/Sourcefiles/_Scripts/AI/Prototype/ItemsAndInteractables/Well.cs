using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : Interactable {

    [SerializeField]
    private Water water;

	public Water GetWater()
    {
        return water;
    }
}
