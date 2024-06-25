using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MovingTarget : MonoBehaviour, IHittable
{
    private Rigidbody rb;
    private bool stopped = false;

    private Vector3 nextposition;
    private Vector3 originPosition;

    [SerializeField]
    private int health = 1;

    //[SerializeField]
    //private AudioSource audioSource;
    [SerializeField]
    private bool moveHorizontally = true;
    [SerializeField]
    private float arriveThreshold, movementRadius = 2, speed = 1;
    [SerializeField] public ParticleSystem explose;
    [SerializeField] public ParticleSystem success;
    [SerializeField] GameObject Item;
    [SerializeField] public TextMeshProUGUI text;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originPosition = transform.position;
        nextposition = GetNewMovementPosition();
    }

    private Vector3 GetNewMovementPosition()
    {
        Vector3 newPosition = originPosition;
        if (moveHorizontally)
        {
            // Move horizontally
            newPosition.x = originPosition.x + movementRadius * (Random.value > 0.5f ? 1 : -1);
        }
        else
        {
            // Move vertically
            newPosition.y = originPosition.y + movementRadius * (Random.value > 0.5f ? 1 : -1);
        }
        return newPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((rb.isKinematic || collision.gameObject.CompareTag("Arrow")) == false)
        {
            //audioSource.Play();
            Debug.Log("Target down");

        }
        if(collision.gameObject.CompareTag("Arrow") == true){
            
        }
    }
    void OnDestroy() {
       
    }

    public void GetHit()
    {
        Item.SetActive(false);
       
        
        text.gameObject.SetActive(false);
        


    }

    private void FixedUpdate()
    {
        if (stopped == false)
        {
            if(Vector3.Distance(transform.position,nextposition) < arriveThreshold)
            {
                nextposition = GetNewMovementPosition();
            }

            Vector3 direction = nextposition - transform.position;
            rb.MovePosition(transform.position + direction.normalized * Time.fixedDeltaTime * speed);
        }
    }
}

public interface IHittable
{
    void GetHit();
}