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
    public float darkRotationSpeed = 5;

    // �Ա���
    public GameObject DarkCloud;

    // ��
    public Transform Sun;
    [Tooltip("�޺��� X������ ȸ���ϴ� �ӵ�")]
    public float sunXrotationSpeed = 1;

    [Tooltip("�ϴð� �޺��� ������ ����Ǵ� �ð�(��)")]
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

            // �Ա��� Ȱ��ȭ
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
 

        // �ð� ���� ������ ������ DarkCloud�� ȸ��
        if (Timer >= (ChangeTime + 20))
        {
            darkRotationSpeed += 0.01f;
            DarkCloud.SetActive(false);
        }
    }
}
