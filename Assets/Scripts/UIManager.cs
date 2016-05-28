using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameObject selectionPrefab;
    public Text playerNameField;


	private GameObject selection;

	// Use this for initialization
	void Start ()
	{
		this.selection = Instantiate (selectionPrefab);
		this.selection.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonUp (0)) {
			this.SelectObject ();
		}
		
		this.DisplaySelection ();
        this.UpdateStats();
	}
	
	private void SelectObject ()
	{
		// did I hit UI?
		if (EventSystem.current.IsPointerOverGameObject ()) {
			Debug.Log ("I'm over a UI object?");
			return;
		}
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit)) {
		
			Transform hitTransform = GetRootTransform (hit.transform);
			
			PlayerObject po = hitTransform.GetComponent<PlayerObject> ();
			
			if (po != null) {
				if (po.owner == GameProperties.props.playerName) {
					Debug.Log ("setting new selected object");
					GameProperties.props.selectedObject = hitTransform.gameObject;
				} else {
					Debug.Log ("not mine");
				}
			} else {
				Debug.Log ("I didn't hit a player object");
				GameProperties.props.selectedObject = null;
			}
		} else {
			Debug.Log ("I didn't hit anything");
			GameProperties.props.selectedObject = null;
		}

	}
	
	private void DisplaySelection ()
	{
		if (GameProperties.props.selectedObject) {
			if (this.selection.activeSelf == false) {
				this.selection.SetActive (true);
			}
			this.selection.transform.position = GameProperties.props.selectedObject.transform.position;
			float scaleFactor = GameProperties.props.selectedObject.GetComponent<PlayerObject> ().selectionScale;
			this.selection.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
		} else {
			if (this.selection.activeSelf == true) {
				this.selection.SetActive (false);
			}
		}
	}
	
	private void UpdateStats()
    {
        if( PhotonNetwork.inRoom )
        {
            PhotonPlayer[] playerlist = PhotonNetwork.playerList;

            playerNameField.text = "";

            for( int i = 0; i < playerlist.Length; i++ )
            {
                PhotonPlayer player = playerlist[i];
                playerNameField.text += player.name + "\n";
            }
        }
    }
	
	// Button Functions
	public void OnCreatePlanet ()
	{
		if (GameProperties.props.selectedObject != null) {
			GameProperties.props.state = GameState.CreatePlanet;
		}
	}
	
	Transform GetRootTransform (Transform hitTransform)
	{
		PlayerObject h = hitTransform.GetComponent<PlayerObject> ();
		
		while (h==null && hitTransform.parent) {
			hitTransform = hitTransform.parent.transform;
			h = hitTransform.GetComponent<PlayerObject> ();
		}
		return hitTransform;
	}
}
