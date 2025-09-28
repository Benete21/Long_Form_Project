using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Object_Stop : MonoBehaviour
{
    public Animator animator;
    public float time;
    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StopMovement()
    {

            animator.speed = 0f;
            StartCoroutine(RestartAnimationAfterDelay(time));
    }

        private IEnumerator RestartAnimationAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            animator.speed = 1f;
            animator.Play("YourAnimationName", -1, 0f);
        }



    }
