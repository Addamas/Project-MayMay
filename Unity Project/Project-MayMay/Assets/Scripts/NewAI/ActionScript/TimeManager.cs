using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public static TimeManager instance;
    public static int time;

    public float minuteDuration;

    private void Awake()
    {
        instance = this;       
    }

    public void StartFlow()
    {
        StartCoroutine(Timeflow());
    }

    private IEnumerator Timeflow()
    {
        while (true)
        {
            yield return new WaitForSeconds(minuteDuration);
            time++;
        }
    }
}
