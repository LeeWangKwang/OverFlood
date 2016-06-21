using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{

    public Slider healthBarSlider;  //reference for slider
    public Text gameOverText;   //reference for text
    private bool isGameOver = false; //flag to see if game is over
    float healthpoint;
    int unclearedObject = 5; //교체필요
    int maxhealthpoint;

    void Start()
    {
        maxhealthpoint = 1000;
        gameOverText.enabled = false; //disable GameOver text on start
        healthpoint = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //check if game is over i.e., health is greater than 0
        if (!isGameOver)
        {
            healthpoint -=  unclearedObject*Time.deltaTime*10f;
            healthBarSlider.value = (float)healthpoint/maxhealthpoint;
        }
        if (healthBarSlider.value <= 0)
        {
            GameOver();
        }
    }



    void GameOver()
    {
        isGameOver = true;
        gameOverText.enabled = true;

    }
}
