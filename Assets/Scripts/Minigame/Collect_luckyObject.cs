using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collect_luckyObject : MonoBehaviour {

    private bool _isclicked = false;

    Collect_lucky lucky;
    public Sprite wrong_answer;
    public Sprite answer;

    void Awake() {
        lucky = GameObject.Find("collect_Background").GetComponent<Collect_lucky>();
    }

    /*
        서랍을 열었을 때 
    */

    public void OnClickme(int index)
    {
        if (!_isclicked)
        {
            if (!lucky.cooling)
            {
                Debug.Log("You Click number : " + index);
                Image image = GetComponent<Image>();
    
                if (lucky.Matching(index))
                {
                    Debug.Log("AWSOOOOOM!");
                    image.sprite = answer;
                }
                else
                {
                    this.gameObject.GetComponent<Image>().sprite = wrong_answer;
                }
                transform.position = new Vector2(transform.position.x, transform.position.y - 15);
                transform.localScale = new Vector2(1.1f, 1.5f);
            }
            _isclicked = true;
        }
    }
}
