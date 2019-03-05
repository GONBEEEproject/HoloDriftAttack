using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMove :Photon.MonoBehaviour {

    private static SphereMove instance;
    public static SphereMove Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SphereMove>();
            }
            return instance;
        }
    }

    private GameObject state;

    [SerializeField]
    private TextMesh statePos;

    private void Start()
    {
        state = PhotonNetwork.Instantiate("StateSphere", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    [ContextMenu("MoveNext")]
    public void MoveNext()
    {
        int x = Mathf.RoundToInt(state.transform.position.x);
        x++;
        state.transform.position = new Vector3(x, 0, 0);
    }

    [ContextMenu("MovePrevious")]
    public void MovePrevious()
    {
        int x = Mathf.RoundToInt(state.transform.position.x);
        x--;
        state.transform.position = new Vector3(x, 0, 0);
    }

    public void PositionUpdate(float movement)
    {
        float x = state.transform.position.x;

        state.transform.position = new Vector3(x + movement, 0, 0);
    }

    private void Update()
    {
        statePos.text = state.transform.position.ToString();
    }
}
