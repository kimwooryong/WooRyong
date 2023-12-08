using UnityEngine;

public class WeaponParticle : MonoBehaviour
{
    public GameObject particlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("Hammer") || other.CompareTag("Pickaxe"))
        {
            Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
            GameObject particleInstance = Instantiate(particlePrefab, hitPoint, Quaternion.identity);
            Invoke("DestroyParticle", 0.5f);
        }
    }

    private void DestroyParticle()
    {
        GameObject particleInstance = GameObject.FindWithTag("Particle");
        if (particleInstance != null)
        {
            Destroy(particleInstance);
        }
    }
}
