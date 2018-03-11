using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public static TimeManager instance;
    public static int time;

    public Transform sunMoon;
    public float minuteDuration;

    private void Awake()
    {
        instance = this;
        StartFlow();
    }

    public void StartFlow()
    {
        StartCoroutine(Timeflow());
    }

    private IEnumerator Timeflow()
    {
        while (true)
        {
            sunMoon.Rotate(0, 0, 1);
            Debug.Log(time);
            yield return new WaitForSeconds(minuteDuration);
            time++;
        }
    }
}
