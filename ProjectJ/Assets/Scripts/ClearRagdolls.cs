using UnityEngine;
using System.Collections;

public class ClearRagdolls: MonoBehaviour {
    public int maxRagdolls;
    public GameObject rag;

	// Use this for initialization
	void Start () 
    {
        InvokeRepeating("ClearRags", 1, 1);
	}

    void ClearRags() 
    {
        int counter = 0;
        foreach(var fooObj in GameObject.FindGameObjectsWithTag("Player")) 
        {
            if (fooObj.GetComponent<Rigidbody>().useGravity == true)
            {
                if (!rag)
                {
                    rag = fooObj;
                    counter++;
                }
            }

            if(maxRagdolls < counter)
            {
                Destroy(rag);
            }
        }
    }
}
