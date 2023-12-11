using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    [Tooltip("������Ʈ �ӵ�")]
    public float speed = 10;
    [Tooltip("������Ʈ �ı� ��ġ")]
    public float destoryPosition = -100;
    
    public bool isSeagull = false;
    public float waveHeight = 3;
    Waves waves;
    /*
    [Tooltip("���ߴ� ��ġ")]
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
            Debug.Log("���ű⿡ ���� �ĵ� ���� ����");
            waves.Octaves[1].height = waveHeight;
        }


    }



}
