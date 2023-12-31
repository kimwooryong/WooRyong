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
    public float darkDomeRotationSpeed = 5;
    public float darkCloudUpSpeed = 10;

    // 먹구름
    public GameObject DarkCloud;
    public float targetY = -300f;
    private bool isMovingDarkCloud = true;
    // 비
    public GameObject Rain;

    // 빛
    public Transform Sun;
    [Tooltip("햇빛이 X축으로 회전하는 속도")]
    public float sunXrotationSpeed = 1;

    [Tooltip("하늘과 햇빛의 각도가 변경되는 시간(초)")]
    public float ChangeTime = 30;
    private float Timer;

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
            SunRotate();

            DarkCloudMove();
            DarkDomeRotate();
                 

        }
    }
    private void DarkCloudMove()
    {
        DarkCloud.SetActive(true);

        if (isMovingDarkCloud == true)
        {
            // DarkCloud를 위로 이동시킵니다.
            DarkCloud.transform.Translate(Vector3.forward * darkCloudUpSpeed * Time.deltaTime);

            // DarkCloud의 y 좌표가 목표 y 좌표에 도달하면
            if (DarkCloud.transform.position.y >= targetY)
            {
                // 이동을 멈추고 darkCloudUpSpeed를 0으로 설정합니다.
                isMovingDarkCloud = false;
                darkCloudUpSpeed = 0f;
                Rain.SetActive(true);
            }
        }
    }

    private void SunRotate()
    {
        Quaternion targetRotation1 = Quaternion.Euler(170f, 0f, 0f);
        Sun.rotation = Quaternion.RotateTowards(Sun.rotation, targetRotation1, sunXrotationSpeed * Time.deltaTime);

    }

    private void DarkDomeRotate()
    {


        if(isMovingDarkCloud == false)
        {
            // DarkDome 회전.
            Quaternion targetRotation3 = Quaternion.Euler(-90f, 0f, 0f);
            DarkDome.rotation = Quaternion.RotateTowards(DarkDome.rotation, targetRotation3, darkDomeRotationSpeed * Time.deltaTime);


            // 시간 많이 지나면 DarkDome이 빠르게 회전
            if (Timer >= (ChangeTime + 25))
            {
                darkDomeRotationSpeed += 0.01f;
                //DarkCloud.SetActive(false);
            }
        }

       
    }
}
