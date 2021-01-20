using UnityEngine;

public class TargetController : MonoBehaviour
{
    public Texture2D targetCursor;
    [SerializeField]
    private float speed = 6.0f;

    private Rigidbody targetRb;
    private bool hasCollided;
    private bool isHit;
    private int direction;
    private Animator animator;

    [SerializeField]
    private int scorePoints;
    public int ScorePoints
    {
        get
        {
            return scorePoints;
        }
    }

    void Start()
    {
        isHit = false;
        hasCollided = false;
        animator = GetComponent<Animator>();
        targetRb = GetComponent<Rigidbody>();
        // Adjust direction so the target allways go towards the middle
        direction = transform.position.x > 0 ? -1 : 1;
    }

    void FixedUpdate()
    {
        if (!hasCollided)
        {
            transform.Translate(Vector3.right * direction * Time.deltaTime * speed, Space.World);
        }
    }

    private void OnMouseEnter()
    {
        if (!isHit)
            Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    // Once the target is hit, enable gravity so it will start falling down
    private void OnMouseDown()
    {
        // Return if the target has been already hit
        if (isHit)
            return;

        SoundManager.Instance.PlaySoundEffect(SoundEffect.EnemyHit);
        targetRb.useGravity = true;

        // Disable flying animation once the target is hit
        animator.enabled = false;
        isHit = true;
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

            // Do not execute the following code if the toggle feature is turned off
            if (!GameManager.Instance.GameOverOnEyeEscapeFeature)
                return;

            if (!isHit)
            {
                GameManager.Instance.GameOver();
            }

        }
    }
}
