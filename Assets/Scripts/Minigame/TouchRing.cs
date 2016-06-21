using UnityEngine;
using System.Collections;

public class TouchRing : MonoBehaviour {

    public ParticleSystem touchParticle;
    public Repair repair;

	private int _index;
    private int _level;

    private int _rightup = 17;
    private Vector2 MinScale;
    private bool _smallring = true;
    private float _correct = 0.85f;
    private float _correct_ranage = 0.15f;
    private float _remove = 0.5f;
    private float _point;

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

    // Use this for initialization
    void Start() {
        transform.localScale = new Vector2(1.5f, 1.5f);
    }

    public void ValueSetting(int index, int level, Repair repair)
    {
        this.index = index;
        this.level = level;
        this.repair = repair;
    }

    // Update is called once per frame
    void Update () {

        MinScale = new Vector2(0.3f, 0.3f);

        transform.localScale = Vector2.Lerp(transform.localScale, MinScale, Time.deltaTime);
        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x + _rightup, transform.position.y + _rightup), Time.deltaTime*1.1f);
      
        if (transform.localScale.x <= _remove)
        {
            Destroy(transform.gameObject);
        }
    }

	public void Onclikme()
	{
        //touchParticle.transform.SetParent(null);
        touchParticle.transform.position = transform.position;
        touchParticle.gameObject.SetActive(true);
        touchParticle.Stop();
        touchParticle.Play();

        if (transform.localScale.x <= _correct + _correct_ranage && transform.localScale.x >= _correct - _correct_ranage)
		{
            // 오차 범위 내
            _point = Mathf.Abs(transform.localScale.x - _correct); // 위치와 정답의 차이
			Debug.Log ("CORRECT!!");
		} else 
		{
            //오차 범위 밖
			Debug.Log ("Fail!!");
            _point = 0;
		}

        if (index == level * 3) // 마지막 링
        {
            repair.reward(_point);
        }
        else {
            repair.point += _point;
        }

        Debug.Log("인덱스 : " + index);
		Destroy(transform.gameObject);
	}
}
