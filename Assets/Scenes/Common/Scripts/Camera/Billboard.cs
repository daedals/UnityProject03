using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _cameraTransform = null;

    void LateUpdate()
    {
        transform.LookAt(transform.position + _cameraTransform.forward);
    }
}
