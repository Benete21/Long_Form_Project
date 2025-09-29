using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animator animator;


    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OpenDoorAnimator()
    {
        animator.enabled = false; 
        animator.GetInteger(1);
    }
}
