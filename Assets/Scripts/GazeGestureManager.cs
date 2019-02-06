using UnityEngine;
using UnityEngine.XR.WSA.Input;
using System.Collections;
using System.Collections.Generic;
using System;

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


    
    void Start()
    { 
        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.Tapped += (args) =>
        {
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

                MarkerHolder = new List<GameObject>();

                MarkerManager.Instance.StartMarker(FlagHolder);

                state = NowState.Marker;
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

                for (int i = 0; i < MarkerHolder.Count; i++)
                {
                    if (MarkerHolder[i] == null) continue;

                    Destroy(MarkerHolder[i].gameObject);
                }

                state = NowState.Start;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit info;

            if (Physics.Raycast(headPosition, gazeDirection, out info))
            {
                Tap(info);
            }
        }
    }
}
