using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    [Tooltip("오브젝트 속도")]
    public float speed = 10;
    [Tooltip("오브젝트 파괴 위치")]
    public float destoryPosition = -100;
    
    public bool isSeagull = false;
    public float waveHeight = 3;
    Waves waves;
    /*
    [Tooltip("멈추는 위치")]
    public Vector3 stopPosition = new Vector3 (0, 0, 10);
    */

    private void Start()
    {
        waves = GameObject.Find("Waves").GetComponent<Waves>();
    }

    private void Update()
    {
        
        transform.Translate(Vector3.forward * Time.deltaTime * speed* -1);

        if (transform.position.z < destoryPosition)
        {
            Destroy(gameObject);
        }

        if (isSeagull == true && transform.position.z <40) 
        {
            Debug.Log("갈매기에 의해 파도 높이 변경");
            waves.Octaves[1].height = waveHeight;
        }


    }



}
