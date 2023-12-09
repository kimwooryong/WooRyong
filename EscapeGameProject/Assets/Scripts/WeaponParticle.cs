using UnityEngine;

public class WeaponParticle : MonoBehaviour
{

    public GameObject[] particlePrefab;
    private PlayerStatus player;
    private PlayerCotroller playerController;


    private void Start()
    {
        player = GetComponentInParent<PlayerStatus>();
        playerController = GetComponentInParent<PlayerCotroller>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Hammer"))
        {
            player.playerDamage = 0;
            //if로 무기들 tag 해두고 PlayerStatus에 데미지를 나눠놓고 거기에 맞게 더해주기
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
        if (other.CompareTag("Animal") && playerController._isAttack)
        {
            AnimalMovement animal = other.GetComponent<AnimalMovement>();
            if (animal != null)
            {
                BoxCollider boxCollider = other.GetComponent<BoxCollider>();

                if (boxCollider != null)
                {
                    animal.isHit = true;
                    animal.TakeDamage(player.animalDamage);
                    Vector3 hitPoint = boxCollider.ClosestPointOnBounds(transform.position);
                    GameObject particleInstance = Instantiate(particlePrefab[0], hitPoint, Quaternion.identity);
                    Invoke("DestroyParticle", 0.5f);
                }
            }
        }
      
        else if (other.CompareTag("Tree") && playerController._isAttack)
        {
            Tree tree = other.GetComponent<Tree>();
            if (tree != null)
            {
                MeshCollider meshCollider = other.GetComponent<MeshCollider>();

                if (meshCollider != null)
                {

                    tree.TakeDamage(player.animalDamage);
                    Vector3 hitPoint = meshCollider.ClosestPointOnBounds(transform.position);
                    GameObject particleInstance = Instantiate(particlePrefab[1], hitPoint, Quaternion.identity);
                    Invoke("DestroyParticle", 0.5f);
                }
            }
        }
        else if (other.CompareTag("Rock") && playerController._isAttack)
        {
            Rock rock = other.GetComponent<Rock>();
            if (rock != null)
            {
                CapsuleCollider capsuleCollider = other.GetComponent<CapsuleCollider>();

                if (capsuleCollider != null)
                {

                    rock.TakeDamage(player.animalDamage);
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

}