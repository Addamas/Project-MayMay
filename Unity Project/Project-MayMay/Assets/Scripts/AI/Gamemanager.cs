using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;
using System.Linq;

public class Gamemanager : MonoBehaviour {

    public static Gamemanager instance;
    public static List<Interactable> publicInteractables = new List<Interactable>();
    public static List<Social> socialables = new List<Social>();
    public static Jai[] ais;

    public static int time; //in minutes
    [SerializeField]
    private float minuteLength = 1, timePerSentence;

    private void Awake()
    {
        instance = this;
        FindInteractables();
        ContinueFlow();
        EnableAI();
    }

    private void FindInteractables()
    {
        Interactable[] interactables = FindObjectsOfType(typeof(Interactable)) as Interactable[];
        foreach (Interactable interactable in interactables)
            if (!interactable.HasOwner)
                publicInteractables.Add(interactable);
    }

    private void EnableAI()
    {
        ais = FindObjectsOfType(typeof(Jai)) as Jai[];
        foreach (Jai ai in ais)
        {
            ai.Activate();
            foreach (Stat stat in ai.stats)
                if (stat as Social != null)
                {
                    socialables.Add(stat as Social);
                    break;
                }
        }

        foreach (Jai ai in ais)
            ai.LateActivate();
    }

    public void ContinueFlow()
    {
        pauseFlow = false;
        if (timeFlow == null)
            timeFlow = StartCoroutine(FlowTime());
    }

    public void PauseFlow()
    {
        pauseFlow = true;
    }

    private bool pauseFlow;
    private Coroutine timeFlow;
    private IEnumerator FlowTime()
    {
        while (true)
        {
            if (pauseFlow)
                yield return null;
            time++;
            if (time >= 1440)
                time = 0;
            yield return new WaitForSeconds(minuteLength);
        }
    }

    public static List<T> GetAllTInScene<T>()
    {
        return (FindObjectOfType(typeof(T)) as T[]).ToList();
    }
}
