using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBalloon : MonoBehaviour {

    public Text text;

	private void Update () {
        transform.LookAt(GameManager.instance.mainCamera.transform);
    }
}
