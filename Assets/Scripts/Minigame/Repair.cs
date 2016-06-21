using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/**
    Park Hyun Seo
*/

public class Repair : MonoBehaviour{

    LinkedList<Vector2> _ring_position = new LinkedList<Vector2>();

    /*
        수리게임 포인트 : 터치 시간의 오차도에 따른 점수
        level(1,2,3)        : 시간에 따른 난이도      [ L ]
        ring_count          : 난이도에 따른 Ring 갯수 [ L * 3 ]
        touch_error         : 오차                   [ R ]
        _point              : 최종 포인트             [ L * 3 * 50 ] 
    */

    [HideInInspector]
    public Durability durability;
    [HideInInspector]
    public GameObject Ring;

    public GameObject Nailup;
    [HideInInspector]
    public Vector2 Ring_Size;
    //_minX = -340 (_maxX*-1.55f)
    public int _maxX = 220;
    //_minY = -220 (_maxY*-2)
    public int _maxY = 110;

    private int _level = 2;
    private float _point = 0;
    private float _gamespeed = 1.0f;
    private int _ring_term = 40;

    public float point
    {
        get
        {
            return _point;
        }
        set
        {
            _point = value;
        }
    }

    #region Minigame

    public void Setting()
    {
        for (int i = 0; i < _level * 3; i++)
        {
            //랜덤 위치 받기
            Vector2 rand_offsetmin = new Vector2(UnityEngine.Random.Range(_maxX * -1.55f, _maxX), UnityEngine.Random.Range(_maxY * -2, _maxY));

            //링의 포지션 위치 리스트원소만큼 반복문돌려 중복검사

            // 중복검사
            RePlay:
            foreach (Vector2 position in _ring_position)
            {                       
                    // 전체
               if (Math.Abs(position.x - rand_offsetmin.x) <= _ring_term || Math.Abs(position.y - rand_offsetmin.y) <= _ring_term)
               {
                 rand_offsetmin = new Vector2(UnityEngine.Random.Range(_maxX * -1.55f, _maxX), UnityEngine.Random.Range(_maxY * -2, _maxY));
                 goto RePlay;
               }            
            }           

            //최종 랜덤 포지션
            _ring_position.AddLast(rand_offsetmin);
            Debug.Log(rand_offsetmin);
        }
        GameStart();
    }

    public void GameStart()
    {
        StartCoroutine(Spawn_Ring(_level));
    }

    public void reward(float point)
    {
        this.point += point; // 마지막 링의 점수 까지 합산
        float reward = point * _level * 50; // [ Point * Level * 50 ]

        durability.currentHP += (int)reward;  //점수 계산 후 보상을 준다.
        Debug.Log("수리 보상 : 체력이 " + reward + "증가!");
        end();
    }

    public void end()
    {
        Destroy(this.gameObject);
        //gameObject.SetActive(false); // 미니게임을 안 보이게 한다.
        //
    }

    #endregion

    // Use this for initialization
    void Start () {
    
        durability = GameObject.Find("GameManager").GetComponent<Durability>();
        Ring_Size = new Vector2(100, 100);

        Setting();

        //MinPosition은 위치
        //MaxPosition은 크기
        //Width = Max -Min, Height = Max - Min [일정해야한다.]
        //PosX = Min + Scale, PosY = Max + Scale
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Spawn_Ring(int level)
    {
        /*
            Setting된 Ring의 Position에 따라 Spawn
        */
		int count = 0;

        foreach (Vector2 position in _ring_position)
        {
            yield return new WaitForSeconds(_gamespeed);
            GameObject nail = Instantiate(Nailup) as GameObject;
			GameObject ring = Instantiate(Ring) as GameObject;

            nail.transform.parent = transform;
            nail.GetComponent<RectTransform>().offsetMin = position;
            nail.GetComponent<RectTransform>().offsetMax = position + Ring_Size;

            ring.transform.parent = transform;
            ring.GetComponent<RectTransform>().offsetMin = position;
            ring.GetComponent<RectTransform>().offsetMax = position + Ring_Size;
            TouchRing touch = ring.GetComponent<TouchRing>();
            touch.ValueSetting(++count, level, this);
            touch.nailup = nail;
		}
    }

}
