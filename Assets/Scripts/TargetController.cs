using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField]
    private float speed = 6.0f;
    private Rigidbody targetRb;
    private bool hasCollided;
    private int direction;
    private Animator animator;

    void Start()
    {
        hasCollided = false;
        animator = GetComponent<Animator>();
        targetRb = GetComponent<Rigidbody>();
        // Adjust direction so the target allways go towards the playGround (camera)
        direction = transform.position.x > 0 ? -1: 1;
    }

    void FixedUpdate()
    {
        if (!hasCollided)
        {
            transform.Translate(Vector3.right * direction * Time.deltaTime * speed, Space.World);
        }
    }

    // Once the target is hit, enable gravity so it will start falling down
    private void OnMouseDown()
    {
        targetRb.useGravity = true;
        // Disable flying animation once the target is hit
        animator.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        hasCollided = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy target once it goes off the screen
        if (other.gameObject.CompareTag("Sensor"))
        {
            Destroy(gameObject);
        }
    }
}
