using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum MonsterBehavior
{
    Aggression,
    Defensiveness
}

public class AnimalMovement : MonoBehaviour
{
    private Animation ani;
    public string[] idleStates;
    private int currentIdle;
    public float attackRecognitionScope;

    public float moveSpeed = 5f;
    public float rotationSpeed = 45f;
    public float rotationInterval = 2f;
    private float timer = 0f;

    public MonsterBehavior behavior;
    public float boxSize = 1f;
    public Transform playerPosition;
    public bool isPlayerCheck;

    public float coneAngle = 45f;
    public int rayCount = 5;

    public bool isIdleStateChangeRunning = false;
    private bool isAttckTime = false;

    public int currentHp;
    public int maxHp;
    private PlayerMovement player;
    private bool isHit = false;
    private bool isDie = false;
    [HideInInspector]
    public float dieTime = 1.0f;

    private BoxCollider boxCollider;

    private AnimalAttack animalAttack;

    private GameObject attackCollider;
    private Vector3 attackColliderPosition;

    private void Start()
    {
        ani = GetComponent<Animation>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        player = FindObjectOfType<PlayerMovement>();
        animalAttack = GetComponentInChildren<AnimalAttack>();
        if (attackCollider != null)
        {
            attackCollider = animalAttack.gameObject;
            attackCollider.transform.localPosition = Vector3.zero;
        }

        currentHp = maxHp;

        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);

        if (!isPlayerCheck && !isHit && !isDie)
        {
            ani.wrapMode = WrapMode.Loop;
            timer += Time.deltaTime;

            if (!isIdleStateChangeRunning)
            {
                StartCoroutine(IdleStateChange());
            }

            string currentAnimationState = idleStates[currentIdle];

            ani.CrossFade(idleStates[currentIdle], 0.3f);

            if (currentIdle == 0)
            {
                if (timer >= rotationInterval)
                {
                    RotateSmoothly();
                    timer = 0f;
                }
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }

            Quaternion rayRotation = Quaternion.Euler(0, -coneAngle / 2, 0);

            Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(boxSize, boxSize, boxSize), rayRotation);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    ani.Stop();
                    Debug.Log("¼±¸ÂÀ½");
                    isPlayerCheck = true;
                    break;
                }
            }

            DebugDrawWireBox(transform.position, new Vector3(boxSize, boxSize, boxSize), rayRotation);
        }


        else if (isPlayerCheck && !isHit && !isDie)
        {

            if (behavior == MonsterBehavior.Aggression)
            {
                
                Vector3 directionToPlayer = (playerPosition.position - transform.position);
                directionToPlayer.y = 0f;

                Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 2f);

                if (distanceToPlayer > attackRecognitionScope && !isAttckTime)
                {
                    transform.Translate(Vector3.forward * moveSpeed * 2 * Time.deltaTime);
                    ani.wrapMode = WrapMode.Loop;
                    ani.Play("run");
                }
                if (distanceToPlayer <= attackRecognitionScope)
                {
                    transform.Translate(Vector3.forward * moveSpeed * 0.01f * Time.deltaTime);
                    if(isAttckTime == false)
                    {
                    StartCoroutine(WaitAttackTime());

                    }
                }

                else if (distanceToPlayer >= 20)
                {
                    isPlayerCheck = false;
                }

            }

            if (behavior == MonsterBehavior.Defensiveness)
            {
                ani.wrapMode = WrapMode.Loop;
                ani.Play("run");
                Vector3 directionToPlayer = (playerPosition.position - transform.position);
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
        if (Input.GetKeyDown(KeyCode.J))
        {
            isHit = true;
            TakeDamage(player.playerDamage);
            if (currentHp <= 0)
            {

                isDie = true;
                StartCoroutine(DestroyAfterDelay(dieTime));

            }
        }
    }
    void DebugDrawWireBox(Vector3 center, Vector3 size, Quaternion rotation)
    {
        // Calculate the local axes based on rotation
        Vector3 right = rotation * Vector3.right * size.x / 2f;
        Vector3 up = rotation * Vector3.up * size.y / 2f;
        Vector3 forward = rotation * Vector3.forward * size.z / 2f;

        // Calculate the 8 corners of the box
        Vector3 topLeftFront = center + up + forward - right;
        Vector3 topRightFront = center + up + forward + right;
        Vector3 bottomLeftFront = center - up + forward - right;
        Vector3 bottomRightFront = center - up + forward + right;
        Vector3 topLeftBack = center + up - forward - right;
        Vector3 topRightBack = center + up - forward + right;
        Vector3 bottomLeftBack = center - up - forward - right;
        Vector3 bottomRightBack = center - up - forward + right;

        // Draw lines connecting the corners to represent the wireframe
        Debug.DrawLine(topLeftFront, topRightFront);
        Debug.DrawLine(topRightFront, bottomRightFront);
        Debug.DrawLine(bottomRightFront, bottomLeftFront);
        Debug.DrawLine(bottomLeftFront, topLeftFront);

        Debug.DrawLine(topLeftBack, topRightBack);
        Debug.DrawLine(topRightBack, bottomRightBack);
        Debug.DrawLine(bottomRightBack, bottomLeftBack);
        Debug.DrawLine(bottomLeftBack, topLeftBack);

        Debug.DrawLine(topLeftFront, topLeftBack);
        Debug.DrawLine(topRightFront, topRightBack);
        Debug.DrawLine(bottomRightFront, bottomRightBack);
        Debug.DrawLine(bottomLeftFront, bottomLeftBack);
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

    IEnumerator IdleStateChange()
    {
        isIdleStateChangeRunning = true;

        yield return new WaitForSecondsRealtime(7.0f);

        currentIdle = Random.Range(0, idleStates.Length);

        isIdleStateChangeRunning = false;
    }
    IEnumerator WaitAttackTime()
    {
        isAttckTime = true;

        ani.Play("attack");

        yield return new WaitForSecondsRealtime(0.2f);
        StartCoroutine(animalAttack.Attack());
        yield return new WaitForSecondsRealtime(0.6f);
        ani.Stop("attack");

        yield return new WaitForSecondsRealtime(0.2f);

        isAttckTime = false;
    }

    void TakeDamage(int damage)
    {
        isPlayerCheck = true;
        currentHp -= damage;

        if (currentHp < 0)
        {
            currentHp = 0;
        }

        StartCoroutine(HitAnimation());
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        ani.Stop();
        ani.Play("die");

        Destroy(boxCollider, delay - 0.1f);
        yield return new WaitForSecondsRealtime(delay);
        ani.Stop();
        float destroyWaitTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);

        while (destroyWaitTime < 2.0f)
        {
            destroyWaitTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, destroyWaitTime / 5.0f);
            yield return null;
        }
        Destroy(gameObject, delay * 2.0f);
        yield return new WaitForSecondsRealtime(delay * 2.0f);
    }

    IEnumerator HitAnimation()
    {
        ani.Stop();
        ani.Play("hit");
        yield return new WaitForSeconds(1.0f);
        isHit = false;
    }



}
