using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 5f;
    [HideInInspector]
    public Rigidbody rb;
    public int playerDamage = 2;
    public GameObject miniMap;
    public bool onMove;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
        onMove = true;
        rb = GetComponent<Rigidbody>();
        if(miniMap != null)
        {
             miniMap.SetActive(false);

        }

    }

    // Update is called once per frame
    void Update()
    {
 /*       if (onMove)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            Vector3 velocity = new Vector3(inputX, -1, inputY);
            velocity *= playerSpeed;
            rb.velocity = velocity;
        }*/

        if (Input.GetKeyDown(KeyCode.L))
        {
            Destroy(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.SetActive(!miniMap.activeSelf);
        }


    }
}
