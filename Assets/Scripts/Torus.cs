//This Script is base on the script created by Steffen (http://forum.unity3d.com/threads/torus-in-unity.8487/)

// The improvement is the possibility to manipulate the ring outside the script. 
// This script must be attached to an empty Game Object (Main).
// When the script starts, it creates an empty Game Object that will be the ring (meshRing).
// The user can changes the segmentRadius, tubeRadius and tubes of the meshRing through the 
// transform.scale.x, transform.scale.y and transform.scale.z, respectively, of the main Game Object.
// The position, rotation and color of the meshRing are copied from the Main Game Object.


// Outside this script, the transform.scale of Main Game Object can be accessed by: GameObject.Find(name of the Main Game Object);


using UnityEngine;
using System.Collections;

public class Torus
{

	public static void CreateTorusMesh (GameObject aGameObject, float aRadius, float aThickness, Color aColor, int aTubes = 4)
	{
		// optional code
		int segments = (int)Mathf.Floor (aRadius * 20f);
		
		// Total vertices
		int totalVertices = segments * aTubes;
		
		// Total primitives
		int totalPrimitives = totalVertices * 2;
		
		// Total indices
		int totalIndices = totalPrimitives * 3;
		
		// Init vertexList and indexList
		ArrayList verticesList = new ArrayList ();
		ArrayList indicesList = new ArrayList ();
		
		// Save these locally as floats
		float numSegments = segments;
		float numTubes = aTubes;
		
		// Calculate size of segment and tube
		float segmentSize = 2 * Mathf.PI / numSegments;
		float tubeSize = 2 * Mathf.PI / numTubes;
		
		// Create floats for our xyz coordinates
		float x = 0;
		float y = 0;
		float z = 0;
		
		// Init temp lists with tubes and segments
		ArrayList segmentList = new ArrayList ();
		ArrayList tubeList = new ArrayList ();
		
		// Loop through number of tubes
		for (int i = 0; i < numSegments; i++) {
			tubeList = new ArrayList ();
			
			for (int j = 0; j < numTubes; j++) {
				// Calculate X, Y, Z coordinates.
				x = (aRadius + aThickness * Mathf.Cos (j * tubeSize)) * Mathf.Cos (i * segmentSize);
				y = (aRadius + aThickness * Mathf.Cos (j * tubeSize)) * Mathf.Sin (i * segmentSize);
				z = aThickness * Mathf.Sin (j * tubeSize);
				
				// Add the vertex to the tubeList
				tubeList.Add (new Vector3 (x, z, y));
				
				// Add the vertex to global vertex list
				verticesList.Add (new Vector3 (x, z, y));
			}
			
			// Add the filled tubeList to the segmentList
			segmentList.Add (tubeList);
		}
		
		// Loop through the segments
		for (int i = 0; i < segmentList.Count; i++) {
			// Find next (or first) segment offset
			int n = (i + 1) % segmentList.Count;
			
			// Find current and next segments
			ArrayList currentTube = (ArrayList)segmentList [i];
			ArrayList nextTube = (ArrayList)segmentList [n];
			
			// Loop through the vertices in the tube
			for (int j = 0; j < currentTube.Count; j++) {
				// Find next (or first) vertex offset
				int m = (j + 1) % currentTube.Count;
				
				// Find the 4 vertices that make up a quad
				Vector3 v1 = (Vector3)currentTube [j];
				Vector3 v2 = (Vector3)currentTube [m];
				Vector3 v3 = (Vector3)nextTube [m];
				Vector3 v4 = (Vector3)nextTube [j];
				
				// Draw the first triangle
				indicesList.Add ((int)verticesList.IndexOf (v1));
				indicesList.Add ((int)verticesList.IndexOf (v2));
				indicesList.Add ((int)verticesList.IndexOf (v3));
				
				// Finish the quad
				indicesList.Add ((int)verticesList.IndexOf (v3));
				indicesList.Add ((int)verticesList.IndexOf (v4));
				indicesList.Add ((int)verticesList.IndexOf (v1));
			}
		}
		
		
		aGameObject.AddComponent<MeshFilter> ();
		aGameObject.AddComponent<MeshRenderer> ();
		
		Mesh mesh = new Mesh ();
		
		Vector3[] vertices = new Vector3[totalVertices];
		verticesList.CopyTo (vertices);
		int[] triangles = new int[totalIndices];
		indicesList.CopyTo (triangles);
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		
		mesh.RecalculateBounds ();
		mesh.Optimize ();
		MeshFilter mFilter = aGameObject.GetComponent (typeof(MeshFilter)) as MeshFilter;
		mFilter.mesh = mesh;
        //aGameObject.GetComponent<Renderer>().material = Resources.Load("Materials/Ring Material", typeof(Material)) as Material;
		aGameObject.GetComponent<Renderer> ().material.color = aColor;
        //aGameObject.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
        //Shader.EnableKeyword("_EMISSION");
        aGameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", aColor);
        aGameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        aGameObject.GetComponent<Renderer>().receiveShadows = false;

    }

}
