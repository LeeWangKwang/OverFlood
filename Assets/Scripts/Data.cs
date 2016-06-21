using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {

    public enum Family
    {
        Mother,
        Father,
        Daughter,
        Son
    }

    public enum Ability
    {
        Action,
        Power,
        Dash,
        Equip
    }

    public enum State
    {
        Default,
        Repair,
        Collect,
        Dash,
        Power,
        Equip,
        Slow
    }

    public enum ItemList
    {
        Nail,
        Hammer,
        Key
    }

}
