using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotation : MonoBehaviour
{
    // �ϴ�
    public Transform Sky;
    [Tooltip("���� �ϴ��� Y������ ȸ���ϴ� �ӵ�")]
    public float skyRotationSpeed = 1;

    public Transform DarkDome;
    [Tooltip("���� �ϴ��� X������ ȸ���ϴ� �ӵ�")]
    public float darkDomeRotationSpeed = 5;
    public float darkCloudUpSpeed = 10;

    // �Ա���
    public GameObject DarkCloud;
    public float targetY = -300f;
    private bool isMovingDarkCloud = true;
    // ��
    public GameObject Rain;

    // ��
    public Transform Sun;
    [Tooltip("�޺��� X������ ȸ���ϴ� �ӵ�")]
    public float sunXrotationSpeed = 1;

    [Tooltip("�ϴð� �޺��� ������ ����Ǵ� �ð�(��)")]
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
            // DarkCloud�� ���� �̵���ŵ�ϴ�.
            DarkCloud.transform.Translate(Vector3.forward * darkCloudUpSpeed * Time.deltaTime);

            // DarkCloud�� y ��ǥ�� ��ǥ y ��ǥ�� �����ϸ�
            if (DarkCloud.transform.position.y >= targetY)
            {
                // �̵��� ���߰� darkCloudUpSpeed�� 0���� �����մϴ�.
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
            // DarkDome ȸ��.
            Quaternion targetRotation3 = Quaternion.Euler(-90f, 0f, 0f);
            DarkDome.rotation = Quaternion.RotateTowards(DarkDome.rotation, targetRotation3, darkDomeRotationSpeed * Time.deltaTime);


            // �ð� ���� ������ DarkDome�� ������ ȸ��
            if (Timer >= (ChangeTime + 25))
            {
                darkDomeRotationSpeed += 0.01f;
                //DarkCloud.SetActive(false);
            }
        }

       
    }
}
