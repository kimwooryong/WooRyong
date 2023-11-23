using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour
{

    public string sceneName;
    public Vector3 playerPosition;
    public Vector3 playerRotation;



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
            collision.transform.position = playerPosition;
            collision.transform.eulerAngles = playerRotation;

        }
    }

}

