using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AnimalDamage
{
    Bear,
    Boar,
    Wolf

}

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

    public AnimalDamage damage;


    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }
    void Start()
    {
        capsuleCollider.enabled = false;


    }

    private void Update()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (damage == AnimalDamage.Bear)
            {
                player.TakeDamage(20);
            }
            else if (damage == AnimalDamage.Boar)
            {
                player.TakeDamage(15);
            }
            else if (damage == AnimalDamage.Wolf)
            {
                player.TakeDamage(10);
            }
            player.rb.AddForce(Vector3.back * 20, ForceMode.Impulse);
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
