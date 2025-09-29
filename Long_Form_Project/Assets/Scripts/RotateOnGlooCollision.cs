using UnityEngine;

public class RotateOnGlooCollision : MonoBehaviour
{
    [Tooltip("Degrees to rotate on the X axis when Gloo collides.")]
    public float rotationAmount = 90f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the colliding object is a Gloo clone
        if (collision.gameObject.name.Contains("Gloo"))
        {
            Debug.Log("Rotating and sticking Gloo to platform!");

            // Make Gloo a child of this platform so it sticks during rotation
            collision.transform.SetParent(transform);

            // Rotate the platform 90 degrees around its local X axis
            transform.Rotate(rotationAmount, 0f, 0f, Space.Self);
        }
    }
}