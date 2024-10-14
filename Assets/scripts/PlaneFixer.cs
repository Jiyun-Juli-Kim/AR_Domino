using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneFixer : MonoBehaviour
{
    public ARPlaneManager arPlaneManager;

    void Start()
    {
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        DisablePlaneDetection();
        CreateFixedPlane();
    }

    void DisablePlaneDetection()
    {
        if (arPlaneManager != null)
        {
            arPlaneManager.enabled = false;
        }
    }

    void CreateFixedPlane()
    {
        if (GameManager.Instance.SavedPlane != null)
        {
            ARPlane savedPlane = GameManager.Instance.SavedPlane;
            Vector3 position = savedPlane.transform.position;
            Quaternion rotation = savedPlane.transform.rotation;
            Vector3 scale = savedPlane.transform.localScale;

            GameObject fixedPlane = Instantiate(savedPlane.gameObject, position, rotation);
            fixedPlane.transform.localScale = scale;

            fixedPlane.GetComponent<ARPlane>().enabled = false;
            fixedPlane.GetComponent<MeshRenderer>().enabled = true;
            fixedPlane.GetComponent<MeshCollider>().enabled = false;
        }
    }
}

