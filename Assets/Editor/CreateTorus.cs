using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateTorus : ScriptableWizard
{
	//private static float Pi = 3.14159f;
	
	public float radius = 1f;
	public float thickness = 0.1f;
	protected int segments = 32;
	protected int tubes = 4;
	public Color color = Color.white;
	
	[MenuItem ("GameObject/Create Other/Torus")]
	static void CreateWizard ()
	{
		ScriptableWizard.DisplayWizard ("Create Torus", typeof(CreateTorus));
	}
	
	void OnWizardCreate ()
	{
		//float diameter = radius * 2f;
		//float circumference = Mathf.PI * diameter;
		//float interval = 360f / (circumference / (thickness * 2f));
		
		GameObject torus = new GameObject ("Torus");
		
		Torus.CreateTorusMesh (torus, radius, thickness, color, tubes);
		
		Selection.activeObject = torus;
	}
	
	
}