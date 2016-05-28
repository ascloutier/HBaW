using UnityEngine;
using System.Collections;

public class PlayerObject : Photon.MonoBehaviour
{

	public string owner;
	public PlanetType type;
	
	public float selectionScale = 1.0f;

	protected Vector3 realPosition = Vector3.zero;
	protected Quaternion realRotation = Quaternion.identity;
	protected Vector3 realScale = Vector3.one;

	// Use this for initialization
	protected virtual void Start ()
	{
		//MeshRenderer mesh = this.GetComponentInChildren<MeshRenderer> ();
		Collider mesh = this.GetComponentInChildren<Collider> ();
		
		this.selectionScale = mesh.bounds.size.x + 0.3f;
		
		Debug.Log ("Selection Scale: " + this.selectionScale);
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
	    if( photonView )
        {
            if( !photonView.isMine )
            {
                transform.position = Vector3.Lerp(transform.position, realPosition, 0.3f);
                transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.3f);
                transform.localScale = Vector3.Lerp(transform.localScale, realScale, 0.3f);
            }
        }
	}
		
	public virtual void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
            // my stuff on my computer
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(transform.localScale);
            stream.SendNext(this.type);
            stream.SendNext(this.owner);

		}
		else
		{
            // my stuff on another computer
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
			realScale = (Vector3)stream.ReceiveNext();
            this.type = (PlanetType)stream.ReceiveNext();
            this.owner = (string)stream.ReceiveNext();
		}
	}
}