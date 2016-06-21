using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int[] item; // 아이템 갯수
    public Text[] text;


    void AddItem(int id)
    {   // 아이템의 갯수를 증가시킨다. 
        item[id]++;
    }
    void RemoveItem(int id)
    {   // 아이템의 갯수를 감소시킨다.
        item[id]--;
    }

    void DisplayItem()
    {
        for(int j = 0; j<text.Length; j++)
        {
            text[j].GetComponent<Text>().text = item[j].ToString();
        }
    }


    void Update()
    {
        DisplayItem();
    }

}


