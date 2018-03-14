using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region Goals
//one day, become as great as my great senpai san, JAN <3

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

    #region Player Var's
    [SerializeField]
    private Transform player;
    private NavMeshAgent playerAgent;
    #endregion

    #region Camera Var's
    [SerializeField]
    private Transform cam, camZoomHigh, camZoomLow;
    [SerializeField]
    private float cameraRotationSpeed = 150, zoomSpeed;

    [SerializeField]
    private string scroll = "Mouse ScrollWheel", horizontal = "Horizontal";

    #region Fast References
    private Vector3 CamPos
    {
        get
        {
            return cam.transform.position;
        }
        set
        {
            cam.transform.position = value;
        }
    }

    private Quaternion CamRot
    {
        get
        {
            return cam.rotation;
        }
        set
        {
            cam.rotation = value;
        }
    }
    #endregion

    #endregion

    private void Awake()
    {
        playerAgent = player.GetComponent<NavMeshAgent>();
    }

    private void Start () {
        CamPos = camZoomHigh.position;
        CamRot = camZoomHigh.rotation;

        cam.LookAt(player);
    }

    private float rotation;
    private void Update () {

        cam.LookAt(player);

        #region Camera Functions

        #region Camera Rotation
        rotation = (Input.GetAxis(horizontal) * cameraRotationSpeed) * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
        //cam.LookAt(player); //overcall
        
        #endregion

        #region Camera Zoom

        if (Input.GetAxisRaw(scroll) != 0)
        {
            if (camZoom != null)
                StopCoroutine(camZoom);
            camZoom = StartCoroutine(CamZoom());
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
    /*
    private void CamZoom(float speed)
    {
        Transform target = speed > 0 ? camZoomLow : camZoomHigh;
        float disPart = Mathf.Abs(speed) * zoomSpeed * Time.deltaTime;
        CamPos = Vector3.MoveTowards(CamPos, target.position, disPart);
    }
    */

    private Coroutine camZoom;
    private IEnumerator CamZoom()
    {
        float axis = Input.GetAxis(scroll);
        float startingDistance = Mathf.Abs(axis) * zoomSpeed, 
            distanceLeft = startingDistance;
        Transform target = axis > 0 ? camZoomLow : camZoomHigh;

        float disPart;
        while(distanceLeft > 0)
        {
            disPart = zoomSpeed * Time.deltaTime;
            distanceLeft -= disPart;

            CamPos = Vector3.MoveTowards(CamPos, target.position, disPart);
            yield return null;
        }
    }
    #endregion
}