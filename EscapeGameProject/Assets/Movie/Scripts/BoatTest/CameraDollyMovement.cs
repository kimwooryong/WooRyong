using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDollyMovement : MonoBehaviour
{
    //private GameObject startSeagull;
    public GameObject firstCamera; // 변수명 수정: fristCamera -> firstCamera
    public GameObject secondCamera;
    public GameObject VNCamera;
    public GameObject playerCamera;
    public GameObject LastCamera;
    //public CinemachineSmoothPath cinemachineSmoothPath;
    private float timeNum = 0;
    public float changeTime = 30;
    public float changeLastTime = 130;
    public GameObject WavePlane;
    public GameObject BoatLight;
    private bool _isEnding =false;
    void Start()
    {
        //startSeagull = transform.Find("StartSeagull").gameObject;
        timeNum = Time.time; // 현재 시간으로 초기화

    }

    // Update is called once per frame
    void Update()
    {
        timeNum += Time.deltaTime;
        _isEnding = timeNum > 25;
        //Debug.Log($"{timeNum}");
        if(_isEnding == false)
        {
            if (timeNum <= changeTime)
            {
                WavePlane.transform.position = new Vector3(0,10,0);
                Debug.Log("첫번째 씬");
                firstCamera.SetActive(true); // 
                secondCamera.SetActive(false);
            }
         
            else if (timeNum > changeTime)
            {
                WavePlane.transform.position = new Vector3(0,-10,0);
                Debug.Log("두번째 씬");
                firstCamera.SetActive(false); // 
                secondCamera.SetActive(true);
            }
        }
        else
        {
           
            if(timeNum > changeLastTime)
            {
                Debug.Log("마지막 씬");
                secondCamera.SetActive(false);
                playerCamera.SetActive(false);
                VNCamera.SetActive(true);
                LastCamera.SetActive(true);
            }
            else
            {
                Debug.Log("세번째 씬");
                secondCamera.SetActive(false);
                VNCamera.SetActive(false);
                playerCamera.SetActive(true);
                BoatLight.SetActive(true);
            }
            
        }
         
        
        
       
    }
}
