using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    Animator dragonAnimation;
    public float speedThreshold = 0.1f; // The speed threshold to change animation


    private FixedJoystick joystick;

    private Rigidbody rigidBody;
    private Vector3 lastPosition;  // Store the last position
    private float lastTime;        // Store the last time position was recorded

    private WaitForSeconds quartSec = new WaitForSeconds(.25f);

    
    //Control the animation state, so that 
    private DragonAnimationState AnimationState;
    void OnEnable()
    {
        dragonAnimation = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>();
        rigidBody = gameObject.GetComponent<Rigidbody>();

        // lastPosition = transform.position;  // Initialize last position
        // lastTime = Time.time;
        // StartCoroutine(ControlState());
        // AnimationState = DragonAnimationState.isIdle;
    }

    
    void FixedUpdate()
    {
        float xVal = joystick.Horizontal;
        float yVal = joystick.Vertical;

        Vector3 movement = new Vector3(xVal,0.0f,yVal);
        rigidBody.velocity = movement*speedThreshold;

          if (movement.sqrMagnitude<=0)
                {
                     dragonAnimation.SetBool("isIdle",true);
                     dragonAnimation.SetBool("isRunning",false);
                    return;
                }
                    dragonAnimation.SetBool("isIdle",false);
                     dragonAnimation.SetBool("isRunning",true);
                var targetDirection = Vector3.RotateTowards(transform.forward, movement,
                    10 * Time.deltaTime, 0.0f);
                
                transform.rotation=Quaternion.LookRotation(targetDirection);


        // if (xVal !=0 && yVal!=0){
        //     transform.eulerAngles = new Vector3(transform.eulerAngles.x,Mathf.Atan2(xVal,yVal) *Mathf.Rad2Deg,transform.eulerAngles.z);
        // }

      

              
    }
    
    public enum DragonAnimationState
    {
        isIdle, 
        isRunning,
    }
    
    IEnumerator ControlState()
    {
        while (true)
        {
            // Calculate elapsed time
            float elapsedTime = Time.time - lastTime;

            // Calculate the distance traveled since the last frame
            float distance = Vector3.Distance(transform.position, lastPosition);

            // Calculate the speed
            float speed = distance / elapsedTime;

            // Update last time and last position for the next frame
            lastTime = Time.time;
            lastPosition = transform.position;
            
            Debug.Log("CurrentState is => " + AnimationState);

            
            //Change Animation based on speed
            if (speed > speedThreshold)
            {
                if (AnimationState != DragonAnimationState.isRunning)
                {
                    Debug.Log("Set to is Running, Animation State => " + AnimationState);
                    ChangeToState("isRunning");
                    AnimationState = DragonAnimationState.isRunning;
                }
            }
            else if(AnimationState != DragonAnimationState.isIdle)
            {
                Debug.Log("Set to is Idle =>" + AnimationState);
                ChangeToState("isIdle");
                AnimationState = DragonAnimationState.isIdle;

            }
            
            yield return quartSec;
        }
        yield return null;
    }

    //Change one bool to true and the others to false
    void ChangeToState(string setToState)
    {
        foreach (var param in dragonAnimation.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                dragonAnimation.SetBool(param.name, param.name == setToState);
            }
        }
    }
}
