using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

public class Area : MonoBehaviour {

    public enum AreaType {Default, District, Street, House, Room }
    
    public AreaType Type
    {
        get
        {
            try
            {
                return (AreaType)ParentCount;
            }
            catch
            {
                return AreaType.Default;
            }
        }
    }

    private int ParentCount
    {
        get
        {
            int ret = 1;
            Transform trans = transform;
            while (trans.parent != null)
                if (trans.parent.GetComponent<Area>() != null)
                {
                    trans = trans.parent;
                    ret++;
                }
            return ret;
        }
    }

    private void Awake()
    {
        if(Type == AreaType.District)
            GameManager.districts.Add(this);
    }

    public Area GetClosestInDistrict(Vector3 pos)
    {
        if (transform.childCount == 0)
            return this;
        Area[] ret = GetComponentsInChildren<Area>();
        ret.SortByClosest(pos);
        return ret.First();
    }
}
