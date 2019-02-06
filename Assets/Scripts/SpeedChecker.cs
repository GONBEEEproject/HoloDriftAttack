using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChecker : MonoBehaviour {

    private Vector3 latest;
    private float speed;
    private GameObject cam;

    [SerializeField]
    private TextMesh speedText;

	// Use this for initialization
	void Start () {
        cam = Camera.main.gameObject;
        latest = cam.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        speed = ((cam.transform.position - latest) / Time.deltaTime).magnitude;

        latest = cam.transform.position;

        speedText.text = speed.ToString("#.00");
	}
}
