using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;


public class Timer : MonoBehaviour {

    float TimeRemaining;
    public Text remainingtxt;
    


	// Use this for initialization
	void Start () {
        TimeRemaining = 300;

	
	}
	
	// Update is called once per frame
	void Update () {
        TimeRemaining -= Time.deltaTime;
        int tt = (int)TimeRemaining;
        GetComponent<Text>().text = Convert.ToString(tt);
    }
}
