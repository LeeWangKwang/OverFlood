using UnityEngine;
using System.Collections;

public class Collect_luckyObject : MonoBehaviour {

    Collect_lucky lucky; 
    private int _index;

    public int index
    {
        get
        {
            return _index;
        }
        set
        {
            _index = value;
        }
    }

    void Awake() {
        lucky = GameObject.Find("collect_Background").GetComponent<Collect_lucky>();
    }

    /*
        서랍을 열었을 때 
    */
    public void OnClick()
    {
        if(!lucky.cooling)
            lucky.Matching(index);
    }
}
