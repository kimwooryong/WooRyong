using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum MonsterBehavior
{
    Aggression,
    Defensiveness
}



public class AnimalMovement : MonoBehaviour
{

    private Animation ani;
    public float moveSpeed = 5f;
    public float rotationSpeed = 45f;
    public float rotationInterval = 2f;

    private float timer = 0f;

    public MonsterBehavior behavior;
    public float rayRange = 10f;
    public Transform player;
    public bool isPlayerCheck;

    public float coneAngle = 45f;
    public int rayCount = 5;


    private void Start()
    {
        ani = GetComponent<Animation>();

    }

    void Update()
    {


        if (!isPlayerCheck)
        {

            ani.wrapMode = WrapMode.Loop;
            ani.Play("walk_1");

            timer += Time.deltaTime;
            if (timer >= rotationInterval)
            {
                RotateSmoothly();

                timer = 0f;
            }
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            RaycastHit hit;
            float raySpacing = coneAngle / (rayCount - 1);

            for (int i = 0; i < rayCount; i++)
            {
                float currentHorizontalAngle = -coneAngle / 2 + i * raySpacing;

                for (int j = 0; j < rayCount; j++)
                {
                    float currentVerticalAngle = -coneAngle / 2 + j * raySpacing;

                    Quaternion rayRotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);

                    if (Physics.Raycast(this.gameObject.transform.position, rayRotation * this.gameObject.transform.forward, out hit, rayRange))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            isPlayerCheck = true;
                            break;
                        }
                    }
                    Debug.DrawRay(transform.position, rayRotation * transform.forward * rayRange, Color.Lerp(Color.red, Color.green, (i * rayCount + j) / (float)(rayCount * rayCount - 1)));
                }

            }

        }


        else if (isPlayerCheck)
        {
            ani.wrapMode = WrapMode.Loop;
            ani.Play("run");
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (behavior == MonsterBehavior.Aggression)
            {
                Vector3 directionToPlayer = (player.position - transform.position);
                directionToPlayer.y = 0f;

                Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 2f);

                if (distanceToPlayer > 1f)
                {
                    transform.Translate(Vector3.forward * moveSpeed * 2 * Time.deltaTime);
                }
                else if (distanceToPlayer >= 20)
                {
                    isPlayerCheck = false;
                }

            }

            if (behavior == MonsterBehavior.Defensiveness)
            {
                Vector3 directionToPlayer = (player.position - transform.position);
                directionToPlayer.y = 0f;

                Quaternion rotation = Quaternion.LookRotation(-directionToPlayer);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 2f);

                transform.Translate(Vector3.forward * moveSpeed * 2 * Time.deltaTime);
                if (distanceToPlayer >= 20)
                {
                    isPlayerCheck = false;
                }
            }
        }

    }

    void RotateSmoothly()
    {
        float targetRotation = Random.Range(0f, 360f);
        StartCoroutine(RotateOverTime(targetRotation, rotationSpeed));
    }

    IEnumerator RotateOverTime(float targetRotation, float speed)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotationQuaternion = Quaternion.Euler(0f, targetRotation, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotationQuaternion, t);
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerCheck = true;
        }
    }
}