using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour {

    public static Gamemanager instance;
    public static List<Interactable> publicInteractables = new List<Interactable>();
<<<<<<< HEAD
    public static List<Social> socialables = new List<Social>();
    public static Jai[] ais;

    public static int time; //in minutes
=======

    public int time; //in minutes
>>>>>>> 242e4cf73c44ac2fad1f9f47262ffc06f6ff1182
    [SerializeField]
    private float minuteLength = 1;

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
<<<<<<< HEAD
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
=======
        Jai[] ais = FindObjectsOfType(typeof(Jai)) as Jai[];
        foreach (Jai ai in ais)
            ai.Activate();
>>>>>>> 242e4cf73c44ac2fad1f9f47262ffc06f6ff1182
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
}
