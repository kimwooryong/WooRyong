using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{

    public Transform Sun;
    [Tooltip("햇빛이 X축으로 회전하는 속도")]
    public float sunXrotationSpeed = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(170f, 0f, 0f);
        Sun.rotation = Quaternion.RotateTowards(Sun.rotation, targetRotation, sunXrotationSpeed * Time.deltaTime);
    }
}
