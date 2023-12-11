using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSpawn : MonoBehaviour
{
    public float targetRotationAngleX;

    void Start()
    {
        AlignObjectAndChildren(transform);
    }
    

    void AlignObjectAndChildren(Transform currentTransform)
    {
        currentTransform.rotation = Quaternion.Euler(targetRotationAngleX, 0, 0);

        // ��� �ڽĿ� ���� ��������� ȣ��
        foreach (Transform child in currentTransform)
        {
            AlignObjectAndChildren(child);
        }
    }
}