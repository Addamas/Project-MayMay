using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "Dinner", menuName = "Action/Dinner", order = 1)]
public class Dinner : SimpleQuest {

    [SerializeField]
    private bool searchForMissing;

    public override Vector3 Pos()
    {
        List<Vector3> dinnerables = new List<Vector3>();
        if (searchForMissing)
        {
            try
            {
                //later een search functie voor karakters, memory all that
                List<Character.Other> others = ai.Social.GetAssociates();
                foreach (Character.Other other in others)
                    if (other.dinnerable && other.character.curAction as Dinner == null)
                        dinnerables.Add(other.Pos);
                dinnerables = dinnerables.SortByClosest(ai.transform.position);
                return dinnerables[0];
            }
            catch { }
        }

        List<Table> tables = GetInteractableX.SPGetAllX<Table>(ai);
        if(tables.Count == 0)
            tables = GetInteractableX.SGetAllX<Table>(ai);
        return tables.SortByClosest(ai.transform.position)[0].transform.position;
    }
}
