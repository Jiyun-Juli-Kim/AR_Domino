using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LockPlane : MonoBehaviour
{
    public ARPlaneManager arPlaneManager;
    public ARPlane lockedPlane;

    void OnEnable()
    {
        arPlaneManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        arPlaneManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (ARPlane plane in args.added)
        {
            LockFirstPlane(plane);
            break;
        }
    }

    void LockFirstPlane(ARPlane plane)
    {
        lockedPlane = plane;
        arPlaneManager.enabled = false;
        Debug.Log("Plane locked: " + plane.name);
    }
}
