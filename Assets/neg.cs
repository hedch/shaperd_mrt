using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class neg : MonoBehaviour {


    protected GameObject objCamera = new GameObject();
    // Use this for initialization
    void Start () {
        objCamera = GameObject.Find("Main Camera");
    }

	
	// Update is called once per frame
	void Update () {


    transform.rotation = Quaternion.Inverse(objCamera.transform.localRotation);
    transform.position = (-1) * objCamera.transform.localPosition;
    }
}
