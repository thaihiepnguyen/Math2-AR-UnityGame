using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MissileController : MonoBehaviour, IVFXEntity
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float maxDistanceToPlayer = 10f;
    [SerializeField] private float delayValidateDistanceToPlayer = 0.25f;

    [field: SerializeField] public Color CurrentColor { get; private set; }
    public Vector3 ExplosionPosition => transform.position;

    Rigidbody rb;
    Vector3 kinematicVelocity;
    HealthController healthController;
    WaitForSeconds waitValidateDistanceToPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthController = GetComponent<HealthController>();
        waitValidateDistanceToPlayer = new WaitForSeconds(delayValidateDistanceToPlayer);
    }
    



    public void Init()
    {
        GameManager.Instance.MissileCreated(this);
        kinematicVelocity = transform.forward * speed;
        StartCoroutine(DestroyFarAwayFromPlayerRoutine());
    }

    private void FixedUpdate() => rb.MovePosition(transform.position + kinematicVelocity * Time.deltaTime);



    IEnumerator DestroyFarAwayFromPlayerRoutine()
    {
        while (true)
        {
            yield return waitValidateDistanceToPlayer;
            if (Vector3.Distance(transform.position, GameManager.Instance.PlayerPosition) > maxDistanceToPlayer)
                AutoDestroy();
        }
    }

    void AutoDestroy()
    {
        healthController.Damage(healthController.Health, DamageMode.Collision);
    }
}
