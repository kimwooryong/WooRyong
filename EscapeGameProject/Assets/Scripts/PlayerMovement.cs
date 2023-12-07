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
    public GameObject miniMap;
    //public bool onMove;
    private GameManager gameManager;
    private CaveEntrance cave;
    private CameraLook cameraLook;
    private BoatRide boat;
    public float rayDistance = 3.0f;

    [SerializeField]
    private int playerCurrentHp = 100;
    private int playerMaxHp = 100;
    public int playerDamage = 2;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        boat = FindObjectOfType<BoatRide>();
        playerCurrentHp = playerMaxHp;
        cameraLook = GetComponentInChildren<CameraLook>();

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
            Vector3 rayOrigin = cameraLook.transform.position;
            Vector3 rayDirection = cameraLook.transform.forward;
            RaycastHit hit;

            if(Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
            {
                if (hit.collider.CompareTag("Boat"))
                {

                    if(boat != null)
                    {
                    rb.isKinematic = true;
                    collider.isTrigger = true;
                    gameObject.transform.position = boat.boatSeat.transform.position;
                        gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                        boat.isRiding = true;
                        // gameObject.transform.SetParent(boat.transform);


                    }

                }
            }
        }
        if (boat == null)
        {
            boat = FindObjectOfType<BoatRide>();
        }
}
    private void FixedUpdate()
    {

            if (boat.isRiding)
            {
                gameObject.transform.position = boat.boatSeat.transform.position;
                DestroyChild("_M_Base_Suit");
                DestroyChild("_M_Hands_C");
                DestroyChild("_M_Rig");
            }

    }
    public void TakeDamage(int damage)
    {
        playerCurrentHp -= damage;
    }
    private void DestroyChild(string name)
    {
        Transform childName = gameObject.transform.Find(name);
        if (childName != null)
        {
            childName.gameObject.SetActive(false);
        }
    }
}
