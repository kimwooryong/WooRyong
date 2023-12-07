using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
    Collider cookCollider;

    private void Awake()
    {
        cookCollider = GetComponent<Collider>();

    }
}
