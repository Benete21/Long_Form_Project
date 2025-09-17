using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Object_Stop : MonoBehaviour
{
    public Animator animator;
    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StopMovement()
    {
        animator.enabled = false;
        animator.GetInteger(1);
        StartCoroutine(StartAnimation());
    }
    public IEnumerator StartAnimation()
    {
        animator.GetInteger(0);
        yield return null;
    }
}
