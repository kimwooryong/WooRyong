using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPreview : MonoBehaviour
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
        if (other.gameObject.transform.GetComponent<WallSnap>() != null)
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
    }


    public Transform GetClosestCollider()
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
    }
}

