using UnityEngine;

public class ModelController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float pinchZoomSpeed = 0.01f;
    public float minScale = 0.3f;
    public float maxScale = 3.0f;

    private Transform _target;
    private float _initialDistance;
    private Vector3 _initialScale;

    void Start()
    {
        _target = this.transform; // Attach to the model root
    }

    void Update()
    {
        HandleTouchGestures();
    }

    private void HandleTouchGestures()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            // Single-finger drag rotates model around Y axis
            if (t.phase == TouchPhase.Moved)
            {
                float rotateAmount = -t.deltaPosition.x * rotationSpeed * Time.deltaTime * 0.1f;
                _target.Rotate(0, rotateAmount, 0, Space.World);
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            // Pinch to scale
            if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
            {
                _initialDistance = Vector2.Distance(t0.position, t1.position);
                _initialScale = _target.localScale;
            }
            else
            {
                float currentDist = Vector2.Distance(t0.position, t1.position);
                float delta = currentDist - _initialDistance;
                float scaleFactor = 1 + (delta * pinchZoomSpeed);
                Vector3 newScale = _initialScale * scaleFactor;
                newScale = ClampVector3(newScale, minScale, maxScale);
                _target.localScale = newScale;
            }

            // Two-finger twist rotate (optional)
            float angle = AngleBetweenTouches(t0, t1);
            // apply twist if needed (left as an exercise)
        }
    }

    private float AngleBetweenTouches(Touch t0, Touch t1)
    {
        Vector2 a = t0.position - t1.position;
        return Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
    }

    private Vector3 ClampVector3(Vector3 v, float min, float max)
    {
        v.x = Mathf.Clamp(v.x, min, max);
        v.y = Mathf.Clamp(v.y, min, max);
        v.z = Mathf.Clamp(v.z, min, max);
        return v;
    }

    // UI-driven scale & rotate helpers
    public void ScaleUp(float step = 0.05f)
    {
        _target.localScale = ClampVector3(_target.localScale + Vector3.one * step, minScale, maxScale);
    }

    public void ScaleDown(float step = 0.05f)
    {
        _target.localScale = ClampVector3(_target.localScale - Vector3.one * step, minScale, maxScale);
    }

    public void RotateLeft(float degrees = 15f)
    {
        _target.Rotate(0, -degrees, 0, Space.World);
    }

    public void RotateRight(float degrees = 15f)
    {
        _target.Rotate(0, degrees, 0, Space.World);
    }

    public void ResetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        _target.position = position;
        _target.rotation = rotation;
        _target.localScale = scale;
    }
}