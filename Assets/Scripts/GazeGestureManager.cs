using UnityEngine;
using UnityEngine.XR.WSA.Input;
using System.Collections;
using System.Collections.Generic;

public class GazeGestureManager : MonoBehaviour
{

    private static GazeGestureManager instance;
    public static GazeGestureManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GazeGestureManager>();
            }
            return instance;
        }
    }


    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;

    public enum NowState
    {
        Start,Marker,TimeAttack
    }

    public NowState state = NowState.Start;

    public GameObject StartFlag;
    public GameObject Marker;

    private GameObject FlagHolder;
    private List<GameObject> MarkerHolder = new List<GameObject>();


    
    // Use this for initialization
    void Start()
    { 

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.Tapped += (args) =>
        {
            //// Send an OnSelect message to the focused object and its ancestors.
            //if (FocusedObject != null)
            //{
            //    FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
            //}

            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit info;

            if (Physics.Raycast(headPosition, gazeDirection, out info))
            {
                Tap(info);
            }




        };
        recognizer.StartCapturingGestures();
    }

    private void Tap(RaycastHit info)
    {
        switch (state)
        {
            case NowState.Start:

                FlagHolder = Instantiate(StartFlag, info.point + new Vector3(0, 0.5f, 0), Quaternion.identity);
                state = NowState.Marker;

                MarkerManager.Instance.StartMarker(FlagHolder);

                break;
            case NowState.Marker:
                if (info.transform.tag == StartFlag.tag)
                {
                    MarkerManager.Instance.EndMarker();

                    state = NowState.TimeAttack;
                }
                else if (info.transform.tag == Marker.tag)
                {
                    break;
                }
                else
                {
                    GameObject tmp = Instantiate(Marker, info.point + new Vector3(0, 0.5f, 0), Quaternion.identity);

                    MarkerManager.Instance.SetNewMarker(tmp.GetComponent<Node>());

                }


                break;
            case NowState.TimeAttack:
                Destroy(FlagHolder);
                for(int i = 0; i < MarkerHolder.Count; i++)
                {
                    Destroy(MarkerHolder[i].gameObject);
                }

                state = NowState.Start;

                break;
        }
    }




    private void Update()
    {

        Vector2 touchScreenPosition = Input.mousePosition;

        touchScreenPosition.x = Mathf.Clamp(touchScreenPosition.x, 0.0f, Screen.width);
        touchScreenPosition.y = Mathf.Clamp(touchScreenPosition.y, 0.0f, Screen.height);

        Camera gameCamera = Camera.main;
        Ray touchPointToRay = gameCamera.ScreenPointToRay(touchScreenPosition);

        // デバッグ機能を利用して、スクリーンビューでレイが出ているか見てみよう。
        Debug.DrawRay(touchPointToRay.origin, touchPointToRay.direction * 1000.0f);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit info;

            if (Physics.Raycast(gameCamera.gameObject.transform.position, touchPointToRay.direction, out info))
            {
                Tap(info);
            }
        }
    }


}
