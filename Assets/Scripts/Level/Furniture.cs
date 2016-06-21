﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Furniture : NetworkBehaviour {

    public GameObject[] _gamelist = new GameObject[2]; 

    private GameObject _game;
    private bool _iscollecting = false;
    
    // Use this for initialization
    void Start () {
        float rand_num = Random.Range(0, 1);

        if (rand_num >= 0.5f)
            _game = _gamelist[0];
        else
            _game = _gamelist[1];
	}   
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D col)
    {
        //플레이어 확인 코드 추가
        if (Input.GetButton(PC2D.Input.JUMP))
        {
            if (!_iscollecting) // 이미 수리중이 아니다.
            {
                Instantiate(_game);
            }
        }
    }
}
