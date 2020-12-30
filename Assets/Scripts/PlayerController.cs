using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip enemyCaughtSound;
    public static event Action<int> ScorePointsEvent;

    public ParticleSystem dirtParticleRight;
    public ParticleSystem dirtParticleLeft;

    [SerializeField]
    private float speed = 10.0f;

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameState.RUNNING)
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

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.AudioSource.PlayOneShot(enemyCaughtSound, 1f);

        // Get the total points this Enemy is worth for; store and display
        int pointsToScore = other.gameObject.GetComponentInParent<TargetController>().ScorePoints;

        ScorePointsEvent?.Invoke(pointsToScore);

        Destroy(other.gameObject);
    }
}
