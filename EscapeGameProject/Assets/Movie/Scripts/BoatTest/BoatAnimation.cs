using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAnimation : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Start", true);
    }

}
