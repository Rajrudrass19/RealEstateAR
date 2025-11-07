using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ARPlacementManager arPlacementManager;
    public GameObject explorePlayerPrefab;   // prefab for first-person player/camera
    private GameObject _explorePlayerInstance;

    // Called by "Place" button (optional if you use tap)
    public void OnPlacePressed()
    {
        arPlacementManager.PlaceObject();
    }

    public void OnResetPressed()
    {
        arPlacementManager.ResetPlacement();
    }

    public void OnScaleUpPressed()
    {
        if (arPlacementManager.IsPlaced())
        {
            var placed = arPlacementManager.GetPlacedObject();
            var mc = placed.GetComponent<ModelController>();
            if (mc != null) mc.ScaleUp();
        }
    }

    public void OnScaleDownPressed()
    {
        if (arPlacementManager.IsPlaced())
        {
            var placed = arPlacementManager.GetPlacedObject();
            var mc = placed.GetComponent<ModelController>();
            if (mc != null) mc.ScaleDown();
        }
    }

    public void OnRotateLeft()
    {
        if (arPlacementManager.IsPlaced())
        {
            var placed = arPlacementManager.GetPlacedObject();
            var mc = placed.GetComponent<ModelController>();
            if (mc != null) mc.RotateLeft();
        }
    }

    public void OnRotateRight()
    {
        if (arPlacementManager.IsPlaced())
        {
            var placed = arPlacementManager.GetPlacedObject();
            var mc = placed.GetComponent<ModelController>();
            if (mc != null) mc.RotateRight();
        }
    }

    // Enter Explore: spawn a player inside placed model
    public void OnEnterExplore()
    {
        if (!arPlacementManager.IsPlaced()) return;

        // Get center or a designated "entry point" inside the model
        var placed = arPlacementManager.GetPlacedObject();
        Transform entryPoint = placed.transform.Find("EntryPoint");
        Vector3 spawnPos = entryPoint != null ? entryPoint.position : placed.transform.position + Vector3.up * 1.2f;

        // Disable AR camera / AR controls by toggling AR Session Origin camera
        Camera arCam = Camera.main;
        if (arCam != null) arCam.gameObject.SetActive(false);

        _explorePlayerInstance = Instantiate(explorePlayerPrefab, spawnPos, Quaternion.identity);
    }

    public void OnExitExplore()
    {
        if (_explorePlayerInstance != null) Destroy(_explorePlayerInstance);

        // Re-enable AR camera
        Camera arCam = Camera.main;
        if (arCam != null) arCam.gameObject.SetActive(true);
    }
}