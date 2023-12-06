using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapWater : MonoBehaviour
{
    private MinimapController minimapController;
    public GameObject miniMapWater;

    // Start is called before the first frame update
    
    void Start()
    {
        miniMapWater.SetActive(false);
        minimapController = FindObjectOfType<MinimapController>();
    }

    void Update()
    {
        if (minimapController.isMinimapActive == false)
        {
            miniMapWater.SetActive(false);
        }
        else 
        {
            miniMapWater.SetActive(true);
        }
    }

}
