using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

[System.Serializable]
public class craft
{
    public string craftName; // 이름
    public GameObject go_Prefab; // 실제 설치되는 프리펩
    public GameObject go_PreviewPrefab; // 미리보기 프리렙
}

public class prefabs
{

}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false; // 상태변수 꺼두고 시작
    private bool isPreviewActived = false; // 프리뷰 활성화

    public Dictionary<int, GameObject> buildBlocks = new Dictionary<int, GameObject>();


    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스


    [SerializeField]
    public craft[] craft_fire; // 모닥불용 탭

    
    private GameObject go_Preview; // 미리보기 프리펩을 담을 변수

    private PreviewObject previewObject; // 클래스 참조

    private Quaternion savedRotation;


    private GameObject go_Prefab; // 실체 생성될 프리펩을 담을 변수


    [SerializeField]
    private Transform tf_Player; // 플레이어 위치


    // RaycastHit 필요 변수 선언
    private RaycastHit hitInfo; // 정보
    [SerializeField]
    private LayerMask layerMask; // 레이어 마스크 설정
    [SerializeField]
    private float range; // 거리


    public void SlotClick(int _slotNumber) // 슬롯 클릭
    {
        Debug.Log("0");
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        previewObject = go_Preview.GetComponent<PreviewObject>();

        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreviewActived = true; // 프리뷰 켜주기
        go_BaseUI.SetActive(false); // UI 꺼주기
        
    }

    void Update()
    {
        RayObject(); // 게임 오브젝트 레이 확인용
        if (go_Preview != null)
        {
            savedRotation = go_Preview.transform.rotation;
            go_Prefab.transform.rotation = go_Preview.transform.rotation;
        }
        InputHandler();
    }

    private void LateUpdate()
    {
        if (isPreviewActived) // 프리뷰 활성화
        {
            PreviewPositionUpdate(); // 프리뷰 움직임
        }
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

    private void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isPreviewActived) // UI열기 중복열림 방지
        {
            Window(); // UI
        }

        if (Input.GetButtonDown("Fire1")) // 마우스 좌클릭
        {
            Build(); // 설치
        }

        if (Input.GetMouseButtonDown(1))
        {
            Cancel(); // 취소
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel(); // 취소
        }

        if (Input.GetKey(KeyCode.Q))
        {
            if (go_Preview != null)
            {
                go_Preview.transform.Rotate(new Vector3(0f, 90f * Time.deltaTime, 0f)); // 회전
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (go_Preview != null)
            {
                go_Preview.transform.Rotate(new Vector3(0f, -90f * Time.deltaTime, 0f)); // 회전
            }
        }

        if (Input.GetKey(KeyCode.K))
        {
            if (go_Preview != null)
            {
                go_Preview.transform.Rotate(new Vector3(0f, 0f, -90f * Time.deltaTime)); // 회전
            }
        }
    }

    private void Build() // 건설
    {
        if (isPreviewActived == true && previewObject.isBuildable() == true)
        {
            Vector3 buildPosition = go_Preview.transform.position;
            Quaternion buildRotation = go_Preview.transform.rotation;

            GameObject newObject = Instantiate(go_Prefab, buildPosition, buildRotation); // 저장된 위치값, 로테이션값을 받기
            Destroy(go_Preview); // 프리뷰 오브젝트 삭제
            isActivated = false; // 상태변수 꺼두고 시작
            isPreviewActived = false; // 프리뷰 꺼주기
            go_Prefab = null;
            go_Preview = null;
            previewObject = null;
        }
    }

    private void PreviewPositionUpdate() // 프리뷰 설치전 움직임
    {
        Debug.Log("1");
        Debug.DrawRay(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, Color.red); // 씬 창에 레이선이 보이게 해주는 코드
        if(Physics.Raycast(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, out hitInfo, range, layerMask)) // 카메라 앞으로 레이를 쏘고 위치값을 보내주고 레이어 마스크를 확인한다
        {
            if(hitInfo.transform != null) // 위치값이 있다면
            {
                Quaternion rotation = go_Preview.transform.rotation;
                Vector3 _location = previewObject.GetSnapPosition(hitInfo.point, ref rotation); 
                Debug.Log("4");
                go_Preview.transform.position = _location; // 저장된 위치값에 프리뷰를 보여준다
                go_Preview.transform.rotation = rotation;
            }
        }
    }

  
        // 제희 피셜 확인하면 프리뷰 오브젝트가 겹치지않게 만들 수 있다. ( 설치 가능 상태일때 넣어주면 좋을듯 )
        // BoxCollider box;
        // box.bounds.extents.x;
    

    private void Cancel() // 취소
    {
        if (isPreviewActived)
        {
            Destroy(go_Preview);
        }
        isActivated = false;
        isPreviewActived= false;
        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }

    private void Window()
    {
        if(!isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }

    private void OpenWindow()
    {
        Cursor.lockState = CursorLockMode.None; // 마우스 고정 해제
        Cursor.visible = true; // 마우스 클릭가능
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
        Cursor.visible = false; // 마우스 클릭불가
        isActivated = false;
        go_BaseUI.SetActive(false);
    }

}
