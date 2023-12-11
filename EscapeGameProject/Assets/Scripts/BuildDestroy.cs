using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildDestroy : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera; // 메인 카메라 지정
    [SerializeField]
    private float raycastDistance; // 레이 최대 거리

    [SerializeField]
    private TextMeshProUGUI BuildName;
    private GameObject crosshairInstance; // 크로스헤어
    public GameObject crosshairPrefab; // 크로스헤어 프리팹


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
            Debug.Log("Hit object with tag: " + hit.collider.gameObject.tag); // 레이가 충돌한 물체의 태그를 출력

            GameObject destroy = hit.collider.gameObject;

            if (destroy != null)
            {
                if (destroy.tag == "BuildDestroy")
                {
                    BuildName.gameObject.SetActive(true);
                    BuildName.text = destroy.name;
                    crosshairPrefab.gameObject.SetActive(true);
                    UpdateCrosshair(hit.point); // 크로스헤어를 표시하거나 업데이트
                    if (Input.GetKeyDown(KeyCode.F)) // 건축물 부수기
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
        // 크로스헤어를 표시할 위치에 인스턴스를 생성하거나 위치 업데이트
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
