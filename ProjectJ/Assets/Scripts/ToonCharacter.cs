using UnityEngine;
using System.Collections;

public class ToonCharacter : MonoBehaviour {

    private Rigidbody[] boneRig;        //. contains the ragdool bones
    private float mass = 0.1f;           //. mass of each bone

    public Transform projector;         //. shadow projector
    public Transform root;              //. assign the root bone to position the shadow projector
    public Color _bloodColor;
    public GameObject _model;
    public Mesh _bodyMesh;

    public ParticleSystem _explodeHeadPS;
    public GameObject _head;
    public Transform _headBone;

    public GameObject[] _disableWhenDecapitated;
    public ParticleSystem _bodyPS;

    private bool _decapitated;

    //. blinking
    private Color colorOriginal;
    private Color color;
    private float _R = 2500;
    private float _G = 2500;
    private float _B = 2500;

    private bool _randomColor;
    private int _blinkCounter;
    private int _stopBlink;

    private Collider collider;

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();

        if (!root)
            root = transform.FindChild("Root");

        if(!projector)
 	        projector = transform.FindChild("Blob Shadow Projector");

 	    if(!_model)
 	        _model = transform.FindChild("MicroMale").gameObject;

 	    if(!_headBone)
 	        _headBone = transform.FindChild("Head");

	    boneRig = gameObject.GetComponentsInChildren<Rigidbody>(); 
	    disableRagdoll();

	    //Blinking
	    colorOriginal = _model.GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        if(collider.enabled == false && projector && root)
        {
            projector.transform.position = new Vector3(root.position.x, 0.0f, root.position.z);
        }
    }

    void Blink(int times, float speed, float red, float green, float blue)
    {
        CancelInvoke();
        _randomColor = false;
        _R = red;
        _G = green;
        _B = blue;
        _stopBlink = times;
        InvokeRepeating("BlinkInvoke", speed, speed);
    }

    void Blink(int times, float speed)
    {
        CancelInvoke();
        _randomColor = true;
        _stopBlink = times;
        InvokeRepeating("BlinkInvoke", speed, speed);
    }

    void disableRagdoll()
    {
        foreach (var ragdoll in boneRig)
        {
            Collider col = ragdoll.GetComponent<Collider>();
            if(col && col != this.GetComponent<Collider>())
            {
                col.enabled = false;
                ragdoll.isKinematic = true;
                ragdoll.mass = 0.01f;
            }
        }
    }

    IEnumerator enableRagdoll(float delay, Vector3 force)
    {
        yield return new WaitForSeconds(delay);
        foreach(var ragdoll in boneRig)
        {
            Collider col = ragdoll.GetComponent<Collider>();
            if(col)
            {
                col.enabled = true;
                ragdoll.isKinematic = false;
                ragdoll.mass = mass;
                if(force.magnitude > 0)
                    ragdoll.AddForce(force * Random.value);
            }
        }

        GetComponent<Animator>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(GetComponent("PlayerController"));
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        for(int i = 0; i < this._disableWhenDecapitated.Length; ++i)
        {
            _disableWhenDecapitated[i].SetActive(false);
        }

    }

    void BlinkInvoke()
    {
        if (_blinkCounter < _stopBlink)
        {
            if (_randomColor)
            {
                color = new Color(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5), 1);
            }
            else
            {
                color = new Color(_R, _G, _B, 1);
            }

            if (_model.GetComponent<Renderer>().material.color == colorOriginal)
            {
                _model.GetComponent<Renderer>().material.color = color;
            }
            else
            {
                _model.GetComponent<Renderer>().material.color = colorOriginal;
            }

            _blinkCounter++;
        }
        else
        {
            _model.GetComponent<Renderer>().material.color = colorOriginal;
            _blinkCounter = 0;
            CancelInvoke();
        }
    }

    void Decapitate(bool explode, float delay, Vector3 force)
    {
        if(!_decapitated)
        {
            _decapitated = true;
            _model.GetComponent<SkinnedMeshRenderer>().sharedMesh = this._bodyMesh;

            if(_head)
            {
                if(explode == false)
                {

                }
            }
            enableRagdoll(delay, force);
        }
    }

    IEnumerator EnableCollisions(Collider c1, Collider c2)
    {
        yield return new WaitForSeconds(1.0f);
        if(c2 && c1.enabled)
            Physics.IgnoreCollision(c1, c2, false);
    }

}
