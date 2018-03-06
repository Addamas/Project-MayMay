using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [NonSerialized]
    public static List<Character> characters = new List<Character>();
    [NonSerialized]
    public static List<Area> districts = new List<Area>();

    private void Awake()
    {
        instance = this;
    }

    private void Start () {

        StartCoroutine(Init());
	}

    private IEnumerator Init()
    {
        FindCharacters();
        foreach (Character character in characters)
        {
            character.memory.Init();
            yield return null;
        }

        TimeManager.instance.StartFlow();
        characters.ForEach(x => x.NewEvent());
    }

    private void FindCharacters()
    {
        Character[] characters = FindObjectsOfType(typeof(Character)) as Character[];
        foreach (Character chararacter in characters)
            GameManager.characters.Add(chararacter);
    }
}
