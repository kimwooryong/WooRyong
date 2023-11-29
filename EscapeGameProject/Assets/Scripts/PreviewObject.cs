using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public class PreviewObject : MonoBehaviour
{
    // 충돌한 오브젝트의 콜라이더
    private List<Collider> colliderList = new List<Collider>();
    private List<Collider> snapColliderList = new List<Collider>();

    [SerializeField]
    private int layerGround; // 지상 레이어
    // private int foundation; // 토대 레이어
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green; // 초록색
    [SerializeField]
    private Material Red; // 빨간색

    private List<Renderer> _renderers = new List<Renderer>();
    private List<GameObject> _gameObjects = new List<GameObject>();

    private void Start() // 오브젝트의 모든 랜더러를 받아오는 코드
    {
        _renderers.Add(transform.GetComponent<Renderer>());
        foreach (Transform tfChild in transform) // 자식 오브젝트에게도 렌더러를 넘겨준다.
            _renderers.Add(tfChild.GetComponent<Renderer>());
    }



    void Update()
    {
        ChangeColor(); // 색깔 바꾸기 초록, 빨강
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0) // 닿는 콜라이더가 1개 이상이면 빨간색
        {
            SetColor(Red);
        }
        else
        {
            SetColor(green); // 닿는 콜라이더가 없다면 초록색
        }
    }

    private void SetColor(Material mat) // 색깔 바꿔주는 함수
    {
        foreach (var renderer in _renderers)
            renderer.material = mat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.GetComponent<SnapObject>() != null)
        {
            snapColliderList.Add(other);    // Snap 콜라이더로 추가
            return;
        }

        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Add(other); // 콜라이더 리스트 추가
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.GetComponent<SnapObject>() != null)
        {
            snapColliderList.Remove(other);    // Snap 콜라이더로 추가
            return;
        }

        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other); // 콜라이더 리스트 삭제
        }
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }

    void Test() // 레이에 맞은 물체의 콜라이더 크기 받아오기
    {
        // 레이를 쏘기 위한 원점과 방향 설정
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        // 레이캐스트를 통해 맞은 객체 정보 저장
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit))
        {
            // 맞은 객체의 콜라이더 가져오기
            Collider collider = hit.collider;

            // 콜라이더의 크기 출력
            Vector3 colliderSize = collider.bounds.size;
            Debug.Log("맞은 객체의 콜라이더 크기: " + colliderSize);
        }
    }


    // 이것들을 하나의 벡터값 * 방향 으로 만들어라
    public Vector3 GetSnapPosition(Vector3 currentPosition, Transform snapTransform) // 위치 설정
    {
        // 우측
        Vector3 right = snapTransform.position + snapTransform.right * transform.localScale.z;
        float fDist = Vector3.Distance(currentPosition, right);
        float fDistShortest = fDist;
        Vector3 closestPosition = right;

        // 좌측
        Vector3 left = snapTransform.position - snapTransform.right.normalized * transform.localScale.z;
        fDist = Vector3.Distance(currentPosition, left);
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = left;
            Debug.Log("Left");
        }

        // 앞
        Vector3 front = snapTransform.position + snapTransform.forward.normalized * transform.localScale.z;
        fDist = Vector3.Distance(currentPosition, front);
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = front;
            Debug.Log("front");
        }

        // 뒤
        Vector3 back = snapTransform.position - snapTransform.forward.normalized * transform.localScale.z;
        fDist = Vector3.Distance(currentPosition, back);
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = back;
            Debug.Log("back");
        }

        // 위
        Vector3 up = snapTransform.position + snapTransform.up.normalized * transform.localScale.y ;
        fDist = Vector3.Distance(currentPosition, up);
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = up;
            Debug.Log("up");
        }

        // 아래
        Vector3 down = snapTransform.position - snapTransform.up.normalized * transform.localScale.y;
        fDist = Vector3.Distance(currentPosition, down);
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = down;
            Debug.Log("down");
        }


        Debug.Log("9" + closestPosition);
        return closestPosition;
    }

    public Vector3 GetSnapPosition(Vector3 currentPosition, ref Quaternion snapRotation)
    {
        Debug.Log("2");
        
        Transform snapTransform = GetClosestCollider();
        if (snapTransform != null)
        {
            Vector3 snapPosition = GetSnapPosition(currentPosition, snapTransform);
            Debug.Log("snapPosition" + snapPosition);

            float dist = Vector3.Distance(snapPosition, currentPosition);
            if (dist < 1f)
            {
                snapRotation = snapTransform.rotation;
                return snapPosition;
            }
        }
        return currentPosition;
    }

    public Transform GetClosestCollider()
    {
        if (snapColliderList.Count == 0)
            return null;

        Collider closestCollider = null;
        float closestDist = -1.0f;
        foreach (Collider collider in snapColliderList)
        {
            SnapObject snapObject = collider.GetComponent<SnapObject>();
            if (snapObject == null)
            {
                continue;
            }

            float dist = Vector3.Distance(this.transform.position, collider.transform.position);

            // 비어있을땐 부딪힌 것 아무거나 가까운 물체로 인식
            if (closestCollider == null)
            {
                closestCollider = collider;
                closestDist = dist;
                continue;
            }

            // 제일 가까운 것이 체크되도록
            if (dist < closestDist)
            {
                closestCollider = collider;
                closestDist = dist;
            }
        }

        if (closestCollider == null)
        {
            return null;
        }

        return closestCollider.transform;
    }
}
