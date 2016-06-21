using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SharedBox : MonoBehaviour {

    public int _item_count = 4;
    /*
    private int _board;
    private int _hammer;
    private int _nail;
    private int _key;
    */
    private Item _hammer = new Hammer();
    private Item _nail = new Nail();
    private Item _key = new Key();

    public Item[] Inventory = new Item[4];
    /*
    #region Property

    public int board
    {
        get
        {
            return _board;
        }
        set
        {
            _board = value;
        }
    }

    public int hammer
    {
        get
        {
            return _hammer;
        }
        set
        {
            _hammer = value;
        }
    }

    public int nail
    {
        get
        {
            return _nail;
        }
        set
        {
            _nail = value;
        }
    }


    public int key
    {
        get
        {
            return _key;
        }
        set
        {
            _key = value;
        }
    }
   
    #endregion
    */
    // Use this for initialization
    void Start()
    {
        Inventory[0] = _nail;
        Inventory[1] = _hammer;
        Inventory[2] = _key;
    }
}
