using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeManager))]
public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [SerializeField]
    private string seed;

    [NonSerialized]
    public static List<Character> characters = new List<Character>();
    [NonSerialized]
    public static List<Area> districts = new List<Area>();

    public static System.Random random;

    private void Awake()
    {
        instance = this;
        SetSeed();
    }

    private void SetSeed()
    {
        random = new System.Random(
            seed.Length == 0 ? DateTime.Now.GetHashCode() : seed.GetHashCode());
    }

    private void Start () {

        StartCoroutine(PathfindingQueue());
        StartCoroutine(Init());
	}

    private IEnumerator Init()
    {
        FindCharacters();
        foreach (Character character in characters)
        {
            character.Init();
            yield return null;
        }

        FindInteractables();

        TimeManager.instance.StartFlow();
        characters.ForEach(x => x.NewEvent());
    }

    private void FindCharacters()
    {
        Character[] characters = FindObjectsOfType(typeof(Character)) as Character[];
        foreach (Character chararacter in characters)
            GameManager.characters.Add(chararacter);
    }

    private void FindInteractables()
    {
        Interactable[] interactables = FindObjectsOfType(typeof(Interactable)) as Interactable[];
        foreach (Interactable interactable in interactables)
            interactable.Init();
    }

    #region Pathfinding Queue

    private static Queue<GHOPE> pathfindingQueue = new Queue<GHOPE>();

    public void EnqueuePathfinding(GHOPE ghope)
    {
        pathfindingQueue.Enqueue(ghope);
    }

    private IEnumerator PathfindingQueue()
    {
        GHOPE ghope;
        while (true)
        {
            while (pathfindingQueue.Count == 0)
                yield return null;

            ghope = pathfindingQueue.Dequeue();
            yield return StartCoroutine(ghope.Pathfinding());
        }
    }

    #endregion
}
