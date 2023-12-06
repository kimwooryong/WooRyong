using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDestroy : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float raycastDistance; // 레이 최대 거리
    void Update()
    {
        PerformRaycast();
    }

    void PerformRaycast()
    {
        // 메인 카메라의 중앙에서 앞으로 레이 발사
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // 레이가 어떤 물체와 충돌했는지 확인
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // 레이가 충돌한 물체의 태그를 출력
            Debug.Log("Hit object with tag: " + hit.collider.gameObject.tag);

            GameObject destroy = hit.collider.gameObject;

            if (destroy.tag == "BuildDestroy")
            {
                Debug.Log("떴음");
                if (Input.GetKeyDown(KeyCode.E)) // 건축물 부수기
                {
                    Destroy(destroy);
                }
            }
        }
    }
}
