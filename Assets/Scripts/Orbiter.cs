using UnityEngine;
using System.Collections;

public class Orbiter : PlayerObject
{

	public float orbitSpeed = 1f;
	public float rotationSpeed = 1f;
	public GameObject orbitCenter;

	private GameObject torus;
	//private float orbitDistance;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

        if (this.orbitCenter != null)
        {
            float orbitDistance = Mathf.Abs(Vector3.Distance(this.gameObject.transform.position, this.orbitCenter.transform.position));

            this.torus = new GameObject();
            this.torus.transform.position = this.orbitCenter.transform.position;

            Torus.CreateTorusMesh(this.torus, orbitDistance, 0.007f, Color.white);
        }
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();

        if (!photonView)
        {
            this.UpdateOrbitPosition();
        }
        else
        {
            if (photonView.isMine)
            {
                this.UpdateOrbitPosition();
            }
        }
	}

    protected void UpdateOrbitPosition()
    {
        Vector3 difference = this.orbitCenter.transform.position - this.torus.transform.position;
        this.torus.transform.position = this.orbitCenter.transform.position;
        this.transform.position = this.transform.position + difference;
        this.transform.RotateAround(this.orbitCenter.transform.position, this.orbitCenter.transform.up, orbitSpeed * Time.deltaTime);
        this.transform.Rotate(new Vector3(0, this.rotationSpeed * Time.deltaTime, 0));
    }
}
