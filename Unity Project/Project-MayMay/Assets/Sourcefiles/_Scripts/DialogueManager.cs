using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance;

    [SerializeField]
    private int difficultyDis, maxDis;

    private void Awake()
    {
        instance = this;
    }

    public void ConvertTextVisually(Character character, string text, float duration)
    {
        float dis = Vector3.Distance(character.Pos, GameManager.Player.transform.position);

        if (dis > maxDis)
            return;

        dis = Mathf.InverseLerp(difficultyDis, maxDis, dis);
        string newText = "";

        if (dis > difficultyDis)
            for (int i = 0; i < text.Length; i++)
                newText += Random.Range(0, 1f) < dis ? "." : text[i].ToString();
        else
            newText = text;

        Debug.Log(newText);

        //use character and text and destroy / fade after duration
    }
}
