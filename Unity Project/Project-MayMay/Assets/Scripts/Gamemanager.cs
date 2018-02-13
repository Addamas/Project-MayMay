using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour {

    public static Gamemanager instance;
    public static List<Interactable> publicInteractables = new List<Interactable>();

    public int time; //in minutes
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
        Jai[] ais = FindObjectsOfType(typeof(Jai)) as Jai[];
        foreach (Jai ai in ais)
            ai.Activate();
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
