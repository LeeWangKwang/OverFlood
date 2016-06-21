using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour {

    //Public 변수
    public RectTransform panel; // ScrollPanel 고정
    public GameObject[] bttn;
    public RectTransform center;

    //Private 변수
    private float[] distance; // 버튼들의 센터까지 거리
    private bool dragging = false; // 마우스가 드래그되고 있는지 확인
    private int bttnDistance; // 버튼간의 거리 고정
    private int minButtonNum; // 센터에서 가장 가까운 버튼의 번호

    void Start()
    {
        int bttnLength = bttn.Length;
        distance = new float[bttnLength];

        //버튼간의 거리
        bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x  - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);

    }

    void Update()
    {
        for (int i = 0; i< bttn.Length; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);

        }

        float minDistance = Mathf.Min(distance);

        for(int a = 0; a< bttn.Length; a++)
        {
            if (minDistance == distance[a])
            {
                minButtonNum = a;
                if(bttn[minButtonNum].transform.localScale != new Vector3(2, 2, 0))
                {
                    SizeUp(minButtonNum);
                }
               
            }
        }

        if (!dragging)
        {

            LerpToBttn(minButtonNum * -bttnDistance);


        }
        if (dragging)
        {
            SizeDown();


        }
    }

    void LerpToBttn(int position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 10f);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    void SizeUp(int minButtonNum)
    {
        
        float newX = Mathf.Lerp(bttn[minButtonNum].transform.localScale.x, 2.0f, Time.deltaTime * 10f);
        Vector3 newSize = new Vector3(newX, newX, 0);
        bttn[minButtonNum].transform.localScale = newSize;
        
        
    }
    void SizeDown()
    {
        for(int i = 0; i < bttn.Length; i++)
        {
            if (minButtonNum != i) {
                bttn[i].transform.localScale = new Vector2(1, 1);
            }
            
        }
    }



}
