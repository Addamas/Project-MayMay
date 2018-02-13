using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

public class GetXType : SimpleNormalAction
{
    public override void Cancel()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {

    }

    public override Vector3 Pos()
    {
        return GetX<MonoBehaviour>().transform.position;
    }

    protected virtual T GetX<T>() where T : MonoBehaviour
    {
        return null;
    }
}

public class GetItemX : GetXType
{
    protected List<T> GetAllX<T>() where T : Item
    {
        T[] xs = FindObjectsOfType(typeof(T)) as T[];
        List<T> ret = new List<T>();
        foreach (T t in xs)
            ret.Add(t);
        ret.AddList(ai.ownedItems.GetTypeFromListAsU<Item, T>(), false);
        return ret;
    }

    public static List<T> SGetAllX<T>(Character character) where T : Item
    {
        T[] xs = FindObjectsOfType(typeof(T)) as T[];
        List<T> ret = new List<T>();
        foreach (T t in xs)
            ret.Add(t);
        ret.AddList(character.ownedItems.GetTypeFromListAsU<Item, T>(), false);
        return ret;
    }

    public static List<T> SPGetAllX<T>(Character character) where T : Item
    {
        return character.ownedItems.GetTypeFromListAsU<Item, T>();
    }
}

public class GetInteractableX : GetXType
{
    protected List<T> GetAllX<T>() where T : Interactable
    {
        T[] xs = FindObjectsOfType(typeof(T)) as T[];
        List<T> ret = new List<T>();
        foreach (T t in xs)
            ret.Add(t);
        ret.AddList(ai.ownedInteractables.GetTypeFromListAsU<Interactable, T>(), false);
        return ret;
    }

    public static List<T> SGetAllX<T>(Character character) where T : Interactable
    {
        T[] xs = FindObjectsOfType(typeof(T)) as T[];
        List<T> ret = new List<T>();
        foreach (T t in xs)
            ret.Add(t);
        ret.AddList(character.ownedInteractables.GetTypeFromListAsU<Interactable, T>(), false);
        return ret;
    }

    public static List<T> SPGetAllX<T>(Character character) where T : Interactable
    {
        return character.ownedInteractables.GetTypeFromListAsU<Interactable, T>();
    }
}
