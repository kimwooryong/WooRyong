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

    public float maxRaycast; // ������Ʈ Ȯ�� ���� �Ÿ� 
    public GameObject crosshairPrefab; // ũ�ν���� ������
    private GameObject crosshairInstance; // ũ�ν����

    private void RayObject() // ���Ͼ� �帱 ���� ������ Ȯ�ο�
    {
        Debug.DrawRay(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, Color.blue); // �� â�� ���̼��� ���̰� ���ִ� �ڵ�
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // ī�޶� ���� ����� ���� �߻�

        RaycastHit hit; // ������

        if (Physics.Raycast(ray, out hit, maxRaycast))
        {
            Debug.Log("���� ������Ʈ " + hit.collider.gameObject.name); // ���̿� �浹�� ������Ʈ�� �̸��� ǥ��

            crosshairPrefab.gameObject.SetActive(true);

            Vector3 crosshairPosition = hit.point; // ũ�ν��� ǥ���� ��ġ ����

            UpdateCrosshair(crosshairPosition); // ũ�ν��� ǥ���ϰų� ������Ʈ
        }
        else
        {
            Debug.Log("HideCrosshair");
            crosshairPrefab.gameObject.SetActive(false);
        }
    }
    void UpdateCrosshair(Vector3 position)
    {
        // ũ�ν��� ǥ���� ��ġ�� �ν��Ͻ��� �����ϰų� ��ġ ������Ʈ
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
