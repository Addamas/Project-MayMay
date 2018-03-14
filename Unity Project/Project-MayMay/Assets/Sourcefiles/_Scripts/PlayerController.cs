using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region Goals
/* Camera Controlls, 
 *  - A-D to rotate left and right around player. W-S to pan/zoom in on player (slowly goes horizontal abit)
 *  - Roof fading when inside house (Roof of building fades to 10% opacity)
 *  - FocusZoom, Depending on player state, Cam adjust zoom en focus, or even the shader of surrounding. (not final)
 * Player Movement
 *  - Right click, on terrain moves player to location - on interactable opens menu with appropriate choices.
 *  - Left click to directly (fast)interact with objects, ie. Closing or opening a door.
 *  
 * Sidenote's
 *  - Use public floats for var's a player can adjust in settings (stuff like, sounds en shiz)
*/
#endregion

public class PlayerController : MonoBehaviour {
    [Header("Main Camera needs to be child 0")]
    
    public Transform player;
    #region Camera Functions Var's
    private Transform cam;
    public Transform camZoomHigh;
    public Transform camZoomLow;
    public float camZoomThreshold;
    [Header("150 is the sweet spot")]
    public float cameraRotationSpeed; //Settings var (150 is the sweet spot imo)
    #endregion
    #region Player Functions Var's
    public NavMeshAgent playerAgent;
    #endregion

    void Start () {
        player = GameObject.FindWithTag("Player").transform;
        cam = transform.GetChild(0);
        cam.position = camZoomHigh.position;
        cam.rotation = camZoomHigh.rotation;
        playerAgent = player.GetComponent<NavMeshAgent>();
        cam.LookAt(player);
    }
	
	void Update () {

        cam.LookAt(player);

        #region Camera Functions

        #region Camera Rotation
        float rotation = (Input.GetAxis("Horizontal") * cameraRotationSpeed) * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
        //cam.LookAt(player); //overcall
        
        #endregion

        #region Camera Zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && camZoomThreshold <= 1f)
        {
            StartCoroutine(CamZoom(0.2f));
            Debug.Log("Zooming In");
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && camZoomThreshold >= 0f)
        {
            StartCoroutine(CamZoom(-0.2f));
            Debug.Log("Zooming Out");
        }
        #endregion

        #endregion

        #region Player Controls
        if (transform.position != player.position)
            transform.position = player.position;

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                if (hit.transform.tag == "Walkable")
                    playerAgent.SetDestination(hit.point);
                else
                    Debug.Log("Can't walk here");
        }
            /*
            When i want to multi-tag something i create Acronyms for the tags Ex:
            Ally = All
            Minion = Min
            Champion = Chm
            So when i want for some object to have multiple tags i tag it with a string containing several of them.
            ex : "EnChm" (for Enemy Champion) contains "Ch" , and i get by with doing the regular checks : tag.Contains(string name)
            */

        #endregion
    }

    #region Camera Zoom Coroutine
    IEnumerator CamZoom(float camZoomThresholder)
    {
        float testZoom;
        testZoom = camZoomThreshold + camZoomThresholder;
        Debug.Log("Testzoom = " + testZoom);

        while (camZoomThreshold <= testZoom)
        {
            cam.position = Vector3.Lerp(camZoomHigh.position, camZoomLow.position, camZoomThreshold);

            if (testZoom > camZoomThreshold)
            {
                camZoomThreshold += 0.01f;
            }
            yield return null;
        }
        while (camZoomThreshold >= testZoom)
        {
            cam.position = Vector3.Lerp(camZoomHigh.position, camZoomLow.position, camZoomThreshold);

            if (testZoom < camZoomThreshold)
            {
                camZoomThreshold -= 0.01f;
            }
            yield return null;
        }
    }
    #endregion

}