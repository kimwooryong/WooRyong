using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public class PreviewObject : MonoBehaviour
{
    // �浹�� ������Ʈ�� �ݶ��̴�
    private List<Collider> colliderList = new List<Collider>();
    private List<Collider> snapColliderList = new List<Collider>();

    [SerializeField]
    private int layerGround; // ���� ���̾�
    // private int foundation; // ��� ���̾�
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green; // �ʷϻ�
    [SerializeField]
    private Material Red; // ������

    private List<Renderer> _renderers = new List<Renderer>();
    private List<GameObject> _gameObjects = new List<GameObject>();

    private void Start() // ������Ʈ�� ��� �������� �޾ƿ��� �ڵ�
    {
        _renderers.Add(transform.GetComponent<Renderer>());
        foreach (Transform tfChild in transform) // �ڽ� ������Ʈ���Ե� �������� �Ѱ��ش�.
            _renderers.Add(tfChild.GetComponent<Renderer>());
    }



    void Update()
    {
        ChangeColor(); // ���� �ٲٱ� �ʷ�, ����
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0) // ��� �ݶ��̴��� 1�� �̻��̸� ������
        {
            SetColor(Red);
        }
        else
        {
            SetColor(green); // ��� �ݶ��̴��� ���ٸ� �ʷϻ�
        }
    }

    private void SetColor(Material mat) // ���� �ٲ��ִ� �Լ�
    {
        foreach (var renderer in _renderers)
            renderer.material = mat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.GetComponent<SnapObject>() != null)
        {
            snapColliderList.Add(other);    // Snap �ݶ��̴��� �߰�
            return;
        }

        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Add(other); // �ݶ��̴� ����Ʈ �߰�
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.GetComponent<SnapObject>() != null)
        {
            snapColliderList.Remove(other);    // Snap �ݶ��̴��� �߰�
            return;
        }

        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other); // �ݶ��̴� ����Ʈ ����
        }
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }

    void Test() // ���̿� ���� ��ü�� �ݶ��̴� ũ�� �޾ƿ���
    {
        // ���̸� ��� ���� ������ ���� ����
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        // ����ĳ��Ʈ�� ���� ���� ��ü ���� ����
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit))
        {
            // ���� ��ü�� �ݶ��̴� ��������
            Collider collider = hit.collider;

            // �ݶ��̴��� ũ�� ���
            Vector3 colliderSize = collider.bounds.size;
            Debug.Log("���� ��ü�� �ݶ��̴� ũ��: " + colliderSize);
        }
    }


    // �̰͵��� �ϳ��� ���Ͱ� * ���� ���� ������
    public Vector3 GetSnapPosition(Vector3 currentPosition, Transform snapTransform) // ��ġ ����
    {
        // ����
        Vector3 right = snapTransform.position + snapTransform.right * transform.localScale.z;
        float fDist = Vector3.Distance(currentPosition, right);
        float fDistShortest = fDist;
        Vector3 closestPosition = right;

        // ����
        Vector3 left = snapTransform.position - snapTransform.right.normalized * transform.localScale.z;
        fDist = Vector3.Distance(currentPosition, left);
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = left;
            Debug.Log("Left");
        }

        // ��
        Vector3 front = snapTransform.position + snapTransform.forward.normalized * transform.localScale.z;
        fDist = Vector3.Distance(currentPosition, front);
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = front;
            Debug.Log("front");
        }

        // ��
        Vector3 back = snapTransform.position - snapTransform.forward.normalized * transform.localScale.z;
        fDist = Vector3.Distance(currentPosition, back);
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = back;
            Debug.Log("back");
        }

        // ��
        Vector3 up = snapTransform.position + snapTransform.up.normalized * transform.localScale.y ;
        fDist = Vector3.Distance(currentPosition, up);
        if (fDist < fDistShortest)
        {
            fDistShortest = fDist;
            closestPosition = up;
            Debug.Log("up");
        }

        // �Ʒ�
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
    }
}
