using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private float secondPerRealTimeSecond;
    public bool isNight = false;

    [SerializeField]
    private float nightFogDensity;// �� Fog
    private float dayFogDensity; // �� Fog
    [SerializeField]
    private float fogDensityCalc; // ������ ����
    public float currentFogDensity;

    private void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }
    private void Update()
    {
        // �¾��� x �� �߽����� ȸ��, ���ǽð� 1�ʿ�  0.1f * secondPerRealTimeSecond ������ŭ ȸ��
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170) // 170 �̻� ��
        {
            isNight = true;
        }
        else if (transform.eulerAngles.x <= 10)// 10 ���� ��
        {
            isNight= false;
        }

        if(isNight)
        {
            if(currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if(currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
    public void ResetDayNightCycle()
    {
        transform.rotation = Quaternion.Euler(0, -115, 0);
    }
}
