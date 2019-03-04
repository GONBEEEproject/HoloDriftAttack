using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LobbyManager : Photon.MonoBehaviour {

    [SerializeField]
    private TextMesh status;

    private void Start()
    {
        status.text = "Connecting";
        Debug.Log("Connecting");
        PhotonNetwork.ConnectUsingSettings(null);
    }

    private void OnJoinedLobby()
    {
        status.text = "Joined Lobby";
        Debug.Log("OnJoinedLobby");

        RoomOptions option = new RoomOptions();
        option.MaxPlayers = 2;
        option.IsOpen = true;
        option.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom("Main", option, null);
    }

    private void OnJoinedRoom()
    {
        status.text = "Joined Room";
        Debug.Log("OnJoinedRoom");
        StartCoroutine(SceneMoveSequence());
    }

    private IEnumerator SceneMoveSequence()
    {
        status.text = "Scene Changing";
        Debug.Log("SceneChanging");
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("Main");
    }
}
