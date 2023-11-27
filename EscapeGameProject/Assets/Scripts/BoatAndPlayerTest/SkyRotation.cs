using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotation : MonoBehaviour
{
    // 하늘
    public Transform Sky;
    [Tooltip("밝은 하늘이 Y축으로 회전하는 속도")]
    public float skyRotationSpeed = 1;

    public Transform DarkDome;
    [Tooltip("검은 하늘이 X축으로 회전하는 속도")]
    public float darkRotationSpeed = 5;

    // 먹구름
    public GameObject DarkCloud;

    // 빛
    public Transform Sun;
    [Tooltip("햇빛이 X축으로 회전하는 속도")]
    public float sunXrotationSpeed = 1;

    [Tooltip("하늘과 햇빛의 각도가 변경되는 시간(초)")]
    public float ChangeTime = 30;
    private float Timer;


    private Quaternion initialRotation;

    // private bool isChanged = false;
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        Sky.transform.Rotate(Vector3.up * skyRotationSpeed*Time.deltaTime);

        Timer += Time.deltaTime;
        if(Timer >= ChangeTime)
        {
            skyRotationSpeed = 0;
            //isChanged = true;

            // 먹구름 활성화
            DarkCloud.SetActive(true);

            DarkDomeRotate();
            SunRotate();
           

        }
    }

    private void SunRotate()
    {
        Quaternion targetRotation1 = Quaternion.Euler(170f, 0f, 0f);
        Sun.rotation = Quaternion.RotateTowards(Sun.rotation, targetRotation1, sunXrotationSpeed * Time.deltaTime);

    }

    private void DarkDomeRotate()
    {
        Quaternion targetRotation2 = Quaternion.Euler(-90f, 0f, 0f);
        DarkDome.rotation = Quaternion.RotateTowards(DarkDome.rotation, targetRotation2, darkRotationSpeed * Time.deltaTime);
 

        // 시간 많이 지나면 빠르게 DarkCloud가 회전
        if (Timer >= (ChangeTime + 20))
        {
            darkRotationSpeed += 0.01f;
            DarkCloud.SetActive(false);
        }
    }
}
