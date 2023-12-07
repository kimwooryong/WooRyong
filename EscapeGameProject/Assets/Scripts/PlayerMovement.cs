using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 5f;
    [HideInInspector]
    public Rigidbody rb;
    private Collider collider;
    public int playerDamage = 2;
    public GameObject miniMap;
    //public bool onMove;
    private GameManager gameManager;
    private CaveEntrance cave;

    public float rayDistance = 2.0f;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
        gameManager.isCaveEnter = false;
        }
        cave = FindObjectOfType<CaveEntrance>();
        if (cave != null)
        {
        cave.newIntensityMultiplier = 1.0f;
        }
        //onMove = true;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        if(miniMap != null)
        {
             miniMap.SetActive(false);

        }

    }
    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.SetActive(!miniMap.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = transform.forward;
            RaycastHit hit;

            if(Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
            {
                if (hit.collider.CompareTag("Boat"))
                {
                BoatRide boat = FindObjectOfType<BoatRide>();
                    if(boat != null)
                    {
                    rb.isKinematic = true;
                    collider.isTrigger = true;
                    gameObject.transform.position = boat.boatSeat.transform.position;
                        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                       // gameObject.transform.SetParent(boat.transform);


                    }
                    else
                    {
                        Debug.Log("gmgmg");
                    }
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red, 0.1f);
        }
        

    }
}
