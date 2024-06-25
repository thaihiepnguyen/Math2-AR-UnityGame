using UnityEngine;

public class MovingBubble : MonoBehaviour, IHittable
{
    private Rigidbody rb;

    private Vector3 nextposition;

    [SerializeField]
    private float maxSpeed = 1;
    private float minSpeed = 0.1f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nextposition = GetNewMovementPosition();
    }

    private Vector3 GetNewMovementPosition()
    {
        Vector3 newPosition = transform.position;

        newPosition.y = transform.position.y + 1;
        newPosition.x = transform.position.x + Random.Range(-1, 1);

        return newPosition;
    }

    public void GetHit()
    {

    }

    private void FixedUpdate()
    {
        nextposition = GetNewMovementPosition();
        Vector3 direction = nextposition - transform.position;
        rb.MovePosition(transform.position + direction.normalized * Time.fixedDeltaTime * Random.Range(minSpeed, maxSpeed));
        if (transform.position.y >= 50 || transform.position.x >= 50 || transform.position.y <= -50 || transform.position.x <= -50)
        {
            Destroy(gameObject);
        }
    }
}

public interface IBubbleHittable
{
    void GetHit();
}