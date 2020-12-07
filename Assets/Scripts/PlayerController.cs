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
        HUDController.RestartGameEvent += HUDController_RestartGameEvent;
        totalPoint = 0;
    }

    private void HUDController_RestartGameEvent()
    {
        totalPoint = 0;
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            MovePlayer();
        }
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
