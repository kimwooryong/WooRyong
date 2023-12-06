using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDestroy : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float raycastDistance; // ���� �ִ� �Ÿ�
    void Update()
    {
        PerformRaycast();
    }

    void PerformRaycast()
    {
        // ���� ī�޶��� �߾ӿ��� ������ ���� �߻�
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // ���̰� � ��ü�� �浹�ߴ��� Ȯ��
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // ���̰� �浹�� ��ü�� �±׸� ���
            Debug.Log("Hit object with tag: " + hit.collider.gameObject.tag);

            GameObject destroy = hit.collider.gameObject;

            if (destroy.tag == "BuildDestroy")
            {
                Debug.Log("����");
                if (Input.GetKeyDown(KeyCode.E)) // ���๰ �μ���
                {
                    Destroy(destroy);
                }
            }
        }
    }
}
