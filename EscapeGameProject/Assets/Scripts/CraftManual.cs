using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class craft
{
    public string craftName; // 이름
    public GameObject go_Prefab; // 실제 설치되는 프리펩
    public GameObject go_PreviewPrefab; // 미리보기 프리렙
}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false; // 상태변수 꺼두고 시작
    private bool isPreviewActived = false; // 프리뷰 활성화

    public Dictionary<int, GameObject> buildBlocks = new Dictionary<int, GameObject>();


    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스


    [SerializeField]
    public craft[] craft_Build; // 용 탭


    private GameObject go_Preview; // 미리보기 프리펩을 담을 변수


    private PreviewObject previewObject; // 클래스 참조


    private Quaternion savedRotation;


    private GameObject go_Prefab; // 실체 생성될 프리펩을 담을 변수

    [SerializeField]
    private LayerMask layerMask;


    [SerializeField]
    private Transform tf_Player; // 플레이어 위치


    [SerializeField]
    private GameObject Fire_Base;
    [SerializeField]
    private GameObject Craft_Base;

    // RaycastHit 필요 변수 선언
    private RaycastHit hitInfo; // 정보
    [SerializeField]
    private float range; // 거리


    public void SlotClick(int _slotNumber) // 슬롯 클릭
    {
        go_Preview = Instantiate(craft_Build[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        previewObject = go_Preview.GetComponent<PreviewObject>();

        go_Prefab = craft_Build[_slotNumber].go_Prefab;
        isPreviewActived = true; // 프리뷰 켜주기
        go_BaseUI.SetActive(false); // UI 꺼주기


    }

    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel(); // 취소
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (go_Preview != null)
            {
                go_Preview.transform.Rotate(new Vector3(0f, 45f , 0f)); // 회전
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (go_Preview != null)
            {
                go_Preview.transform.Rotate(new Vector3(0f, -45f, 0f)); // 회전
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
            CloseWindow();

        }
    }

    private void PreviewPositionUpdate() // 프리뷰 설치전 움직임
    {
        Debug.DrawRay(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, Color.red); // 씬 창에 레이선이 보이게 해주는 코드
        if (Physics.Raycast(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, out hitInfo, range, layerMask)) // 카메라 앞으로 레이를 쏘고 위치값을 보내주고 레이어 마스크를 확인한다
        {
            if (hitInfo.transform != null) // 위치값이 있다면
            {
                Quaternion rotation = go_Preview.transform.rotation;
                Vector3 _location = previewObject.GetSnapPosition(hitInfo.point, ref rotation);
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
        go_BaseUI.SetActive(false);
        isActivated = false;
        isPreviewActived = false;
        go_Preview = null;
        go_Prefab = null;
        GameManager.Instance.InvisibleCursor();
        Fire_Base.gameObject.SetActive(false);
        Craft_Base.gameObject.SetActive(false);

    }

    private void Window()
    {
        if (!isActivated)
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
        GameManager.Instance.VisibleCursor();
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        GameManager.Instance.InvisibleCursor();
        isActivated = false;
        go_BaseUI.SetActive(false);

    }

}