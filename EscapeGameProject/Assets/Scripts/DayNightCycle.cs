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
    private float nightFogDensity;// 밤 Fog
    private float dayFogDensity; // 낮 Fog
    [SerializeField]
    private float fogDensityCalc; // 증감량 비율
    public float currentFogDensity;

    private void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }
    private void Update()
    {
        // 태양을 x 축 중심으로 회전, 현실시간 1초에  0.1f * secondPerRealTimeSecond 각도만큼 회전
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170) // 170 이상 밤
        {
            isNight = true;
        }
        else if (transform.eulerAngles.x <= 10)// 10 이하 낮
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
