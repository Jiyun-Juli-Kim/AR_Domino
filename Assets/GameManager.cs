using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ARPlane SavedPlane;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlane(ARPlane plane)
    {
        if (SavedPlane == null)
        {
            SavedPlane = plane;
        }
    }

    public void ClearPlane()
    {
        SavedPlane = null;
    }
}
