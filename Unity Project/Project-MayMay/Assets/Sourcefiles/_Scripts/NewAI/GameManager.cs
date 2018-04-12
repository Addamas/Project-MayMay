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
