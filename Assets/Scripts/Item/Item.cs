using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour {

    //public Sprite _image;
	private string _name;
    private int _count;
    private int _index;

    public new string name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    public int count
    {
		get
		{ 
			return _count;
		}
		set
		{
            _count = value;
		}
	}

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
}
