using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [Tooltip("how close it needs to be to the camera for inventory detail")]
    public float zoomLevel = 1;
    [Tooltip("What rotation needed for inventory detail")]
    public Vector3 localRotation = new Vector3(0, 0, 0);
}
