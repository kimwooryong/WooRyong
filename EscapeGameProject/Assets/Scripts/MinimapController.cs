using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Camera minimapCamera;
    [HideInInspector]
    public bool isMinimapActive = false;

    private DayNightCycle dayNightCycle;
    private Quaternion savedRotation;
    private float savedFog;

    void Start()
    {
        isMinimapActive = false;
        dayNightCycle = FindObjectOfType<DayNightCycle>();
    }

    void Update()
    {
        if (dayNightCycle != null)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToggleMinimap();
            }
        }
    }

    void ToggleMinimap()
    {
        isMinimapActive = !isMinimapActive;
        if (isMinimapActive)
        {
            // 미니맵 활성화 전 현재 회전값 저장
            savedRotation = dayNightCycle.transform.rotation;
            savedFog = dayNightCycle.currentFogDensity;
            dayNightCycle.transform.eulerAngles = new Vector3(90.0f, -115.0f, 0.0f);
            dayNightCycle.currentFogDensity = 0;
            Time.timeScale = 0;
        }
        else
        {
            // 미니맵 비활성화 시 저장한 회전값 복원
            dayNightCycle.transform.rotation = savedRotation;
            dayNightCycle.currentFogDensity = savedFog;

            Time.timeScale = 1;
        }
    }

}
