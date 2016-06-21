using UnityEngine;
using System.Collections;

public class Collect_lucky : MonoBehaviour {

    public SharedBox box;

    public const int MAX = 3;
    private bool _isgaming = false;
    private bool _cooling = false;
    private int _answer;
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

    public bool cooling
    {
        get
        {
            return _cooling;
        }
        set
        {
            _cooling = value;
        }
    }

    #region Minigame

    //N개의 서랍중 N번째에 아이템을 숨김
    public void Setting()
    {
        _answer = (int)Random.Range(0, MAX-0.01f); //N개의 서랍중 N번째에 아이템을 숨김
        Debug.Log("정답은 : " + _answer);
        // UI 배치
        GameStart();
    }

    public void GameStart()
    {
        _isgaming = true;
    }

    public bool Matching(int select)
    {
        if (_answer == select)
        {
            // 정답처리
            //reward(_character);
            reward(1);
            return true;
        }
        else
        {
            //오답처리
            _cooling = true;
            StartCoroutine("Cool");
            return false;
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

        foreach (Item instance in box.Inventory)
        {
            if (instance.index == reward_item)
            {
                instance.count += reward_size;
                break;
            }
        }

        //end();
    }

    public void end()
    {
        Destroy(this.gameObject);
    }

    #endregion

    void Start()
    {
        box = GameObject.Find("GameManager").GetComponent<SharedBox>();
        Setting();
    }
}
