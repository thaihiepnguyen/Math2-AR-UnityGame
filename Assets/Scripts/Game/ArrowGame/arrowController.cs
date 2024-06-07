using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class arrowController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private SphereCollider myCollider;
    
    [SerializeField]
    private GameObject stickingArrow;
    private bool isStuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (transform.position.magnitude > 3000f)
        {
            Destroy(gameObject);
        }
    }
   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            ArrowGameManager.GetInstance().DecreaseHealth();
            Destroy(gameObject);
            return;
        }
        rb.isKinematic = true;
        myCollider.isTrigger = true;

        TrailRenderer trail=GetComponentInChildren<TrailRenderer>();
        trail.enabled = false;
        
        if (collision.collider.attachedRigidbody != null)
        {
            transform.parent = collision.collider.attachedRigidbody.transform;
        }

        collision.collider.GetComponent<IHittable>()?.GetHit();

        
        if (!collision.gameObject.CompareTag("Target"))
        {
            ArrowGameManager.GetInstance().DecreaseHealth();

            Destroy(gameObject, 3f);
        }
        else
        {
            ArrowGameManager.GetInstance().CheckAnswer(collision.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        }
    }

   
}
