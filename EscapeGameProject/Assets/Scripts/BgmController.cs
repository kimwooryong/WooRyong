using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour
{
    string Sand = "Sand";
    string Ground = "Ground";
    
    int SandLayerNumber;
    int GroundLayerNumber;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Trigger Check");
        SandLayerNumber = LayerMask.NameToLayer(Sand);
        GroundLayerNumber = LayerMask.NameToLayer(Ground);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == SandLayerNumber)
        {
            SoundManager.Instance.PlayBgmBeach();
        } else
        {
            SoundManager.Instance.bgmPlayer.Stop();
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
