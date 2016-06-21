using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    SharedBox _shared_box;
    BoxCollider2D _realDoor;
    private bool _isopen = false;
    private bool _isDaughter = false;

	// Use this for initialization
	void Start () {
        _shared_box = GameObject.Find("GameManager").GetComponent<SharedBox>();
        _realDoor = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
	   
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isopen)
        { 
            if (col.gameObject.tag == "Player")
            {
                if (col.gameObject.GetComponent<PlayerAttribute>()._current_Family == (int)Data.Family.Daughter)
                {
                    //문 열리는 애니메이션
                    Debug.Log("Daughter");
                    _isDaughter = true;//딸 일때 Exit일 때까지 isopen = true;
                    _realDoor.enabled = false;
                }

                if (_shared_box.Inventory[(int)Data.ItemList.Key].count >= 1)
                {
                    // 문을 열었을 때 일어나는 이벤트
                    _shared_box.Inventory[(int)Data.ItemList.Key].count--;
                    _isopen = true;
                    _realDoor.enabled = false;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (_isDaughter)
        {
            //문 닫히는 애니메이션
            _isDaughter = true;
        }

        _realDoor.enabled = true;
    }
}
