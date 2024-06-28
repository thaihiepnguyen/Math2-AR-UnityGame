using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RunnerController : MonoBehaviour
{
    // Start is called before the first frame update

     private CharacterController controller;
     private Vector3 direction;
    public float forwardSpeed = 5;
    
    private int desiredLane = 1;
    public float laneDistance = 2.5f;

    public float maxSpeed;

    public float jumpForce;
    public float gravity = -12f;

    public Animator animator;

    private bool isSliding = false;

      int maxHealth = 5;
      int currentHealth;
  
    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    void Start()
    {
         controller = GetComponent<CharacterController>();

         currentHealth = maxHealth;

         direction = transform.localPosition;
    }

         public void ChangeHealth(int amount)
    {
       
        // currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        currentHealth += amount;
        DragonHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }


    // Update is called once per frame
    void Update()
    {   
        

        if (!RunnerManager.isGameStarted || RunnerManager.gameOver || RunnerManager.gameCompleted )
        return;

     

        if (currentHealth <= 0){
          RunnerManager.gameOver = true;
          return;
        
        }
      

        animator.SetBool("IsGameStarted",true);
        direction.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
        if (isGrounded){
        Debug.Log("Hello");
     
  
      
        if(SwipeManager.swipeUp){
           
            Jump();
        }
        }
        else {
            Debug.Log("no");
        direction.y += gravity * Time.deltaTime;
        }
        
        if (SwipeManager.swipeDown && !isSliding){
            StartCoroutine(Slide());
        }
        if (SwipeManager.swipeRight){
            desiredLane++;
            if (desiredLane == 3) {
                desiredLane = 2;
            }
        }

        if (SwipeManager.swipeLeft){
            desiredLane--;
            if (desiredLane == -1){
                desiredLane = 0;
            }
        }

        Vector3 targetPosition = transform.localPosition.z*transform.forward + transform.localPosition.y* transform.up;
        if (desiredLane == 0){
            targetPosition+= Vector3.left*laneDistance;
        }
        else if (desiredLane == 2){
            targetPosition+= Vector3.right * laneDistance;
        }

    //    transform.position = targetPosition;
        
        //  transform.position = Vector3.Lerp(transform.position,targetPosition, 1000 * Time.deltaTime);
        //  controller.center = controller.center;

        if (transform.localPosition == targetPosition )
            return;
        
        Vector3 diff = targetPosition - transform.localPosition;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;

        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
        
        //    controller.Move(direction * Time.deltaTime);


    }

   
   
    private void FixedUpdate()
    {   
    
          if (!RunnerManager.isGameStarted || RunnerManager.gameOver || RunnerManager.gameCompleted)
        return;
     

           controller.Move(direction * Time.fixedDeltaTime);

    }

    private void Jump(){
         direction.y = jumpForce;
    }

    private void  OnControllerColliderHit (ControllerColliderHit hit){
        if (hit.transform.tag == "Obstacle"){
            RunnerManager.gameOver = true;
        }
    }

    //     void OnCollisionEnter(Collision other)
    // {
    //     if (other.transform.tag == "Obstacle"){
    //         Debug.Log("ab");
    //          RunnerManager.gameOver = true;
    //     }
    // }

    private IEnumerator Slide(){
        isSliding = true;
        animator.SetBool("IsSliding",true);
        controller.center = new Vector3(0,-0.5f,0);
        controller.height = 1;
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("IsSliding", false);
         controller.center = new Vector3(0,0,0);
        controller.height = 2;
        isSliding = false;
    }
}
