using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRide : MonoBehaviour
{
    private PlayerStatus player;

    public Transform boatSeat;
    public bool isRiding = false;
    private Animator ani;
    Transform left;
    Animator leftChild;
    Transform right;
    Animator rightChild;



    void Start()
    {
        left = gameObject.transform.Find("Left");
        leftChild = left.GetComponent<Animator>();

        right = gameObject.transform.Find("Right");
        rightChild = right.GetComponent<Animator>();

        player = FindObjectOfType<PlayerStatus>();    
    }
    
    void Update()
    {
        if(isRiding)
        {
            gameObject.transform.Translate(Vector3.up * 5 * Time.deltaTime);
            leftChild.SetTrigger("isRide");
            rightChild.SetTrigger("isRide");
        }
    }
}
