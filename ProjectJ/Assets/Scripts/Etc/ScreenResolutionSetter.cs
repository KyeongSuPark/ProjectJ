using UnityEngine;
using System.Collections;

public class ScreenResolutionSetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.SetResolution(1280, 720, true);
	}
}
