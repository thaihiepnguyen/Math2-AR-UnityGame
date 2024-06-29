using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

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
    [SerializeField] AudioClip objectHit;
    [SerializeField] AudioClip targetHit;
    AudioSource audioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource= GetComponent<AudioSource>();
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
        if (collision.gameObject.CompareTag("Target"))
        {
            audioSource.clip = targetHit;
            audioSource.Play();
            StartCoroutine(StopSoundAfterSecond(1f));
        }
        else
        {
            audioSource.clip = objectHit;
            audioSource.Play();

        }
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
    IEnumerator StopSoundAfterSecond(float second)
    {
        yield return new WaitForSecondsRealtime(second);
        audioSource.Stop();
    }
   
}
