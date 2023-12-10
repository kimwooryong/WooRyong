using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    private float targetRotationAngleX = -90;

    void Start()
    {
        AlignObjectAndChildren(transform);
    }


    void AlignObjectAndChildren(Transform currentTransform)
    {
        currentTransform.rotation = Quaternion.Euler(targetRotationAngleX, 0, 0);
    }
}
