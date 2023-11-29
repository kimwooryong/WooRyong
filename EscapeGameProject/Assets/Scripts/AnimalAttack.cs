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
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
        animal = GetComponentInParent<AnimalMovement>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 pushDirection = transform.position - other.transform.position ;
            player.rb.AddForce(pushDirection.normalized * 3.0f, ForceMode.Impulse);
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
