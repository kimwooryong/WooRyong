using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
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
    private PlayerCotroller playerCotroller;
    public float rayDistance = 3.0f;

    public int playerCurrentHp = 100;
    public int playerMaxHp = 100;
    public int animalDamage;
    public int TreeDamage;
    public int rockDamage;


    //나중에 다 private로
    public int theCurrentStateOfHunger = 100;
    public int theMaxHunger = 100;
    public float hungerDamageTimer = 0.0f;
    public float hungerTimer = 0.0f;
    public float fullHungerHpAddTimer = 0.0f;
    public bool attackHungerDamageCheck = false;
    public bool attackHungerCheck = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        playerCotroller = GetComponent<PlayerCotroller>();
        playerCurrentHp = playerMaxHp;
        theCurrentStateOfHunger = theMaxHunger;
        boat = FindObjectOfType<BoatRide>();
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

        if (miniMap == null)
        {
            GameObject[] foundObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in foundObjects)
            {
                if (obj.name == "Canvas_Minimap" && !obj.activeSelf)
                {
                    miniMap = obj;
                    miniMap.SetActive(false); // Set it inactive again
                    break;
                }
            }

            if (miniMap == null)
            {
                Debug.LogError("MiniMap GameObject not found. Make sure the GameObject is named 'Canvas_Minimap'.");
            }
        }

        StartCoroutine(HandleHunger());
    }
    void Update()
    {
       


        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.SetActive(!miniMap.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 rayOrigin = cameraLook.transform.position;
            Vector3 rayDirection = cameraLook.transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
            {
                if (hit.collider.CompareTag("Boat"))
                {

                    if (boat != null)
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
        if (boat != null)
        {
            if (boat.isRiding)
            {
                gameObject.transform.position = boat.boatSeat.transform.position;
                DestroyChild("_M_Base_Suit");
                DestroyChild("_M_Hands_C");
                DestroyChild("_M_Rig");
            }

        }

    }
    public void TakeDamage(int damage)
    {
        playerCurrentHp -= damage;
        if(playerCurrentHp <= 0)
        {
            playerCurrentHp = 0;
        }
    }
    public void ReductionInHunger(int reduction)
    {
        theCurrentStateOfHunger -= reduction;
        if(theCurrentStateOfHunger <= 0)
        {
            theCurrentStateOfHunger = 0;
        }
    }
    private void DestroyChild(string name)
    {
        Transform childName = gameObject.transform.Find(name);
        if (childName != null)
        {
            childName.gameObject.SetActive(false);
        }
    }
    IEnumerator HandleHunger()
    {
        while (true)
        {
            if (theCurrentStateOfHunger <= 0.0f)
            {
                theCurrentStateOfHunger = 0;
                if (hungerTimer != 0.0f)
                {
                    hungerTimer = 0.0f;
                }
                if (fullHungerHpAddTimer != 0.0f)
                {
                    fullHungerHpAddTimer = 0.0f;
                }
                if (playerCotroller.isAttacking)
                {
                    attackHungerDamageCheck = true;
                    if (attackHungerDamageCheck)
                    {
                        hungerDamageTimer += 0.5f;
                        attackHungerDamageCheck = false;
                        playerCotroller.isAttacking = false;
                    }
                }
                hungerDamageTimer += 0.5f;
                if (hungerDamageTimer >= 5.0f)
                {
                    TakeDamage(10);
                    hungerDamageTimer = 0.0f;
                }
            }
            else if (100.0f > theCurrentStateOfHunger && theCurrentStateOfHunger > 0.0f)
            {
                if (hungerDamageTimer != 0.0f)
                {
                    hungerDamageTimer = 0.0f;
                }
                if (fullHungerHpAddTimer != 0.0f)
                {
                    fullHungerHpAddTimer = 0.0f;
                }
                if (playerCotroller.isAttacking)
                {
                    attackHungerCheck = true;
                    if (attackHungerCheck)
                    {
                        hungerTimer += 1.0f;
                        attackHungerCheck = false;
                        playerCotroller.isAttacking = false;
                    }
                }

                hungerTimer += 1.0f;
                if (hungerTimer >= 30.0f)
                {
                    ReductionInHunger(5);
                    hungerTimer = 0.0f;
                }
            }
            else if (100.0f == theCurrentStateOfHunger)
            {
                if (playerCotroller.isAttacking)
                {
                    attackHungerCheck = true;
                    if (attackHungerCheck)
                    {
                        hungerTimer += 1.0f;
                        attackHungerCheck = false;
                        playerCotroller.isAttacking = false;
                    }
                }
                if (playerCurrentHp < 100)
                {
                    fullHungerHpAddTimer += 1.0f;
                    if (fullHungerHpAddTimer >= 10.0f)
                    {
                        playerCurrentHp += 10;
                        fullHungerHpAddTimer = 0.0f;
                    }
                }
                else if (playerCurrentHp >= 100)
                {
                    fullHungerHpAddTimer = 0.0f;
                }

                hungerTimer += 1.0f;
                if (hungerTimer >= 50.0f)
                {
                    ReductionInHunger(5);
                    hungerTimer = 0.0f;
                }
            }
            //if문 작성 먹어서 생기는 불값이던 추가값이 발생시 hungerTimer를 0으로 초기화
            yield return new WaitForSeconds(1.0f); // Adjust the interval as needed
        }
    }
    public void AddHunger(int amount)
    {
        theCurrentStateOfHunger += amount;
        if(theCurrentStateOfHunger >= 100)
        {
            theCurrentStateOfHunger = 100;
        }
    }
    
}
