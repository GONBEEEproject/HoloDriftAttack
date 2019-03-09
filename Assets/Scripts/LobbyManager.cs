using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LobbyManager : Photon.MonoBehaviour {

    [SerializeField]
    private TextMesh status;

    [SerializeField]
    private float textSpeed;

    private string waitText;

    private void Start()
    {
        waitText = "Connecting";
        //Debug.Log("Connecting");
        PhotonNetwork.ConnectUsingSettings(null);

        StartCoroutine(TextUpdate());
    }

    private void OnJoinedLobby()
    {
        waitText = "Joined Lobby";
        //Debug.Log("OnJoinedLobby");

        RoomOptions option = new RoomOptions();
        option.MaxPlayers = 2;
        option.IsOpen = true;
        option.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom("Main", option, null);
    }

    private void OnJoinedRoom()
    {
        waitText = "Joined Room";
        //Debug.Log("OnJoinedRoom");
        StartCoroutine(SceneMoveSequence());
    }

    private IEnumerator SceneMoveSequence()
    {
        waitText = "Scene Changing";
        //Debug.Log("SceneChanging");
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("Main");
    }

    private bool CheckText()
    {
        return status.text != waitText;
    }

    private IEnumerator TextUpdate()
    {
        if (status.text[0] != waitText[0])
        {
            status.text = "";
            //Debug.Log("Reset");
        }

        if (status.text != waitText)
        {
            status.text += waitText[status.text.Length];
        }

        yield return new WaitForSeconds(textSpeed);

        StartCoroutine(TextUpdate());
    }
}
