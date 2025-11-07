using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFirstPersonController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 2.0f;
    public Transform cameraTransform;

    private CharacterController _controller;
    private float _verticalVelocity = 0f;
    private float _xRotation = 0f;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouse look (for desktop testing). On mobile, you can wire touch look.
#if UNITY_STANDALONE || UNITY_EDITOR
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        cameraTransform.localEulerAngles = Vector3.right * _xRotation;
        transform.Rotate(Vector3.up * mouseX);
#endif

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.forward * v + transform.right * h;
        move *= moveSpeed;

        if (_controller.isGrounded && _verticalVelocity < 0)
            _verticalVelocity = -1f; // small grounded push

        _verticalVelocity += gravity * Time.deltaTime;
        move.y = _verticalVelocity;

        _controller.Move(move * Time.deltaTime);
    }

    public void SetMoveSpeed(float s) => moveSpeed = s;
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}