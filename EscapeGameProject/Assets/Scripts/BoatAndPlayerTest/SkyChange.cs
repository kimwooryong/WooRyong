using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyChange : MonoBehaviour
{
    public GameObject sun;
    public GameObject sky;
    public GameObject cloudsPrefab;

    private GameObject cloudsInstance;

    public float SunRotationSpeed = 5f;
    public float SkyRotationSpeed_X = 5f;
    public float SkyRotationSpeed_Y = 1f;

    
    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.Find("Sun");
        sky = GameObject.Find("Sky");
        // Prefab�� �ν��Ͻ�ȭ�Ͽ� ���
        cloudsInstance = Instantiate(cloudsPrefab);
        cloudsInstance.name = "Clouds"; // �ν��Ͻ� �̸� ����
    }

    // Update is called once per frame
    void Update()
    {
       

        if(cloudsPrefab != null)
        {
            // ������ ����� Sun�� light �� ������ ���� x������ ȸ��
            sun.transform.Rotate(Vector3.right * SunRotationSpeed * Time.deltaTime*-1);
            sky.transform.Rotate(Vector3.right * SkyRotationSpeed_X * Time.deltaTime*-1);
            SkyRotationSpeed_Y = 0;

            if (sun.transform.rotation.x < 180)
            {
                SunRotationSpeed = 0;
            }

           
            if (sky.transform.rotation.x == -150)
            {
                SkyRotationSpeed_X = 0;
            }
        }
        else
        {
            //  ������ ���ٸ� Sky�� y������ õõ�� ȸ��
            sky.transform.Rotate(Vector3.up * SkyRotationSpeed_Y * Time.deltaTime);
        }


    }

    
    public void SpawnClouds(GameObject _Clouds)
    {
        if (_Clouds != null)
        {
            // ������ ����� Sun�� light �� ������ ���� x������ ȸ��
            sun.transform.Rotate(Vector3.right * SunRotationSpeed * Time.deltaTime);
            sky.transform.Rotate(Vector3.right * SkyRotationSpeed_X * Time.deltaTime);
            SkyRotationSpeed_Y = 0;

            if (sun.transform.rotation.x < 180)
            {
                SunRotationSpeed = 0;
            }


            if (sky.transform.rotation.x == -150)
            {
                SkyRotationSpeed_X = 0;
            }
        }
        else
        {
            //  ������ ���ٸ� Sky�� y������ õõ�� ȸ��
            sky.transform.Rotate(Vector3.up * SkyRotationSpeed_Y * Time.deltaTime);
        }

    }
    

}
