using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//[RequireComponent(BoxCollider2D)]
public class Water : NetworkBehaviour {

    public GameObject _game;

    SharedBox box;
    public ParticleSystem[] water_level = new ParticleSystem[3]; 

    private int _level = 1;
    private bool _isreparing = false;

    public int level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
        }

    }

    void Awake() {
        water_level[0].Play();
        water_level[1].Stop();
        water_level[2].Stop();
    }

	// Use this for initialization
	void Start () {
        box = GameObject.Find("GameManager").GetComponent<SharedBox>();

        /*
            일정시간이 지나면 Level이 올라간다.
            Level 1 -> Level 2 8초
            Level 2 -> Level 3 12초
        */
        StartCoroutine("Levelup");
    }

    IEnumerator Levelup()
    {

        yield return new WaitForSeconds(8); // Level 1 -> Level 2
        water_level[0].Stop();
        water_level[1].Play();
        level = 2;


        yield return new WaitForSeconds(12); // Level 2 -> Level 3
        water_level[1].Stop();
        water_level[2].Play();
        level = 3;
    }
	
    void OnTriggerStay2D(Collider2D col)
    {
        //플레이어 확인 코드 추가
        Debug.Log("TOUCH********************************8");
        if (Input.GetButton(PC2D.Input.ACTION))
        {
            if (!_isreparing) // 이미 수리중이 아니다.
            { 
                if (box.Inventory[(int)Data.ItemList.Nail].count >= _level * 3) // 못이 level의 3배 이상개 존재한다.
                {
                    if (col.GetComponent<PlayerAttribute>()._current_Family == (int)Data.Family.Father 
                        || col.GetComponent<PlayerAttribute>()._current_Family == (int)Data.Family.Son) // 아버지나 아들이다.
                    {
                        Debug.Log("수리중");
                        _isreparing = true;
                        //_game.SetActive(true);
                        Instantiate(_game);
                    }
                    else // 아빠와 아들 (수리특화)가 아니기 때문에 망치가 필요하다.
                    {
                        if (box.Inventory[(int)Data.ItemList.Hammer].count >= 1)
                        {
                            box.Inventory[(int)Data.ItemList.Hammer].count--;
                            Debug.Log("수리중");
                            _isreparing = true;
                           // _game.SetActive(true);
                           //_game.GetComponentInChildren<Repair>().Setting();
                            Instantiate(_game);
                           
                        }else
                        {
                            // 망치가 부족하다는 안내 [ 소리 + 메세지]
                        }
                    }
                }else 
                {
                    //못이 부족하다는 안내  [소리 + 메세지]
                }
            }
        }
    }

}
