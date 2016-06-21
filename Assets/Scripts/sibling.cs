using UnityEngine;
using System.Collections;

public class sibling : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.SetAsLastSibling();
	}

}
