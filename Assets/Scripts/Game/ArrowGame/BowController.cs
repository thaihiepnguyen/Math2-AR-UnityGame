using UnityEngine;
using UnityEngine.UI;

public class BowController : MonoBehaviour
{
    [SerializeField] private BowString bowStringRenderer;
    [SerializeField] private Button shootButton;
    [SerializeField] private GameObject midPointParent;
    [SerializeField] private float maxPullDistance = 1.5f; // Maximum distance to pull the bowstring
    [SerializeField] private float pullSpeed = 0.001f; // Speed at which the bowstring is pulled
    private Vector3 startPullPoint;
    private bool isPulling = false;
    private float pullStartTime;
    private Vector3 initialBowStringScale;
    private Vector3 localRotation;
    [SerializeField] Rigidbody arrowPrefab;
    [SerializeField] Transform arrowSpawn;
    [SerializeField] Transform arrowRotationSpawn;
   
    [SerializeField] private Camera mainCamera;
    public float maxForce = 1000f;
    private float strenght = 0f;
    void Start()
    {
        
        var buttonHoldAndRelease = shootButton.GetComponent<ButtonHoldAndRelease>();
        buttonHoldAndRelease.OnButtonDownEvent += StartPull;
        buttonHoldAndRelease.OnButtonHoldEvent += ContinuePull;
        buttonHoldAndRelease.OnButtonUpEvent += ReleaseBow;
        startPullPoint=midPointParent.transform.localPosition;

    }

    void StartPull()
    {
        isPulling = true;
        pullStartTime = Time.time;
    }

    void ContinuePull()
    {
        if (!isPulling)
            return;

        float pullDuration = Time.time - pullStartTime;
        float pullDistance = Mathf.Min(pullDuration * pullSpeed, maxPullDistance);
        float scaleFactor = pullDistance / maxPullDistance;
        strenght = Mathf.Abs(midPointParent.transform.localPosition.x)/maxPullDistance;
        Vector3 newScale = startPullPoint;
        if (Mathf.Abs(midPointParent.transform.localPosition.x) >= maxPullDistance)
        {
            return;
        }
        //newScale.y -= scaleFactor;
        newScale.x -= scaleFactor;
        midPointParent.transform.localPosition = newScale;
        bowStringRenderer.CreateString(midPointParent.transform.localPosition);
    }

    void ReleaseBow()
    {
        isPulling = false;
       
        midPointParent.transform.localPosition = startPullPoint;
        ShootArrow();
    }
    void ShootArrow()
    {
        if (arrowPrefab!= null)
        {

            Vector3 spawn = arrowSpawn.position;
            
            // Instantiate the arrow prefab at the arrow spawn point position and rotation
            
            var arrow = Instantiate(arrowPrefab, spawn, arrowSpawn.rotation);
            // Add force to the arrow in the direction of the arrow spawn point's forward vector
            if (Mathf.Abs(strenght) > 1)
            {
                strenght= 1;
            }
            Vector3 shootingDirection = Camera.main.transform.forward;
           arrow.AddForce(shootingDirection *Mathf.Abs(strenght)* maxForce,ForceMode.Impulse); // Adjust the force as needed
        }
        
    }
    void Update()
    {
        if (isPulling)
        {
            ContinuePull();
        }
        else
        {
            bowStringRenderer.CreateString(null);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootArrow();
        }
    }
}
