using System.Collections;
using UnityEngine;

public class WeaponParticle : MonoBehaviour
{
    public GameObject[] particlePrefab;
    private PlayerStatus player;
    private PlayerCotroller playerController;
    private CapsuleCollider capsuleCollider;
    private bool hasPlayedAttackMissSound = false;
    private bool isCapsuleColliderEnabled = false;
    private bool isAttackUsing = false;

    private void Start()
    {
        player = GetComponentInParent<PlayerStatus>();
        playerController = GetComponentInParent<PlayerCotroller>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
    }

    private void Update()
    {
        if (playerController.isAttacking)
        {
            if (!isCapsuleColliderEnabled)
            {
                if(!isAttackUsing)
                {
                StartCoroutine(EnableCapsuleColliderWithDelay(0.2f));

                }
                if (!hasPlayedAttackMissSound)
                {
                    SoundManager.Instance.PlayPlayerAttackMiss();
                    hasPlayedAttackMissSound = true;
                }
            }
        }
        else
        {
            capsuleCollider.enabled = false;
            hasPlayedAttackMissSound = false;
            isCapsuleColliderEnabled = false; // Reset the flag when not attacking
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Hammer"))
        {
            player.animalDamage = 2;
            player.rockDamage = 2;
            player.TreeDamage = 4;
        }
        else if (gameObject.CompareTag("Weapon"))
        {
            player.animalDamage = 4;
            player.rockDamage = 2;
            player.TreeDamage = 2;
        }
        else if (gameObject.CompareTag("Pickaxe"))
        {
            player.animalDamage = 2;
            player.rockDamage = 4;
            player.TreeDamage = 2;
        }

        if (other.CompareTag("Animal"))
        {
            AnimalMovement animal = other.GetComponent<AnimalMovement>();
            if (animal != null)
            {
                BoxCollider boxCollider = other.GetComponent<BoxCollider>();

                if (boxCollider != null)
                {
                    int beforeHp = animal.currentHp;
                    SoundManager.Instance.PlayPlayerAttackAnimals(); // 다른 소리로 바꾸자
                    animal.isHit = true;
                    animal.TakeDamage(player.animalDamage);
                    if (animal.currentHp != beforeHp)
                    {
                        playerController.isAttacking = false;
                    }
                    Vector3 hitPoint = boxCollider.ClosestPointOnBounds(transform.position);
                    GameObject particleInstance = Instantiate(particlePrefab[0], hitPoint, Quaternion.identity);
                    Invoke("DestroyParticle", 0.5f);
                }
            }
        }
      
        else if (other.CompareTag("Tree"))
        {
            Tree tree = other.GetComponent<Tree>();
            if (tree != null)
            {
                MeshCollider meshCollider = other.GetComponent<MeshCollider>();

                if (meshCollider != null)
                {
                    int beforeHp = tree.currentHp;
                    SoundManager.Instance.PlayPlayerAttackTree();
                    tree.TakeDamage(player.TreeDamage);
                    if (tree.currentHp != beforeHp) 
                    {
                        playerController.isAttacking = false;
                    }
                    Vector3 hitPoint = meshCollider.ClosestPointOnBounds(transform.position);
                    GameObject particleInstance = Instantiate(particlePrefab[1], hitPoint, Quaternion.identity);
                    Invoke("DestroyParticle", 0.5f);
                }
            }
        }
        else if (other.CompareTag("Rock"))
        {
            Rock rock = other.GetComponent<Rock>();
            if (rock != null)
            {
                CapsuleCollider capsuleCollider = other.GetComponent<CapsuleCollider>();

                if (capsuleCollider != null)
                {
                    int beforeHp = rock.currentHp;
                    SoundManager.Instance.PlayPlayerAttackRock();
                    rock.TakeDamage(player.rockDamage);
                    if (rock.currentHp != beforeHp)
                    {
                        playerController.isAttacking = false;
                    }
                    Vector3 hitPoint = capsuleCollider.ClosestPointOnBounds(transform.position);
                    GameObject particleInstance = Instantiate(particlePrefab[2], hitPoint, Quaternion.identity);
                    Invoke("DestroyParticle", 0.5f);
                }
            }
        }

    }

    private void DestroyParticle()
    {

        GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");

        foreach (GameObject particleInstance in particles)
        {
            Destroy(particleInstance);
        }
    }
    IEnumerator EnableCapsuleColliderWithDelay(float delay)
    {
        isAttackUsing = true;
        yield return new WaitForSeconds(delay);
        capsuleCollider.enabled = true;
        isCapsuleColliderEnabled = true;
        isAttackUsing = false;
    }

}