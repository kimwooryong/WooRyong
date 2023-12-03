using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class WallPreview : PreviewObject
{
    [SerializeField]
    private int layerFoundation; // 지상 레이어
    // private int foundation; // 토대 레이어
    private const int IGNORE_RAYCAST_LAYER = 16;


    *//*public override void AAA()
    {
        base.AAA();
        Debug.Log("를 상속받았지~");
    }*//*


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.GetComponent<WallSnap>() != null)
        {
            snapColliderList.Add(other);    // Snap 콜라이더로 추가
            return;
        }

        if (other.gameObject.layer != layerFoundation && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Add(other); // 콜라이더 리스트 추가
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.GetComponent<WallSnap>() != null)
        {
            snapColliderList.Remove(other);    // Snap 콜라이더로 추가
            return;
        }

        if (other.gameObject.layer != layerFoundation && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other); // 콜라이더 리스트 삭제
        }
    }

    public override Vector3 GetSnapPosition(Vector3 currentPosition, Transform snapTransform) // 위치 설정
    {
        Vector3 up = snapTransform.position + snapTransform.up.normalized * transform.localScale.y;
        float fDist = Vector3.Distance(currentPosition, up);
        float fDistShortest = fDist;
        Vector3 closestPosition = up;
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = up;
            Debug.Log("안착 성공");
        }
        return closestPosition;
    }

    public Vector3 GetSnapPosition(Vector3 currentPosition)
    {
        Transform snapTransform = GetClosestCollider();
        if (snapTransform != null)
        {
            Vector3 snapPosition = GetSnapPosition(currentPosition);
            Debug.Log("snapPosition" + snapPosition);

            float dist = Vector3.Distance(snapPosition, currentPosition);
            if (dist < 1f)
            {
                return snapPosition;
            }
        }
        return currentPosition;
    }*/


   /* public Transform GetClosestCollider()
    {
        if (snapColliderList.Count == 0)
            return null;

        Collider closestCollider = null;
        float closestDist = -1.0f;
        foreach (Collider collider in snapColliderList)
        {
            WallSnap wallSnap = collider.GetComponent<WallSnap>();
            if (wallSnap == null)
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
    }*/
//}

