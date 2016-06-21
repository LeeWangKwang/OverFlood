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
        //선택버튼을 눌렀을 때
        if(_collect.answer == select)
    }
}
