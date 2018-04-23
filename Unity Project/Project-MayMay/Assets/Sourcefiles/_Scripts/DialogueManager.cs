using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance;
    [SerializeField]
    private GameObject textBalloon;

    [SerializeField]
    private int difficultyDis, maxDis;
    [SerializeField]
    private float heightOffset;

    private void Awake()
    {
        instance = this;
    }

    public void ConvertTextVisually(Transform trans, string text, float duration)
    {
        float dis = Vector3.Distance(trans.position, GameManager.Player.transform.position);

        if (dis > maxDis)
            return;

        float lerp = Mathf.InverseLerp(difficultyDis, maxDis, dis);

        string newText = "";

        if (dis > difficultyDis)
            for (int i = 0; i < text.Length; i++)
                newText += Random.Range(0, 1f) < lerp ? "." : text[i].ToString();
        else
            newText = text;

        StartCoroutine(VisualizeText(trans, newText, duration));
    }

    private List<Transform> activeVisualizers = new List<Transform>();

    private IEnumerator VisualizeText(Transform trans, string text, float duration)
    {
        while (activeVisualizers.Contains(trans))
            yield return null;

        activeVisualizers.Add(trans);
        GameObject balloon = Instantiate(textBalloon, trans.position + Vector3.up * heightOffset, Quaternion.identity);
        balloon.GetComponent<TextBalloon>().text.text = text;

        yield return new WaitForSeconds(duration);
        
        activeVisualizers.Remove(trans);
        Destroy(balloon);
    }
}
