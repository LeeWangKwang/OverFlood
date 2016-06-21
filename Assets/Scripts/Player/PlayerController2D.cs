using UnityEngine;
using UnityEngine.Networking;
using System;

/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
[RequireComponent(typeof(PlatformerMotor2D))]
public class PlayerController2D : NetworkBehaviour
{
    private PlatformerMotor2D _motor;
    private bool _restored = true;
    private bool _enableOneWayPlatforms;
    private bool _oneWayPlatformsAreWalls;
    private bool _ignoreActionKey = true;

    /// <summary>
    /// mask with all layers that the player should interact with
    /// </summary>
    public LayerMask platformMask = 0;

    /// <summary>
    /// mask with all layers that trigger events should fire when intersected
    /// </summary>
    public LayerMask triggerMask = 0;

    /// <summary>
    /// mask with all layers that should act as one-way platforms. Note that one-way platforms should always be EdgeCollider2Ds. This is because it does not support being
    /// updated anytime outside of the inspector for now.
    /// </summary>
    //[SerializeField]
    //LayerMask oneWayPlatformMask = 0;


    void Awake()
    {
        //Triiger Mask
        for (var i = 0; i< 32; i++)
        {
            // see if our triggerMask contains this layer and if not ignore it
            if ((triggerMask.value & 1 << i) == 0)
                Physics2D.IgnoreLayerCollision(gameObject.layer, i);
        }
    }

// Use this for initialization
    void Start()
    {
        _motor = GetComponent<PlatformerMotor2D>();

        if (hasAuthority || isLocalPlayer) // 카메라 설정
        {
            GameObject cam = GameObject.Find("Main Camera");

            if (GetComponent<PlayerAttribute>()._current_Family==(int)Data.Family.Mother)
            {
                cam.GetComponent<Camera>().fieldOfView = 120;
                Debug.Log(GetComponent<PlayerAttribute>()._current_Family);
                Debug.Log((int)Data.Family.Mother);
                Debug.Log("Mother");
            }
            else {
                cam.AddComponent<SmoothFollow>().target = gameObject;
                Debug.Log("Others");
            }
        }

        _motor.onTriggerEnterEvent += onTriggerEnterEvent;
        _motor.onTriggerExitEvent += onTriggerExitEvent;
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D col)
    {
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }


    // before enter en freedom state for ladders
    void FreedomStateSave(PlatformerMotor2D motor)
    {
        if (!_restored) // do not enter twice
            return;

        _restored = false;
        _enableOneWayPlatforms = _motor.enableOneWayPlatforms;
        _oneWayPlatformsAreWalls = _motor.oneWayPlatformsAreWalls;
    }
    // after leave freedom state for ladders
    void FreedomStateRestore(PlatformerMotor2D motor)
    {
        if (_restored) // do not enter twice
            return;

        _restored = true;
        _motor.enableOneWayPlatforms = _enableOneWayPlatforms;
        _motor.oneWayPlatformsAreWalls = _oneWayPlatformsAreWalls;
    }

    // Update is called once per frame
    void Update()
    {
        // 로컬플레이어가 아니면 조종할 수 없다.
        if (!isLocalPlayer) return;

        // use last state to restore some ladder specific values
        if (_motor.motorState != PlatformerMotor2D.MotorState.FreedomState)
        {
            // try to restore, sometimes states are a bit messy because change too much in one frame
            FreedomStateRestore(_motor);
        }

        // Jump?
        // If you want to jump in ladders, leave it here, otherwise move it down
        if (Input.GetButtonDown(PC2D.Input.JUMP))
        {
            _motor.Jump();
            _motor.DisableRestrictedArea();
        }

        _motor.jumpingHeld = Input.GetButton(PC2D.Input.JUMP);

        // XY freedom movement
        if (_motor.motorState == PlatformerMotor2D.MotorState.FreedomState)
        {
            _motor.normalizedXMovement = Input.GetAxisRaw(PC2D.Input.HORIZONTAL);
            _motor.normalizedYMovement = Input.GetAxisRaw(PC2D.Input.VERTICAL);
            return; // do nothing more
        }

        // X axis movement
        if (Mathf.Abs(Input.GetAxis(PC2D.Input.HORIZONTAL)) > PC2D.Globals.INPUT_THRESHOLD)
        {
            _motor.normalizedXMovement = Input.GetAxis(PC2D.Input.HORIZONTAL);
            //CmdOnMove(Input.GetAxis(PC2D.Input.HORIZONTAL));
        }
        else
        {
            _motor.normalizedXMovement = 0;
            //CmdOnLand();
        }

        if (Input.GetAxis(PC2D.Input.VERTICAL) != 0)
        {
            bool up_pressed = Input.GetAxis(PC2D.Input.VERTICAL) > 0;
            if (_motor.IsOnLadder())
            {
                if (
                    (up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Top)
                    ||
                    (!up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Bottom)
                 )
                {
                    // do nothing!
                }
                // if player hit up, while on the top do not enter in freeMode or a nasty short jump occurs
                else
                {
                    // example ladder behaviour

                    _motor.FreedomStateEnter(); // enter freedomState to disable gravity
                    _motor.EnableRestrictedArea();  // movements is retricted to a specific sprite bounds

                    // now disable OWP completely in a "trasactional way"
                    FreedomStateSave(_motor);
                    _motor.enableOneWayPlatforms = false;
                    _motor.oneWayPlatformsAreWalls = false;

                    // start XY movement
                    _motor.normalizedXMovement = Input.GetAxis(PC2D.Input.HORIZONTAL);
                    _motor.normalizedYMovement = Input.GetAxis(PC2D.Input.VERTICAL);
                }
            }
        }
        else if (Input.GetAxis(PC2D.Input.VERTICAL) < -PC2D.Globals.FAST_FALL_THRESHOLD)
        {
            _motor.fallFast = false;
        }

        if (Input.GetButtonDown(PC2D.Input.DASH))
        {
            _motor.Dash();
        }
    }

    /*
    [Command]
    void CmdOnMove(float axis)
    {
        Debug.Log("CmdOnMove");

        if (!isClient)
        {
            _motor.normalizedXMovement = axis;
            Debug.Log("ServerMoving");
        }

        RpcOnMove(axis);
        PositionSync(new Vector3(transform.position.x, transform.position.y, 0));
    }

    [Command]
    void CmdOnLand()
    {
        Debug.Log("CmdOnLand");

        if (!isClient)
        { 
            _motor.normalizedXMovement = 0;
        }

        RpcOnLand();
        PositionSync(new Vector3(transform.position.x, transform.position.y, 0));
        //PacketWriter.PlayerCoord(transform.position.x, transform.position.y);
    }

    [ClientRpc]
    void RpcOnMove(float axis)
    {
        Debug.Log("RpcOnMove");
        _motor.normalizedXMovement = axis;
    }

    [ClientRpc]
    void RpcOnLand()
    {
        Debug.Log("RpcOnLand");
        _motor.normalizedXMovement = 0;
    }


    void PositionSync(Vector3 position)
    {
        transform.position = position;
    }
    */
}
