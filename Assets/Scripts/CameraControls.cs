using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour
{

	public float totalZoom = 2f;
	public float zoomMax = 2.85f;
	public float zoomMin = 0.1f;
	public float zoomModifier = 10f;

	public float scrollSpeed = 25f;
	public float scrollArea = 25f;
    
	private Vector3 locationOffset;
    
	public void Start ()
	{
		Vector3 cameraLoc = Camera.main.transform.position;
		//cameraLoc.y = 0f;
		Debug.Log ("CameraLoc: " + cameraLoc);
		Vector3 center = this.FindCenterLoc ();
		Debug.Log ("center: " + center);
		this.locationOffset = cameraLoc - center;
		Debug.Log ("offset: " + locationOffset);
	}

	
	// Update is called once per frame
	void Update ()
	{

		Vector3 translation = Vector3.zero;

		float translate = Input.GetAxis ("Mouse ScrollWheel");

		if (totalZoom + translate > zoomMax) {
			translate = zoomMax - totalZoom;
		} else if (totalZoom + translate < zoomMin) {
			translate = zoomMin - totalZoom;
		}

		totalZoom += translate;

		this.gameObject.transform.Translate (0, 0, translate * zoomModifier);

		float xmouse = Input.mousePosition.x;
		float ymouse = Input.mousePosition.y;

		bool onScreen = (xmouse > 0 && xmouse < Screen.width && ymouse > 0 && ymouse < Screen.height);

		if (xmouse < scrollArea && onScreen) {
			// scroll left
			translation += Vector3.right * -scrollSpeed * Time.deltaTime / totalZoom;
		}

		if (xmouse > Screen.width - scrollArea && onScreen) {
			// scroll right
			translation += Vector3.right * scrollSpeed * Time.deltaTime / totalZoom;
		}

		if (ymouse < scrollArea && onScreen) {
			// scroll up
			translation += Vector3.forward * -scrollSpeed * Time.deltaTime / totalZoom;
		}

		if (ymouse > Screen.height - scrollArea && onScreen) {
			// scroll down
			translation += Vector3.forward * scrollSpeed * Time.deltaTime / totalZoom;
		}

		this.transform.position += translation;
        
		if (Input.GetKeyUp (KeyCode.C)) {
			// Center View on Sun
			this.PointAt (GameProperties.props.mySun);
		}
	}
    
	public void PointAt (GameObject a_object)
	{
		this.transform.position = a_object.transform.position + this.locationOffset;
	}
	
	private Vector3 FindCenterLoc ()
	{
		Vector3 center = new Vector3 (Screen.width / 2, Screen.height / 2);
		Ray ray = Camera.main.ScreenPointToRay (center);
		float delta = ray.origin.y;
		Vector3 dirNorm = ray.direction / ray.direction.y;
		Vector3 intersect = ray.origin - dirNorm * delta;
		return intersect;
	}
}
