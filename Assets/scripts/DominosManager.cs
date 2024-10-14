using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DominosManager : MonoBehaviour
{
    public GameObject dominoPrefab; 
    public Transform dominoContainer; 
    public Button removeAllButton;
    public Button resetButton;
    public GameObject trashCan; 

    private ARRaycastManager arRaycastManager;
    private List<GameObject> placedDominos = new List<GameObject>();

    private GameObject selectedDomino = null;
    private Vector3 offset;
    private Camera arCamera;

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        removeAllButton.onClick.AddListener(RemoveAllDominos);
        resetButton.onClick.AddListener(ResetScene);
        arCamera = Camera.main;
    }

    void Update()
    {
        HandleTouch();
    }

    public void PlaceDomino(Vector3 position, Quaternion rotation)
    {
        GameObject domino = Instantiate(dominoPrefab, position, rotation, dominoContainer);
        placedDominos.Add(domino);
    }

    public void RemoveAllDominos()
    {
        foreach (var domino in placedDominos)
        {
            Destroy(domino);
        }
        placedDominos.Clear();
    }

    public void ResetScene()
    {
        RemoveAllDominos();
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScanScene");
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.All))
            {
                Pose hitPose = hits[0].pose;
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.CompareTag("Domino"))
                    {
                        selectedDomino = hit.collider.gameObject;
                        offset = selectedDomino.transform.position - hitPose.position;
                    }
                }
            }
        }
        else if (touch.phase == TouchPhase.Moved && selectedDomino != null)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                selectedDomino.transform.position = hitPose.position + offset;
            }
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            if (selectedDomino != null)
            {
                Vector3 trashPosition = trashCan.transform.position;
                float distance = Vector3.Distance(selectedDomino.transform.position, trashPosition);

                if (distance < 0.5f)
                {
                    Destroy(selectedDomino);
                    placedDominos.Remove(selectedDomino);
                }

                selectedDomino = null;
            }
        }
    }
}

