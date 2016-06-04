using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour
{

	public float totalZoom = 2f;
	public float maxZoom = 13f;
	public float minZoom = -25f;
	public float zoomModifier = 10f;
    private float baseZoom = 0f;
    private float currentZoom = 0f;
    private float scaledCurrentZoom = 0f;
    private float zoomFactor = 1.0f;

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
        // distance from camera to origin plane
        this.baseZoom = this.FindBaseZoom();
        Debug.Log("distance: " + this.baseZoom);
        this.currentZoom = this.baseZoom;
	}

	
	// Update is called once per frame
	void Update ()
	{

		Vector3 translation = Vector3.zero;

		float translate = Input.GetAxis ("Mouse ScrollWheel") * this.zoomModifier;

        if (translate != 0.0f)
        {
            //Debug.Log(translate);
            
            if (this.currentZoom + translate > this.maxZoom)
            {
                translate = this.maxZoom - this.currentZoom;
            }
            else if (this.currentZoom + translate < this.minZoom)
            {
                translate = this.minZoom - this.currentZoom;
            }
            
            this.currentZoom += translate;
            this.scaledCurrentZoom = this.currentZoom + 26;

            this.zoomFactor = (this.baseZoom +26) / this.scaledCurrentZoom;

            this.gameObject.transform.Translate(0, 0, translate);

            Debug.Log(this.zoomFactor);
        }

		float xmouse = Input.mousePosition.x;
		float ymouse = Input.mousePosition.y;

		bool onScreen = (xmouse > 0 && xmouse < Screen.width && ymouse > 0 && ymouse < Screen.height);

		if (xmouse < scrollArea && onScreen) {
			// scroll left
			translation += Vector3.right * -scrollSpeed * Time.deltaTime * zoomFactor ;
		}

		if (xmouse > Screen.width - scrollArea && onScreen) {
			// scroll right
			translation += Vector3.right * scrollSpeed * Time.deltaTime * zoomFactor;
		}

		if (ymouse < scrollArea && onScreen) {
			// scroll up
			translation += Vector3.forward * -scrollSpeed * Time.deltaTime * zoomFactor;
		}

		if (ymouse > Screen.height - scrollArea && onScreen) {
			// scroll down
			translation += Vector3.forward * scrollSpeed * Time.deltaTime * zoomFactor;
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
        this.currentZoom = this.baseZoom;
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

    private float FindBaseZoom()
    {
        Plane plane = new Plane(Vector3.up, Vector3.up);
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(center);
        float distance;

        plane.Raycast(ray, out distance);

        return distance;
    }
}
