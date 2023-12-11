using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildDestroy : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera; // ���� ī�޶� ����
    [SerializeField]
    private float raycastDistance; // ���� �ִ� �Ÿ�

    [SerializeField]
    private TextMeshProUGUI BuildName;
    private GameObject crosshairInstance; // ũ�ν����
    public GameObject crosshairPrefab; // ũ�ν���� ������


    void Update()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        PerformRaycast();
    }

    void PerformRaycast()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); 
        RaycastHit hit;
        BuildName.gameObject.SetActive(false);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            Debug.Log("Hit object with tag: " + hit.collider.gameObject.tag); // ���̰� �浹�� ��ü�� �±׸� ���

            GameObject destroy = hit.collider.gameObject;

            if (destroy != null)
            {
                if (destroy.tag == "BuildDestroy")
                {
                    BuildName.gameObject.SetActive(true);
                    BuildName.text = destroy.name;
                    crosshairPrefab.gameObject.SetActive(true);
                    UpdateCrosshair(hit.point); // ũ�ν��� ǥ���ϰų� ������Ʈ
                    if (Input.GetKeyDown(KeyCode.F)) // ���๰ �μ���
                    {
                        Destroy(destroy);
                    }
                }

            }
            else
            {
                BuildName.gameObject.SetActive(false);
                crosshairPrefab.gameObject.SetActive(false);
            }
        }
    }

    void UpdateCrosshair(Vector3 position)
    {
        // ũ�ν��� ǥ���� ��ġ�� �ν��Ͻ��� �����ϰų� ��ġ ������Ʈ
        if (crosshairInstance == null)
        {
            crosshairInstance = Instantiate(crosshairPrefab, position, Quaternion.identity);
        }
        else
        {
            crosshairInstance.transform.position = position;
        }
    }
}
