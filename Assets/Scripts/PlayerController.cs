using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip enemyCaughtSound;
    public static event Action<int> EnemyCaughtEvent;

    public ParticleSystem dirtParticleRight;
    public ParticleSystem dirtParticleLeft;

    [SerializeField]
    private float speed = 10.0f;
    private int totalPoint;
    
    private void Start()
    {
        UIController.TimesUpEvent += HUDController_TimesUpEvent;
        UIController.RestartGameEvent += HUDController_RestartGameEvent;
        totalPoint = 0;
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
        float horizontalMovement = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalMovement) > Mathf.Epsilon)
        {
            GenerateParticles(horizontalMovement);

            horizontalMovement = horizontalMovement * Time.deltaTime * speed;
            horizontalMovement += transform.position.x;

            float limit =
                Mathf.Clamp(horizontalMovement, ScreenBounds.Left, ScreenBounds.Right);

            transform.position = new Vector3(limit, transform.position.y, transform.position.z);
        }
        else
        {
            StopDirtParticles();
        }
    }

    private void GenerateParticles(float horizontalMovement)
    {
        if (horizontalMovement > 0.01f && !dirtParticleLeft.isPlaying)
        {
            dirtParticleLeft.Play();
        }
        else if (horizontalMovement < -0.01f && !dirtParticleRight.isPlaying)
        {
            dirtParticleRight.Play();
        }
    }
    private void StopDirtParticles()
    {
        dirtParticleLeft.Stop();
        dirtParticleRight.Stop();
    }

    private void HUDController_TimesUpEvent()
    {
        StopDirtParticles();
    }

    private void HUDController_RestartGameEvent()
    {
        totalPoint = 0;
        transform.position = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.AudioSource.PlayOneShot(enemyCaughtSound, 1f);
        totalPoint++;

        EnemyCaughtEvent?.Invoke(totalPoint);

        Destroy(other.gameObject);
    }
}
