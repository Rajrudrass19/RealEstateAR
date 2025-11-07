using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Events;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlacementManager : MonoBehaviour
{
    [Header("Prefabs & UI")]
    public GameObject placementPrefab;       // 3D building prefab (with collider)
    public GameObject placementReticle;      // visual reticle prefab (simple quad/indicator)
    public Camera arCamera;

    [Header("Options")]
    public float placementRotationOffset = 0f;

    [Header("Events")]
    public UnityEvent OnPlaced;              // hooked for UI or sound

    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;
    private GameObject _spawnedObject;
    private bool _isPlaced = false;
    private Pose _lastPose;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = FindObjectOfType<ARPlaneManager>();
        if (arCamera == null) arCamera = Camera.main;
    }

    void Update()
    {
        if (_isPlaced)
        {
            placementReticle.SetActive(false);
            return;
        }

        // Raycast from screen center
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        if (_raycastManager.Raycast(screenCenter, s_Hits, TrackableType.Planes))
        {
            _lastPose = s_Hits[0].pose;
            placementReticle.SetActive(true);
            placementReticle.transform.SetPositionAndRotation(_lastPose.position, _lastPose.rotation);
        }
        else
        {
            placementReticle.SetActive(false);
        }

        // Tap to place
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (placementReticle.activeSelf)
            {
                PlaceObject();
            }
        }
    }

    public void PlaceObject()
    {
        if (_isPlaced || placementPrefab == null) return;

        _spawnedObject = Instantiate(placementPrefab, _lastPose.position, Quaternion.Euler(0, _lastPose.rotation.eulerAngles.y + placementRotationOffset, 0));
        _isPlaced = true;

        // Optional: disable plane detection to reduce noise
        if (_planeManager != null)
        {
            foreach (var plane in _planeManager.trackables)
                plane.gameObject.SetActive(false);
            _planeManager.enabled = false;
        }

        placementReticle.SetActive(false);
        OnPlaced?.Invoke();
    }

    public void ResetPlacement()
    {
        if (_spawnedObject != null) Destroy(_spawnedObject);
        _isPlaced = false;
        if (_planeManager != null) _planeManager.enabled = true;
    }

    public bool IsPlaced() => _isPlaced;

    public GameObject GetPlacedObject() => _spawnedObject;
}