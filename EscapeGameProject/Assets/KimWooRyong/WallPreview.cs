using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class WallPreview : PreviewObject
{
    [SerializeField]
    private int layerFoundation; // ���� ���̾�
    // private int foundation; // ��� ���̾�
    private const int IGNORE_RAYCAST_LAYER = 16;


    *//*public override void AAA()
    {
        base.AAA();
        Debug.Log("�� ��ӹ޾���~");
    }*//*


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.GetComponent<WallSnap>() != null)
        {
            snapColliderList.Add(other);    // Snap �ݶ��̴��� �߰�
            return;
        }

        if (other.gameObject.layer != layerFoundation && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Add(other); // �ݶ��̴� ����Ʈ �߰�
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.GetComponent<WallSnap>() != null)
        {
            snapColliderList.Remove(other);    // Snap �ݶ��̴��� �߰�
            return;
        }

        if (other.gameObject.layer != layerFoundation && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other); // �ݶ��̴� ����Ʈ ����
        }
    }

    public override Vector3 GetSnapPosition(Vector3 currentPosition, Transform snapTransform) // ��ġ ����
    {
        Vector3 up = snapTransform.position + snapTransform.up.normalized * transform.localScale.y;
        float fDist = Vector3.Distance(currentPosition, up);
        float fDistShortest = fDist;
        Vector3 closestPosition = up;
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = up;
            Debug.Log("���� ����");
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

            // ��������� �ε��� �� �ƹ��ų� ����� ��ü�� �ν�
            if (closestCollider == null)
            {
                closestCollider = collider;
                closestDist = dist;
                continue;
            }

            // ���� ����� ���� üũ�ǵ���
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

