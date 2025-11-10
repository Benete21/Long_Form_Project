using UnityEngine;
using System.Collections;

public class RotateOnGlooCollision : MonoBehaviour
{ 
    [Tooltip("Degrees to rotate on the X axis when Gloo collides.")]
    public float rotationAmount = 90f;

    [Tooltip("Seconds it takes to complete the rotation.")]
    public float rotationDuration = 2f;

    private bool isRotating = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

       
        if (collision.gameObject.name.Contains("Gloo") && !isRotating)
        {
            Debug.Log("Rotating and sticking Gloo to platform!");

           
            collision.transform.SetParent(transform);

           
            StartCoroutine(RotateOverTime(rotationAmount, rotationDuration));
        }
    }

    private IEnumerator RotateOverTime(float degrees, float duration)
    {
        isRotating = true;

        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(degrees, 0f, 0f);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = endRotation;
        isRotating = false;
    }
}

