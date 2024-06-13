using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    Animator dragonAnimation;
    public float speedThreshold = 10f; // The speed threshold to change animation


    private FixedJoystick joystick;

    private Rigidbody rigidBody;

    public Transform jumpRay;
    public float jumpForce = 0.5f;

    // public float maxStepHeight = 0.25f;
    // public int stepDetail = 1;
    // public LayerMask stepMask;

    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 2f;
    bool isGrounded = true;

     public int maxHealth = 5;
      int currentHealth;
    public int health { get { return currentHealth; } set { currentHealth = value; } }

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    
    [SerializeField] UIHealthBar healthBar;
    
    //Control the animation state, so that 
   
    void Awake()
    {
        dragonAnimation = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
          currentHealth = maxHealth;
         stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);

    }

     public void ChangeHealth(int amount)
    {
        if (amount < 0) {
            if (isInvincible) return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        DragonHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }



    public void Jump(){
        if (isGrounded)
        rigidBody.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
    }
    void Update()
    {
        float xVal = joystick.Horizontal;
        float yVal = joystick.Vertical;

        if (isInvincible) {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            isInvincible = false;
        }
    
    
        Vector3 movement = new Vector3(xVal,0.0f,yVal);
       

          if (movement.sqrMagnitude == 0)
                {
                     dragonAnimation.SetBool("isIdle",true);
                     dragonAnimation.SetBool("isRunning",false);
                  
                }
                else {
                    dragonAnimation.SetBool("isIdle",false);
                    dragonAnimation.SetBool("isRunning",true);
                }
                  
                var targetDirection = Vector3.RotateTowards(transform.forward, movement,
                    10 * Time.deltaTime, 0.0f);
                
                transform.rotation=Quaternion.LookRotation(targetDirection);
              

                isGrounded = Physics.BoxCastAll(jumpRay.transform.position, new Vector3(.5f, .1f, .5f), Vector3.down, Quaternion.identity, .1f).Length > 1;

              

                if (!isGrounded)
                {
                     dragonAnimation.SetBool("isIdle",true);
                     dragonAnimation.SetBool("isRunning",false);
                }

                 Vector3 velocity = movement* speedThreshold * Time.deltaTime;
        //         bool isFirstStepCheck = false;
        //         bool canMove = true;

        //         for (int i = stepDetail; i >= 1; i--)
        //         {
        //             Collider[] c = Physics.OverlapBox(transform.position + new Vector3(0, i * maxStepHeight / stepDetail - transform.localScale.y, 0), new Vector3(1.05f, maxStepHeight / stepDetail / 2, 1.05f), Quaternion.identity, stepMask);

        //             if (velocity != Vector3.zero)
        //             {
        //                 if (c.Length > 0 && i == stepDetail)
        //                 {
        //                     isFirstStepCheck = true;
        //                     if (!isGrounded)
        //                     {
        //                         Debug.Log("stack");
        //                         canMove = false;
        //                     }
        //                 }

        //                 if (c.Length > 0 && !isFirstStepCheck)
        //                 {
        //                     transform.position += new Vector3(0, i * maxStepHeight / stepDetail, 0);
        //                     break;
        //                 }
        //             }
        //         }

       
        // if(canMove)
            rigidBody.velocity = velocity;

            stepClimb();
                // rigidBody.velocity = movement*speedThreshold;

        // if (xVal !=0 && yVal!=0){
        //     transform.eulerAngles = new Vector3(transform.eulerAngles.x,Mathf.Atan2(xVal,yVal) *Mathf.Rad2Deg,transform.eulerAngles.z);
        // }

      

              




    }
    

    void stepClimb()
    {
         RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

         RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f,0,1), out hitLower45, 0.1f))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f,0,1), out hitUpper45, 0.2f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f,0,1), out hitLowerMinus45, 0.1f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f,0,1), out hitUpperMinus45, 0.2f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
    }
    

}
