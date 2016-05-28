using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FactoryManager : MonoBehaviour
{

	private GameObject planetPlaceholder;
	public GameObject planetPlaceholderPrefab;
	public List<GameObject> planetPrefabs;
	public List<GameObject> sunPrefabs;

	// Use this for initialization
	void CreateSun ()
	{
		this.planetPlaceholder = Instantiate (planetPlaceholderPrefab);
		this.planetPlaceholder.SetActive (false);

        // Create a SUN
        Vector3 sunpos = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
        GameObject sun = PhotonNetwork.Instantiate("Sun", sunpos, Quaternion.identity, 0);
		//GameObject sun = Instantiate (sunPrefabs [Random.Range (0, sunPrefabs.Count - 1)]);
		Sun obj = sun.GetComponent<Sun> ();
		obj.owner = GameProperties.props.playerName;
		obj.type = PlanetType.Sun;
		obj.SetColor(new Color (Random.value, Random.value, Random.value));
		GameProperties.props.mySun = sun;
		
		CameraControls cam = Camera.main.GetComponent<CameraControls> ();
		if (cam != null) {
			cam.PointAt (sun);
		}

        GameProperties.props.state = GameState.Playing;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        if( GameProperties.props.state == GameState.Ready )
        {
            Debug.Log("creating from update");
            this.CreateSun();

        } else if (GameProperties.props.state == GameState.CreatePlanet) {
			if (planetPlaceholder.activeSelf == false) {
				planetPlaceholder.SetActive (true);
			}
		
			Vector3 loc = GetIntersectionLocation ();
			
			planetPlaceholder.transform.position = loc;
			
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("Create a planet here");
				GameProperties.props.state = GameState.Playing;

                string planetName = ((GameObject)planetPrefabs[Random.Range(0, planetPrefabs.Count - 1)]).name;

                //GameObject newPlanet = Instantiate (planetPrefabs [0], loc, Quaternion.identity) as GameObject;
                GameObject newPlanet = PhotonNetwork.Instantiate(planetName, loc, Quaternion.identity, 0) as GameObject;
                Orbiter orbiter = newPlanet.GetComponent<Orbiter> ();
				orbiter.orbitCenter = GameProperties.props.selectedObject;
				orbiter.owner = GameProperties.props.playerName;
				orbiter.type = PlanetType.LifePlanet;
                orbiter.orbitSpeed = Random.Range(1.0f, 5.0f);
			}
			
		} else {
			if (planetPlaceholder.activeSelf == true) {
				planetPlaceholder.SetActive (false);
			}
		}
	}
	
	private Vector3 GetIntersectionLocation ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float delta = ray.origin.y;
		Vector3 dirNorm = ray.direction / ray.direction.y;
		Vector3 intersect = ray.origin - dirNorm * delta;
		return intersect;
	}
}
