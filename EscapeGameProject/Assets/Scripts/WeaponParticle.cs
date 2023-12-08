using UnityEngine;

public class WeaponParticle : MonoBehaviour
{
    
    public GameObject[] particlePrefab;
    private PlayerStatus player;

    private void Start()
    {
        player = GetComponentInParent<PlayerStatus>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Axe"))
        {
            player.playerDamage = 0;
            //if로 무기들 tag 해두고 PlayerStatus에 데미지를 나눠놓고 거기에 맞게 더해주기
        }
        if (other.CompareTag("Animal"))
        {
            Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
            GameObject particleInstance = Instantiate(particlePrefab[0], hitPoint, Quaternion.identity);
            Invoke("DestroyParticle", 0.5f);
        }
        else if (other.CompareTag("Tree"))
        {
            Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
            GameObject particleInstance = Instantiate(particlePrefab[1], hitPoint, Quaternion.identity);
            Invoke("DestroyParticle", 0.5f);
        }
        else if (other.CompareTag("Rock"))
        {
            Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
            GameObject particleInstance = Instantiate(particlePrefab[2], hitPoint, Quaternion.identity);
            Invoke("DestroyParticle", 0.5f);
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