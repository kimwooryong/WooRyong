using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCampfire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SoundManager.Instance.PlayCampFireNoiseWaitForSeconds());
        //SoundManager.Instance.PlayCampfire(); 
    }

}
