using Jext;
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

    public static List<Item> publicItems = new List<Item>();

    public static System.Random random;

    public static Player Player
    {
        get
        {
            return instance.player;
        }
    }
    [SerializeField]
    private Player player;

    public Camera mainCamera;

    private void Awake()
    {
        instance = this;
        SetSeed();
    }

    private void SetSeed()
    {
        int hashCode = DateTime.Now.GetHashCode();
        random = new System.Random(
            seed.Length == 0 ?  hashCode: seed.GetHashCode());
        if (seed.Length == 0)
            seed = hashCode.ToString();
    }

    private void Start () {

        StartCoroutine(PathfindingQueue());
        StartCoroutine(MovementQueue());
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
        
        FindOwnables();

        TimeManager.instance.StartFlow();
        characters.ForEach(x => x.NewEvent());

        StartCoroutine(SensesQueue());
    }

    private void FindCharacters()
    {
        Character[] characters = FindObjectsOfType(typeof(Character)) as Character[];
        foreach (Character chararacter in characters)
            GameManager.characters.Add(chararacter);
    }

    private void FindOwnables()
    {
        Ownable[] ownables = FindObjectsOfType(typeof(Ownable)) as Ownable[];
        foreach (Ownable ownable in ownables)
            ownable.Init();
    }

    #region Pathfinding Queue
    private static List<GHOPE> pathfindingQueue = new List<GHOPE>();

    public static void EnqueuePathfinding(GHOPE ghope)
    {
        pathfindingQueue.Add(ghope);
    }

    public static void TryRemoveFromPathfindingQueue(GHOPE removable)
    {
        pathfindingQueue.RemoveAll(x => x == removable);
    }

    private IEnumerator PathfindingQueue()
    {
        GHOPE ghope;
        while (true)
        {
            while (pathfindingQueue.Count == 0)
                yield return null;

            ghope = pathfindingQueue.First();
            pathfindingQueue.RemoveAt(0);
            yield return StartCoroutine(ghope.Pathfinding());
        }
    }

    public static Character GetCharacter(string name)
    {
        foreach (Character character in characters)
            if (character.name == name)
                return character;
        return null;
    }
    #endregion

    #region Movement Queue
    private static List<Character> movementQueue = new List<Character>();

    public static void EnqueueMovement(Character ghope)
    {
        movementQueue.Add(ghope);
    }

    public static void TryRemoveFromMovementQueue(Character removable)
    {
        movementQueue.RemoveAll(x => x == removable);
    }

    private IEnumerator MovementQueue()
    {
        Character ghope;
        Action curAction;

        while (true)
        {
            while (movementQueue.Count == 0)
                yield return null;
            
            ghope = movementQueue.First();
            movementQueue.RemoveAt(0);

            curAction = ghope.curAction;

            if (curAction == null)
            {
                if (ghope.debug)
                    Debug.Log("Cancel " + ghope.name);
                ghope.Cancel();
                ghope.NewEvent();
                continue;
            }

            if (curAction.IsExecutable())
                if (!curAction.InRange())
                {
                    ghope.movement.MoveTo(curAction.Pos());
                    //add it to the top again
                    movementQueue.Add(ghope);
                }
                else
                {
                    if (!curAction.autoMovement)
                        ghope.StopMovement();
                    if(ghope.debug)
                        Debug.Log("Execute " + ghope.name);
                    ghope.BaseExecute();
                }
            else
            {
                if (ghope.debug)
                    Debug.Log("Cancel " + ghope.name);
                ghope.Cancel();
                ghope.NewEvent();
            }
            
            yield return null;
        }
    }
    #endregion

    #region Queue

    private IEnumerator SensesQueue()
    {
        int current = 0;
        Character character;

        while (true)
        {
            character = characters[current];
            character.senses.CheckSurrounding();

            current++;
            if (current >= characters.Count)
                current = 0;
            yield return null;
        }
    }

    #endregion
}
namespace ProjectShortcuts
{
    public static class Shortcuts
    {
        public static T GetClosest<T>(ref List<T> sortableList, Vector3 point) where T : Extension
        {
            T ret = sortableList.First();
            float dis = Vector3.Distance(ret.ai.Pos, point), dis2;
            for (int i = 1; i < sortableList.Count; i++)
            {
                dis2 = Vector3.Distance(ret.ai.Pos, sortableList[i].ai.Pos);
                if (dis2 > dis)
                    continue;
                ret = sortableList[i];
                dis = dis2;
            }
            return ret;
        }

        public static Memory.Other GetClosest(ref List<Memory.Other> sortableList, Vector3 point)
        {
            Memory.Other ret = sortableList.First();
            float dis = Vector3.Distance(ret.character.Pos, point), dis2;
            for (int i = 1; i < sortableList.Count; i++)
            {
                dis2 = Vector3.Distance(ret.character.Pos, sortableList[i].character.Pos);
                if (dis2 > dis)
                    continue;
                ret = sortableList[i];
                dis = dis2;
            }
            return ret;
        }
    }
}
