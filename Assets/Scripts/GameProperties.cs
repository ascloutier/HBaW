using UnityEngine;
using System.Collections;

public class GameProperties : MonoBehaviour
{

    public static GameProperties props;
	
	// player properties
	public string playerName;
	public string roomName = "TheUniverse";
	public GameObject mySun;
	public GameState state = GameState.Startup;
	public bool gameSetup = false;
	public GameObject selectedObject;
	
	void Awake ()
	{
        if (props == null) {
            DontDestroyOnLoad(this.gameObject);
            props = this;
		} else if (props != this) {
			Destroy (this.gameObject);
		}
	}
	
}
