using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Durability : NetworkBehaviour {

    //  Healthbar
    public Slider healthBarSlider;
    public Text GameEnd;
    private bool isOver; //게임 오버
    //

    public int _startHP = 80000;
    public float _time = 0.5f;

    [SyncVar(hook = "OnHPChanged")]
    public int _currentHP;

    [SyncVar]
    public int _decrement = 1;

    NetworkGameManager ngm;

    public int currentHP
    {
        get
        {
            return _currentHP;
        }
        set
        {
            _currentHP = value;
        }
    }

	void Start () {

        ngm = GetComponent<NetworkGameManager>();
        _currentHP = _startHP;
        StartCoroutine("Break");
	}

    [Server]
    void Decrement()
    {
        _currentHP -= _decrement;
        healthBarSlider.value = (float)_currentHP/_startHP;
       // Debug.Log("현재 내구도 : " + (float)_currentHP / _startHP);
        if (_currentHP <= 0) Debug.Log("GameOver");
    }

    void OnHPChanged(int newValue)
    {
        _currentHP = newValue;
        //UI변경
    }

    IEnumerator Break()
    {
        while (ngm._decremeting)
        {
            yield return new WaitForSeconds(_time);
            Decrement();
        }
    }


}
