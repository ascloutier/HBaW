using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour {

    private string version = "0.1";

    void Start ()
    {
        if(GameProperties.props.gameSetup == false )
        {
            this.Connect();
        }
    }

    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings(this.version);
    }

    void OnConnectedToMaster ()
    {
        Debug.Log("Connected to Photon: " + PhotonNetwork.Server.ToString() + " : " + PhotonNetwork.ServerAddress.ToString());
        PhotonNetwork.JoinLobby();
    }

    void OnFailedToConnectToPhoton()
    {
        Debug.Log("Failed to connect to photon");
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void OnJoinedLobby ()
    {
        Debug.Log("I've joined the lobby: " + PhotonNetwork.lobby.Type);
        GameProperties.props.state = GameState.Connected;
    }

    public void OnJoinGameButton ()
    {
        if (GameProperties.props.state == GameState.Connected)
        {
            SceneManager.LoadScene("PlayField");
        }
    }
}
