using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public Animator animator;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("IsOpen", true);
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("IsOpen", false);
    }
}
