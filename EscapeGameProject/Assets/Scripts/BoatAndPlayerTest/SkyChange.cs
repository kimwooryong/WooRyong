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
        // Prefab을 인스턴스화하여 사용
        cloudsInstance = Instantiate(cloudsPrefab);
        cloudsInstance.name = "Clouds"; // 인스턴스 이름 설정
    }

    // Update is called once per frame
    void Update()
    {
       

        if(cloudsPrefab != null)
        {
            // 구름이 생기면 Sun의 light 값 변경을 위해 x축으로 회전
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
            //  구름이 없다면 Sky가 y축으로 천천히 회전
            sky.transform.Rotate(Vector3.up * SkyRotationSpeed_Y * Time.deltaTime);
        }


    }

    
    public void SpawnClouds(GameObject _Clouds)
    {
        if (_Clouds != null)
        {
            // 구름이 생기면 Sun의 light 값 변경을 위해 x축으로 회전
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
            //  구름이 없다면 Sky가 y축으로 천천히 회전
            sky.transform.Rotate(Vector3.up * SkyRotationSpeed_Y * Time.deltaTime);
        }

    }
    

}
