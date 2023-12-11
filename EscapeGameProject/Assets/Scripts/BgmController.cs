using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour
{
    string Sand = "Sand";
    string Ground = "Ground";
    string InCave = "InCave";
    
    int SandLayerNumber;
    int GroundLayerNumber;
    int CaveLayerNumber;
    bool IsInCave = true;
    

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start Trigger Check");
        SandLayerNumber = LayerMask.NameToLayer(Sand);
        GroundLayerNumber = LayerMask.NameToLayer(Ground);
        CaveLayerNumber = LayerMask.NameToLayer(InCave);
    }


    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log($"{other.gameObject.layer}, {GroundLayerNumber}");

        if (other.gameObject.layer == SandLayerNumber)
        {
            SoundManager.Instance.PlayBgmBeach();
        } else if (IsInCave && other.gameObject.layer == CaveLayerNumber)
        {
            SoundManager.Instance.bgmPlayer.Stop();
            SoundManager.Instance.PlayBgmInCave();
            StartCoroutine(SoundManager.Instance.PlayBatNoiseWaitForSeconds());
            IsInCave = false;

        } else if (other.gameObject.layer == GroundLayerNumber)
        {
            SoundManager.Instance.bgmPlayer.Stop();
            //Debug.Log("On Ground");
            SoundManager.Instance.PlayBgmForest();
        }
        else
        {
            //Debug.Log("Stop Sound");
            //SoundManager.Instance.bgmPlayer.Stop();
        }
    }

    // 위치가 변경되어야 작동함
    //private void OnCollisionEnter(Collision other)
    //{
    //    string playingClip = SoundManager.Instance.selectedClipName;
    //    Debug.Log("On sand");
    //    //Debug.Log($"{other.gameObject.layer}, {SandLayer},{other.gameObject.layer.Equals(SandLayer)}");

        //    // 해변가에 도달하면 바다 bgm play


        //    // 풀숲에 도달하면 풀숲 bgm play


        //    if (other.gameObject.layer == SandLayerNumber)
        //    {
        //        Debug.Log($"Layer: {SandLayerNumber}");
        //        SoundManager.Instance.PlayBgmBeach();

        //        //Debug.Log($"{SoundManager.Instance.bgmPlayer}");
        //        //Debug.Log($"{SoundManager.Instance.selectedClip}");

        //        Debug.Log(playingClip);

        //    } else if (other.gameObject.layer == GroundLayerNumber)
        //    {
        //        if (SoundManager.Instance.bgmPlayer.isPlaying)
        //        {
        //            //SoundManager.Instance.bgmPlayer.Stop();
        //            //Debug.Log("Stop--");
        //        }
        //        Debug.Log("On Ground");
        //        SoundManager.Instance.PlayBgmForest();
        //    }

}
