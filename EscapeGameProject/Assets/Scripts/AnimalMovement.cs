using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    private Animation ani;
    public float moveSpeed = 5f;
    public float rotationSpeed = 45f;
    public float rotationInterval = 2f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= rotationInterval)
        {
            RotateSmoothly();
            
            timer = 0f;
        }

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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
}

