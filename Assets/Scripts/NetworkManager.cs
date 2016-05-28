using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviour {

    private bool createdRoom = false;

	// Use this for initialization
	void Start () {
        Debug.Log("joining room: " + GameProperties.props.roomName);
        PhotonNetwork.JoinRoom(GameProperties.props.roomName);
	}
	
	// Update is called once per frame
	void Update () {
	    if( Input.GetKey(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Startup");
        }
	}

    void OnPhotonJoinRoomFailed ()
    {
        Debug.Log("Failed to Join Room");
        this.createdRoom = true;
        PhotonNetwork.CreateRoom(GameProperties.props.roomName);
    }

    void OnJoinedRoom ()
    {
        Debug.Log("Joined the room");
        if( this.createdRoom == true )
        {
            PhotonNetwork.room.maxPlayers = 6;
        }

        this.SpawnPlayer();
    }

    void SpawnPlayer()
    {
        PhotonNetwork.playerName = GameProperties.props.playerName;

        GameProperties.props.state = GameState.Ready;
    }

    void onGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
