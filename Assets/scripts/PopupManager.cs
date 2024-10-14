using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class PopupManager : MonoBehaviour
{
    public GameObject startScanButton;
    public GameObject stopScanButton;
    public GameObject scanPopup;
    public ARPlaneManager arPlaneManager;
    private bool planeLocked = false;

    void Start()
    {
        startScanButton.SetActive(true);
        scanPopup.SetActive(false);
        stopScanButton.SetActive(false);
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arPlaneManager.enabled = false;
    }

    public void StartScan()
    { 
        startScanButton.SetActive(false);
        stopScanButton.SetActive(true);
        scanPopup.SetActive(false);
        arPlaneManager.enabled = true;
    }

    public void StopScan()
    {
        stopScanButton.SetActive(false);
        arPlaneManager.enabled = false;
        scanPopup.SetActive(true);
    }

    void OnEnable()
    {
        if (arPlaneManager != null)
            arPlaneManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        if (arPlaneManager != null)
            arPlaneManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (planeLocked)
            return;

        if (args.added.Count > 0)
        {
            planeLocked = true;
            GameManager.Instance.SavePlane(args.added[0]);
            StopScan();
        }
    }

    public void Rescan()
    {
        if (arPlaneManager != null)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }

            arPlaneManager.enabled = false;
            arPlaneManager.enabled = true;
        }

        startScanButton.SetActive(true);
        stopScanButton.SetActive(false);
        scanPopup.SetActive(false);
        planeLocked = false;

        GameManager.Instance.ClearPlane();
    }

    /* 
        Rescan했을때 왜떄무네 plane 새로 인식이 안되나여
        plane manager를       
    */

    public void Play()
    {
        SceneManager.LoadScene("ConstructScene");
    }
}
