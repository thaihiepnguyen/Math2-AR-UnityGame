using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInputControls))]
public class PlayerMovement : NetworkBehaviour
{
    private PlayerInputControls _playerInputControls;
    private const float MOVE_SPEED = 0.02f;
    private const float FLY_SPEED = 0.02f;
        
    private const float MOVE_THRESHOLD = 0.01f;
    private const float LOOKATPOINT_DELTA = 2f;
    private const float SMOOTH_TIME = 0.1f;

    private Vector3 _currentVelocity;
    private GameObject lookAtPoint;
    private GameObject FlyUpButton;
    private GameObject FlyDownButton;
    Vector3 startPos;
    

    private void Start()
    {
        
        
    }

    private void OnFlyUpDown()
    {
        
    }
    private void OnFlyUpHold()
    {
        
        Vector3 newPosition = transform.position;
        newPosition.y +=  FLY_SPEED;
        transform.position = newPosition;
    }
    private void OnFlyUpUp()
    {
        
    }
    private void OnFlyDownDown()
    {
        
    }
    private void OnFlyDownHold()
    {
        Vector3 newPosition = transform.position;
        newPosition.y -=  FLY_SPEED;
        transform.position = newPosition;
    }
    private void OnFlyDownUp()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (GetComponent<NetworkObject>().IsOwner)
        {
            lookAtPoint = new GameObject
            {
                transform =
                {
                    position = transform.position,
                    rotation = transform.rotation
                }
            };

            _playerInputControls = GetComponent<PlayerInputControls>();
            _playerInputControls.OnMoveInput += PlayerInputControlsOnOnMoveInput;
            _playerInputControls.OnShootAnglePerformed += PlayerInputControlsOnShootInput;
            _playerInputControls.OnFlyInput += PlayerInputControlsOnFlyPerformed;
            FlyUpButton = GameObject.FindGameObjectWithTag("FlyUp");
            FlyDownButton = GameObject.FindGameObjectWithTag("FlyDow");
            var flyUp = FlyUpButton.GetComponent<ButtonHoldAndRelease>();
            flyUp.OnButtonDownEvent += OnFlyUpDown;
            flyUp.OnButtonHoldEvent += OnFlyUpHold;
            flyUp.OnButtonDownEvent += OnFlyUpUp;

            var flyDown = FlyDownButton.GetComponent<ButtonHoldAndRelease>();
            flyDown.OnButtonDownEvent += OnFlyDownDown;
            flyDown.OnButtonHoldEvent += OnFlyDownHold;
            flyDown.OnButtonDownEvent += OnFlyDownUp;
            startPos = transform.position;
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
    }

    private void PlayerInputControlsOnShootInput(Vector2 obj)
    {
        if (obj.magnitude < MOVE_THRESHOLD) return;

        Vector3 newPosition = transform.position;
        newPosition.y += obj.y * MOVE_SPEED;
        transform.position = Vector3.Lerp(transform.position, newPosition, SMOOTH_TIME);
    }

    private void PlayerInputControlsOnOnMoveInput(Vector3 inputMovement)
    {
        if (inputMovement.magnitude < MOVE_THRESHOLD) return;

        transform.position += inputMovement * MOVE_SPEED;
        PlayerLookInMovementDirection(inputMovement);
    }

    private void PlayerInputControlsOnFlyPerformed(Vector2 inputFly)
    {
        if (inputFly.magnitude < MOVE_THRESHOLD) return;

        Vector3 newPosition = transform.position;
        newPosition.y += inputFly.y * FLY_SPEED;
        transform.position = Vector3.Lerp(transform.position, newPosition, SMOOTH_TIME);
    }

    private void PlayerLookInMovementDirection(Vector3 inputVector)
    {
        Vector3 pointToLookAt = transform.position + (inputVector.normalized * LOOKATPOINT_DELTA);
        lookAtPoint.transform.position = pointToLookAt;
        transform.LookAt(lookAtPoint.transform);
    }

    public override void OnNetworkDespawn()
    {
        if (GetComponent<NetworkObject>().IsOwner)
        {
            _playerInputControls.OnMoveInput -= PlayerInputControlsOnOnMoveInput;
            _playerInputControls.OnShootAnglePerformed -= PlayerInputControlsOnShootInput;
            _playerInputControls.OnFlyInput -= PlayerInputControlsOnFlyPerformed;
            var flyUp = FlyUpButton.GetComponent<ButtonHoldAndRelease>();
            flyUp.OnButtonDownEvent -= OnFlyUpDown;
            flyUp.OnButtonHoldEvent -= OnFlyUpHold;
            flyUp.OnButtonDownEvent -= OnFlyUpUp;

            var flyDown = FlyDownButton.GetComponent<ButtonHoldAndRelease>();
            flyDown.OnButtonDownEvent -= OnFlyDownDown;
            flyDown.OnButtonHoldEvent -= OnFlyDownHold;
            flyDown.OnButtonDownEvent -= OnFlyDownUp;
        }
    }
    
}
