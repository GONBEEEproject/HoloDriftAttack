using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereReceptor : MonoBehaviour {

    private GameObject sphere;

    private int pos, oldPos;
    private float dotTime, dotMax = 0.5f;
    private int dotCount;

    [SerializeField]
    private GameObject[] slides;

    [SerializeField]
    private Text status;

    private void Start()
    {
        for(int i = 0; i < slides.Length; i++)
        {
            slides[i].SetActive(false);
        }
        slides[0].SetActive(true);
    }

    private void Update()
    {
        if (sphere == null)
        {
            FindState();
        }
        else
        {
            CheckState();
        }
    }

    private void CheckState()
    {
        status.text = "";

        if (slides.Length == 0) return;

        pos = Mathf.RoundToInt(sphere.transform.position.x);

        if (pos != oldPos)
        {
            int page = pos % slides.Length;

            for (int i = 0; i < slides.Length; i++)
            {
                if (slides[i] == null) return;

                slides[i].SetActive(false);
            }

            slides[Mathf.Abs(page)].SetActive(true);

            oldPos = pos;
            Debug.Log("Changed");
        }
    }

    private void FindState()
    {
        sphere = GameObject.FindWithTag("StateSphere");
        //Debug.Log("Finding...");
        dotTime += Time.deltaTime;
        if (dotTime > dotMax)
        {
            dotTime = 0;
            dotCount++;
            status.text += ".";
            if (dotCount > 5)
            {
                dotCount = 0;
                status.text = "Searching HoloLens";
            }
        }
    }
}
