using UnityEngine;
using System.Collections;

public class Collect_Key : MonoBehaviour {

    Collect _collect;
    public int select; // 셀렉트를 여기껏을 사용할지 _collect의 것을 사용할지 UI에따라 고민해보자.

    void Start()
    {
        _collect = GetComponent<Collect>();
    }

    public void OnClick()
    {
        if (!_collect._cooling) { // 쿨 상태가 아니면
            //선택버튼을 눌렀을 때 선택된 열쇠 값을 파라미터로 Collect에 matching()호출
            _collect.Matching(select);
        } else
        {

        }
    }
}
