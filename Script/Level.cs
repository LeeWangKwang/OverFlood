using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level : MonoBehaviour {
    public Slider LevelSlider;  //reference for slider
    public Text LevelupText;   //reference for text
    private bool isLevelup = false; // 레벨업 텍스트 표시
    int EXEpoint;
    int clearedObject = 100; // 교체필요
    int maxEXEpoint;

    int a;

    void Start()
    {
        maxEXEpoint = 100;
        LevelupText.enabled = false; //레벨업 텍스트 안보이게함
        EXEpoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //check if game is over i.e., health is greater than 0
        if (!isLevelup)
        {
            EXEpoint = clearedObject;
            LevelSlider.value = (float)EXEpoint / maxEXEpoint;
        }
        if (LevelSlider.value == 1)
        {
            Levelup();
 /*           while (true) {//일정시간동안 기다리게하는 메소드?
                a ++;
                if(a == 5)
                {
                    break;
                }
            }*/
            isLevelup = false;
            LevelupText.enabled = false;
        }
    }

    void ClearObject()
    {
        clearedObject++;
    }

    void Levelup()
    {
        isLevelup = true;
        LevelupText.enabled = true;

    }


}
