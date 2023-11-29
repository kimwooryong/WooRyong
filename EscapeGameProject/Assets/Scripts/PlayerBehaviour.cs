using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    void Update()
    {
        RayObject();
    }

    public float maxRaycast; // 오브젝트 확인 레이 거리 
    public GameObject crosshairPrefab; // 크로스헤어 프리팹
    private GameObject crosshairInstance; // 크로스헤어

    private void RayObject() // 은하씨 드릴 레이 아이템 확인용
    {
        Debug.DrawRay(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, Color.blue); // 씬 창에 레이선이 보이게 해주는 코드
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // 카메라 기준 가운데에 레이 발사

        RaycastHit hit; // 맞은거

        if (Physics.Raycast(ray, out hit, maxRaycast))
        {
            Debug.Log("맞은 오브젝트 " + hit.collider.gameObject.name); // 레이에 충돌한 오브젝트의 이름을 표시

            crosshairPrefab.gameObject.SetActive(true);

            Vector3 crosshairPosition = hit.point; // 크로스헤어를 표시할 위치 설정

            UpdateCrosshair(crosshairPosition); // 크로스헤어를 표시하거나 업데이트
        }
        else
        {
            Debug.Log("HideCrosshair");
            crosshairPrefab.gameObject.SetActive(false);
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
