using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    private static MarkerManager instance;
    public static MarkerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MarkerManager>();
            }
            return instance;
        }
    }


    private List<Node> nodes = new List<Node>();
    private int flagNum;

    private TextMesh timeMesh;

    private float scoreTime;

    private bool isAttack=false;

    public AudioClip StartSound, EndSound,NodeSound,NodePut;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isAttack)
        {
            scoreTime += Time.deltaTime;
            timeMesh.text = scoreTime.ToString();
        }

    }



    public void MarkerTouched(Node node)
    {
        if (nodes[0] == node)
        {
            nodes.Remove(node);
            Destroy(node.gameObject);
            source.PlayOneShot(NodeSound);
        }
    }

    public void FlagTouched()
    {
        if (nodes.Count == 0)
        {
            isAttack = false;
            source.PlayOneShot(EndSound);
        }
        else if (isAttack == false)
        {
            isAttack = true;
            source.PlayOneShot(StartSound);
        }
    }

    public void StartMarker(GameObject startFlag)
    {
        nodes = new List<Node>();
        scoreTime = 0;

        timeMesh = startFlag.GetComponentInChildren<TextMesh>();
        source.PlayOneShot(NodePut);
    }

    public void SetNewMarker(Node newNode)
    {
        newNode.SetUp(nodes.Count + 1);
        nodes.Add(newNode);
        source.PlayOneShot(NodePut);
    }

    public void EndMarker()
    {
        source.PlayOneShot(EndSound);
    }

}
