using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCharacterController : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement; // Reference to your existing movement script
    public MonoBehaviour psiBlast; // Assign in Inspector
    public MonoBehaviour pyro;     // Assign in Inspector
    public MonoBehaviour glooGun;  // Assign in Inspector

    [Header("Shrink Settings")]
    public Vector3 shrinkScale = new Vector3(0.5f, 0.5f, 0.5f); // Shrunk size
    private Vector3 originalScale;

    public float shrinkSpeedMultiplier = 0.8f;  // Slightly slower
    public float shrinkJumpMultiplier = 0.4f;   // Much weaker jump

    private float originalMoveSpeed;
    private float originalJumpForce;
    private bool isShrunk = false;

    void Start()
    {
        // Save original values
        if (playerMovement != null)
        {
            originalMoveSpeed = playerMovement.moveSpeed;
            originalJumpForce = playerMovement.jumpForce;
        }
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleShrink();
        }
    }

    void ToggleShrink()
    {
        isShrunk = !isShrunk;

        if (isShrunk)
        {
            // Shrink player visually
            transform.localScale = shrinkScale;

            // Adjust movement values
            playerMovement.moveSpeed = originalMoveSpeed * shrinkSpeedMultiplier;
            playerMovement.jumpForce = originalJumpForce * shrinkJumpMultiplier;

            // Disable abilities
            if (psiBlast != null) psiBlast.enabled = false;
            if (pyro != null)     pyro.enabled = false;
            if (glooGun != null)  glooGun.enabled = false;
        }
        else
        {
            // Reset player size
            transform.localScale = originalScale;

            // Restore movement values
            playerMovement.moveSpeed = originalMoveSpeed;
            playerMovement.jumpForce = originalJumpForce;

            // Re-enable abilities
            if (psiBlast != null) psiBlast.enabled = true;
            if (pyro != null)     pyro.enabled = true;
            if (glooGun != null)  glooGun.enabled = true;
        }
    }
}
