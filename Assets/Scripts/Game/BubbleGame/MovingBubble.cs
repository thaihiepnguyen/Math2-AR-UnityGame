using UnityEngine;

public class MovingBubble : MonoBehaviour, IHittable
{
    private Rigidbody rb;

    private Vector3 nextposition;

    [SerializeField]
    private float speed = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nextposition = GetNewMovementPosition();
    }

    private Vector3 GetNewMovementPosition()
    {
        Vector3 newPosition = transform.position;

        // Move horizontally
        newPosition.y = transform.position.y + speed;

        return newPosition;
    }

    public void GetHit()
    {

    }

    private void FixedUpdate()
    {
        nextposition = GetNewMovementPosition();
        Vector3 direction = nextposition - transform.position;
        rb.MovePosition(transform.position + direction.normalized * Time.fixedDeltaTime * speed);
        if (transform.position.y >= 50)
        {
            Destroy(gameObject);
        }
    }
}

public interface IBubbleHittable
{
    void GetHit();
}