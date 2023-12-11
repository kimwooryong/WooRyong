using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTornado : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 150.0f;
    public float stopDistance = 1.0f;
    public float waveHeight = 4f;

    public bool isMove = true;
    public bool isTornado = false;

    Waves waves;

    private void Start()
    {
        waves = GameObject.Find("Waves").GetComponent<Waves>();
    }

    void Update()
    {
        if (isMove)
        {
            OnMoveTarget();

        }

        if(isTornado == true && transform.position.z < 4000)
        {
            Debug.Log("토네이도에 의해 파도높이 변경");
            waves.Octaves[1].height = waveHeight;
        }
      
    }

    void OnMoveTarget()
    {
        Vector3 direction = targetObject.position - transform.position;

        if (direction.magnitude > stopDistance)
        {
            transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
           
        }
        else
        {
            isMove = false;
        }
    }




}
