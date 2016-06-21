using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
[RequireComponent(typeof(PlatformerMotor2D))]
public class NetworkPlayerController2D : NetworkBehaviour
{
    private PlatformerMotor2D _motor;
    private int count = 0;

    // Use this for initialization
    void Awake()
    {
        _motor = GetComponent<PlatformerMotor2D>();
    }

    void Start()
    {
        // 로컬 만 실행
        if (isLocalPlayer && hasAuthority)
        {
            _motor.onMove += OnMove;
            _motor.onLanded += OnLand;
            _motor.onGroundJump += OnJump;
            _motor.onAirJump += OnJump;
            _motor.onDash += OnDash;
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;
       
        if (count++ % 1000 == 0)
        {
            if (isServer)
                RpcPositionSync(new Vector3(transform.position.x, transform.position.y, 0));
            else
                CmdPositionSync(new Vector3(transform.position.x, transform.position.y, 0));
            count = 0;
        }
    }

    void OnDestroy()
    {
        // 로컬 만 실행
        if (isLocalPlayer && hasAuthority)
        {
            _motor.onMove -= OnMove;
            _motor.onLanded -= OnLand;
            _motor.onGroundJump -= OnJump;
            _motor.onAirJump -= OnJump;
            _motor.onDash -= OnDash;
        }
    }

    void OnMove(float axis)
    {
        if(isServer)
            RpcOnMove(axis);
        else
            CmdOnMove(axis);
    }

    void OnLand()
    {
        if (isServer)
            RpcOnLand();
        else
            CmdOnLand();
    }

    void OnJump()
    {
        if (isServer)
            RpcOnJump();
        else
            CmdOnJump();
    }

    void OnDash()
    {
        if (isServer)
            RpcOnDash();
        else
            CmdOnDash();
    }

    //Command -> Client에서 Server로 요청 Host에서는 그냥 실행
    [Command]
	void CmdOnMove(float axis)
    {
        //if(!isClient) _motor.normalizedXMovement = axis;  Server에서 입력한 Client객체가 안움직임 -
        // 자기 자신을 제외한 다른플레이어들에게 OnMove RPC 호출
        Debug.Log("CmdOnLocalMove");
        _motor.normalizedXMovement = axis; 
         RpcPositionSync(new Vector3(transform.position.x, transform.position.y, 0));
        RpcOnMove(axis);
    }

    [Command]
    void CmdOnLand()
    {
        Debug.Log("CmdOnLocalLand");
        RpcPositionSync(new Vector3(transform.position.x, transform.position.y, 0));
        RpcOnLand();
    }

    [Command]
    void CmdOnJump()
    {
        Debug.Log("CmdOnLocalJump");
        _motor.Jump();
        RpcPositionSync(new Vector3(transform.position.x, transform.position.y, 0));
        RpcOnJump();
    }

    [Command]
    void CmdOnDash()
    {
        Debug.Log("CmdOnLocalDash");
        _motor.Dash();
        RpcPositionSync(new Vector3(transform.position.x, transform.position.y, 0));
        RpcOnDash();
    }

    [Command]
    void CmdPositionSync(Vector3 position)
    {
        transform.position = position;
        RpcPositionSync(position);
    }

    // 네트워크상의 다른 플레이어의 움직임이 변화됐을 경우
    // 호출되며, 여기서 그 변화를 모터에 적용
    // ClientRPC -> Server에서도 실행이 된다.

    [ClientRpc]
    void RpcOnMove(float axis)
	{
        if (isServer || hasAuthority || isLocalPlayer) return;
        Debug.Log("RpcOnMove");
        _motor.normalizedXMovement = axis;
    }

    [ClientRpc]
    void RpcOnJump()
	{
        if (isServer || hasAuthority || isLocalPlayer) return;
        Debug.Log("RpcOnJump");
        _motor.Jump ();
    }

    [ClientRpc]
    void RpcOnLand()
    {
        if (isServer || hasAuthority || isLocalPlayer) return;
        Debug.Log("RpcOnLand");
    }

    [ClientRpc]
    void RpcOnDash()
	{
        if (isServer || hasAuthority || isLocalPlayer) return;
        Debug.Log("RpcOnDash");
        _motor.Dash ();
    }

    [ClientRpc]
    void RpcPositionSync(Vector3 position)
    {
        if (isServer || hasAuthority || isLocalPlayer) return;
        transform.position = position;
    }
}
