using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    { 
        if (col.tag.Equals("Player"))
        {
            if (col.GetComponent<PlayerAttribute>()._current_Ability
                == (int)Data.Ability.Power) // (지금은 능력을 가졌을 때) 이 능력을 사용하였을 때
            {
                //Obstacle이 부숴지고 GameManager의 LevelManager에게 알린다음 일정시간 뒤 재스폰
                Destroy(gameObject);
            }
            else {
                if (Input.GetKey(KeyCode.RightArrow))
                    col.GetComponent<PlayerAttribute>().ChangeState((int)Data.State.Slow, 'R');
                else if (Input.GetKey(KeyCode.LeftArrow))
                    col.GetComponent<PlayerAttribute>().ChangeState((int)Data.State.Slow, 'L');

                //느려진다. 
            }
        }
    }
}
