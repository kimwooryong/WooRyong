using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalAttack : MonoBehaviour
{

    [SerializeField]
    private float attackWaitingTime;
    [SerializeField]
    private float attackDuration;
    [SerializeField]
    private float attackEnd;

    private AnimalMovement animal;
    private PlayerMovement player;
    public CapsuleCollider capsuleCollider;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }
    void Start()
    {
        capsuleCollider.enabled = false;


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Debug.Log("동물 공격");

        }   
    }
    public IEnumerator Attack()
    {
        yield return new WaitForSecondsRealtime(attackWaitingTime);
        capsuleCollider.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        capsuleCollider.enabled = false;
        yield return new WaitForSeconds(attackEnd);
    }
}
