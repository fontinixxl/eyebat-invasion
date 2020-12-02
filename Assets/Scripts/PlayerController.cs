using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField]
    private float horizontalSpeed = 5.0f;
    private float horizontalInput;
    // Max distance the player can move on the X axis from the zero position.
    private readonly float xRange = 12.0f;

    private void Update()
    {
        // Check for left and right bounds
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        horizontalInput = Input.GetAxis("Horizontal");

    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * horizontalInput * horizontalSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.AddPointToPlayer();
        Destroy(other.gameObject);
    }
}
