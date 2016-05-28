using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sun : PlayerObject {

    public Color sunColor;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    public void SetColor( Color aColor )
    {
        this.sunColor = aColor;
        //Debug.Log(this.sunColor.ToString());

        MeshRenderer myMesh = this.GetComponentInChildren<MeshRenderer>();

        myMesh.materials[0].EnableKeyword("_EMISSION");
        myMesh.materials[0].color = aColor;
        myMesh.materials[0].SetColor("_EmissionColor", aColor);
    }

    public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info )
    {
        if( stream.isWriting)
        {
            Dictionary<string,float> serialColor = new Dictionary<string,float>();
            serialColor.Add("r", this.sunColor.r);
            serialColor.Add("g", this.sunColor.g);
            serialColor.Add("b", this.sunColor.b);
            serialColor.Add("a", this.sunColor.a);

            stream.SendNext(serialColor);
        }
        else
        {
            Dictionary<string, float> serialColor = (Dictionary<string, float>)stream.ReceiveNext();
            this.SetColor(new Color(serialColor["r"],serialColor["g"],serialColor["b"],serialColor["a"]));
        }
        base.OnPhotonSerializeView(stream, info);
    }
}
