using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [NonSerialized]
    public static List<Character> characters = new List<Character>();

    private void Awake()
    {
        instance = this;
    }

    private void Start () {
        FindCharacters();
        characters.ForEach(x => x.NewEvent());
	}

    private void FindCharacters()
    {
        Character[] characters = FindObjectsOfType(typeof(Character)) as Character[];
        foreach (Character chararacter in characters)
            GameManager.characters.Add(chararacter);
    }
}
