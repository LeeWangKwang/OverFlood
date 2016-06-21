using UnityEngine;
using System.Collections;

public class Collect_lucky : MonoBehaviour {

    public const int MAX = 3;
    private bool _isgaming = false;
    private bool _cooling = false;
    private int _answer;

    public bool cooling
    {
        get { return _cooling; }
        set { _cooling = value; }
    }

    #region Minigame

    //N개의 서랍중 N번째에 아이템을 숨김
    public void Setting()
    {
        _answer = (int)Random.Range(0, MAX+0.99f); //N개의 서랍중 N번째에 아이템을 숨김
        GameStart();
    }

    public void GameStart()
    {
        _isgaming = true;
    }

    public void Matching(int select)
    {
        if (_answer == select)
        {
            // 정답처리
        }
        else
        {
            //오답처리
        }
    }

    public void reward(float point)
    {
        /*
        this.point += point; // 마지막 링의 점수 까지 합산
        float reward = point * _level * 50; // [ Point * Level * 50 ]

        durability.currentHP += (int)reward;  //점수 계산 후 보상을 준다.
        Debug.Log("수리 보상 : 체력이 " + reward + "증가!");
        */
        end();
    }

    public void end()
    {
        Destroy(this.gameObject);
    }

    #endregion
}
