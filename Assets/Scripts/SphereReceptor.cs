using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereReceptor : MonoBehaviour {

    private GameObject sphere;

    private int pos, oldPos;

    [SerializeField]
    private GameObject[] slides;

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
            sphere = GameObject.FindWithTag("StateSphere");
            Debug.Log("Finding...");
        }

        if (sphere != null)
        {
            CheckState();
        }
    }

    private void CheckState()
    {
        if (slides.Length == 0) return;

        pos = Mathf.RoundToInt(sphere.transform.position.x);

        if (pos != oldPos)
        {
            int page = pos % slides.Length;

            for(int i = 0; i < slides.Length; i++)
            {
                if (slides[i] == null) return;

                slides[i].SetActive(false);
            }

            slides[Mathf.Abs(page)].SetActive(true);

            oldPos = pos;
        }
    }
}
