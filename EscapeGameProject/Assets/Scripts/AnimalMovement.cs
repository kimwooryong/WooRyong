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
    public string[] idleStates;
    private int currentIdle;
    public float attackRecognitionScope;


    public float moveSpeed = 5f;
    public float rotationSpeed = 45f;
    public float rotationInterval = 2f;

    private float timer = 0f;

    public MonsterBehavior behavior;
    public float rayRange = 10f;
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

    public GameObject[] dropItemObject;

    private void Start()
    {
        ani = GetComponent<Animation>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        player = FindObjectOfType<PlayerMovement>();
        currentHp = maxHp;

    }


    void Update()
    {
        float distanceToPlayer = Vector3.Distance(this.gameObject.transform.position, playerPosition.position);
        if (!isPlayerCheck && !isHit && !isDie)
        {

            ani.wrapMode = WrapMode.Loop;
            timer += Time.deltaTime;

            if (!isIdleStateChangeRunning)
            {
                StartCoroutine(IdleStateChange());
            }

            string currentAnimationState = idleStates[currentIdle];

            ani.CrossFade(idleStates[currentIdle] , 0.3f);

            

            
            if (currentIdle == 0)
            {
                if (timer >= rotationInterval)
                {
                    RotateSmoothly();

                    timer = 0f;
                }
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }


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
                StartCoroutine(DestroyAfterDelay(1f));

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

        yield return new WaitForSecondsRealtime(0.8f);

        ani.Stop("attack");

        yield return new WaitForSecondsRealtime(0.3f);

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

        yield return new WaitForSecondsRealtime(delay * 1.04f);
        ani.Stop();
        foreach (GameObject item in dropItemObject)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
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
