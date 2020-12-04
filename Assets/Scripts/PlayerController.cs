using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    public static event Action<int> EnemyCaughtEvent;

    [SerializeField]
    private float speed = 10.0f;
    private float horizontalMovement;
    private int totalPoint;

    private void Start()
    {
        HUDController.TimesUpEvent += HUDController_TimesUpEvent;
        totalPoint = 0;
    }

    // Disable MonoBehaviour once the timer is over
    private void HUDController_TimesUpEvent()
    {
        enabled = false;
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        horizontalMovement = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalMovement) > Mathf.Epsilon)
        {
            horizontalMovement = horizontalMovement * Time.deltaTime * speed;
            horizontalMovement += transform.position.x;

            float limit =
                Mathf.Clamp(horizontalMovement, ScreenBounds.Left, ScreenBounds.Right);

            transform.position = new Vector3(limit, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        totalPoint++;

        EnemyCaughtEvent?.Invoke(totalPoint);

        Destroy(other.gameObject);
    }
}
