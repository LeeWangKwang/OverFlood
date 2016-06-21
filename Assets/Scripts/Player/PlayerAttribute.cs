using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttribute : MonoBehaviour {

    public string _Player_Name;
    public Sprite _character;

    public float _slow = 4;
    public int _slowtime = 2;
		
	public int _current_Family;
	public int _current_Ability;
    public ArrayList _current_State = new ArrayList();

    void Awake()
    {
        //테스팅 가족 설정
        _current_Family = (int)Data.Family.Daughter;
        //_current_Ability = (int)Data.Ability.Power;
        _current_State.Add(Data.State.Default);
        //캐릭터 렌더링
        //네트워크 매니저 생성 (init -> 화면구성 및 레벨설정)
        //_update(2);
    }

    /*
        Default,
        Repair,
        Collect,
        Dash,
        Power,
        Equip,
        Slow
    */

    public void ChangeState(int state, char additional)
    {
        Debug.Log(state);
        switch (state)
        {
            case 0:
                if (!_current_State.Contains(Data.State.Default))
                {
                    _current_State.Add(Data.State.Default);
                }
                break;
            case 1:
                if (!_current_State.Contains(Data.State.Repair))
                {
                    _current_State.Add(Data.State.Repair);
                }
                break;
            case 2:
                if (!_current_State.Contains(Data.State.Collect))
                {
                    _current_State.Add(Data.State.Collect);
                }
                break;
            case 3:
                if (!_current_State.Contains(Data.State.Dash))
                {
                    _current_State.Add(Data.State.Dash);
                }
                break;
            case 4:
                if (!_current_State.Contains(Data.State.Power))
                {
                    _current_State.Add(Data.State.Power);
                }
                break;
            case 5: if (!_current_State.Contains(Data.State.Equip))
                    {
                        _current_State.Add(Data.State.Equip);
                    }
                    break;
            case 6: if (!_current_State.Contains(Data.State.Slow))
                    {
                        _current_State.Add(Data.State.Slow);
                        StartCoroutine(Slow(additional));
                    }
                    break;
        }
        if (state == 6) Slow(additional);
    }

    //Slow상황이 일어났을 때

    IEnumerator Slow(char additional)
    {
        Debug.Log("Slow");
        // 저장해둘 사항
        float temp = GetComponent<PlatformerMotor2D>().groundSpeed;

        // 달라지는 사항
        if (additional == 'R')
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 30);
        else if(additional == 'L')
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30);

        GetComponent<PlatformerMotor2D>().groundSpeed -= _slow;

        yield return new WaitForSeconds(_slowtime);

        GetComponent<PlatformerMotor2D>().groundSpeed = temp;
        _current_State.Remove(Data.State.Slow);
    }

    //ClientCallback으로 캐릭터 위치 조정
    //스킬같은거나 점프같은거 local플레이어가 누르면 Cmd로 실행
    //Collision이 발생해도 ClientCallback
    //Client -> 정말 Client에서 실행하는것
    //Server -> 정말 Server에서 실행하는것 [포인트가 깎이거나 점수가 깎이는것]
    //유저가 action키 클릭 -> cmdFire(!isClient -> 총알생성) -> RpcFire(총알생성)
    //Respawn [Server] -> RpcRespawn() [ClientRpc] Event에의해 실행 
}
