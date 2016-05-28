using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour
{

	public int LevelArea = 100;
		
	public int ScrollArea = 25;
	public int ScrollSpeed = 25;
	public int DragSpeed = 50;
	
	public float totalZoom = 2; //scaled zoom value
	public float zoomMax = 2.85f;
	public float zoomMin = 0.1f;
	public float zoomSpeed = 10;
		
	// Update is called once per frame
	void Update ()
	{
		// Init camera translation for this frame.
		var translation = Vector3.zero;

		float translate = Input.GetAxis ("Mouse ScrollWheel");
		if (totalZoom + translate > zoomMax) {
			translate = zoomMax - totalZoom;
		} else if (totalZoom + translate < zoomMin) {
			translate = zoomMin - totalZoom;
		}
		totalZoom += translate;
		this.gameObject.transform.Translate (0, 0, translate * zoomSpeed);


		// Move camera with arrow keys
		translation += new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			
		// Move camera with mouse
		if (Input.GetMouseButton (2)) { // MMB
			// Hold button and drag camera around
			translation -= new Vector3 (Input.GetAxis ("Mouse X") * DragSpeed * Time.deltaTime, 0, 
				                           Input.GetAxis ("Mouse Y") * DragSpeed * Time.deltaTime);
		} else {
			bool onScreen = (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height);
		
			// Move camera if mouse pointer reaches screen borders
			if (Input.mousePosition.x < ScrollArea && onScreen) {
				translation += Vector3.right * -ScrollSpeed * Time.deltaTime / totalZoom;
			}
				
			if (Input.mousePosition.x >= Screen.width - ScrollArea && onScreen) {
				translation += Vector3.right * ScrollSpeed * Time.deltaTime / totalZoom;
			}
				
			if (Input.mousePosition.y < ScrollArea && onScreen) {
				translation += Vector3.forward * -ScrollSpeed * Time.deltaTime / totalZoom;
			}
				
			if (Input.mousePosition.y > Screen.height - ScrollArea && onScreen) {
				translation += Vector3.forward * ScrollSpeed * Time.deltaTime / totalZoom;
			}
		}
			
		// Keep camera within level and zoom area
		Vector3 desiredPosition = GetComponent<Camera> ().transform.position + translation;
		if (desiredPosition.x < -LevelArea || LevelArea < desiredPosition.x) {
			translation.x = 0;
		}
		if (desiredPosition.z < -LevelArea || LevelArea < desiredPosition.z) {
			translation.z = 0;
		}
			
		// Finally move camera parallel to world axis
		GetComponent<Camera> ().transform.position += translation;
	}
}
