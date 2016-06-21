using UnityEngine;
using System.Collections;

public class Collect : MonoBehaviour {

    public SharedBox box;

    private const int MAX = 8; // 열쇠 종류 갯수
    private int _answer;       // 정답 인덱스
    private int _selectnum = 0;// 현재 선택된 인덱스
    private bool _collecting = false;
    public bool _cooling = false;
    private int _character;
    public const int COOLTIME = 1;

    public int character
    {
        get
        {
            return _character;
        }
        set
        {
            _character = value;
        }
    }

    #region Minigame

    /*
        게임 둘(열쇠 맞추기, 복불복 서랍열기)중에 랜덤으로 실행 
    */

    public void Setting()
    {
        _answer = Random.Range(0, 7); // Answer 0 ~ 7;
        // UI 배치
        GameStart();
    }

    public void GameStart()
    {
        _collecting = true;
    }

    public void Matching(int select)
    {
        if (_answer == select)
        {
            //Matching성공 정답
            reward(_character);
        }
        else {
            //Matching실패 쿨타임 ? 미니게임 종료?
            _cooling = true;
            StartCoroutine("Cool");
        }
    }

    /*
        쿨타임
    */
    IEnumerator Cool()
    {
        yield return new WaitForSeconds(COOLTIME);
        _cooling = false;
    }

    /*
        가족 중 딸일 경우 아이템 획득 양이 2배
    */

    public void reward(int family)
    {
        int reward_size = Random.Range(1, 4);                   // 1 ~ 4 랜덤으로 아이템 획득 개수 결정
        int reward_item = Random.Range(0, box._item_count - 1); // 아이템 종류 랜덤 선정

        if (family == (int)Data.Family.Daughter) reward_size *= 2; //딸일 경우 획득량 2배

        //Item instance = box.Inventory.;
        foreach (Item instance in box.Inventory)
        {
            if (instance.index == reward_item)
            {
                instance.count += reward_size;
                break;
            }
        }
  
        end();
    }

    public void end()
    {
        Destroy(this.gameObject);
    }

    #endregion
    // Use this for initialization

    void Start () {
        box = GameObject.Find("GameManager").GetComponent<SharedBox>();
        Setting();
	}
	
}
