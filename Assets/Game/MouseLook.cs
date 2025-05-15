using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("�����")]
    public Transform playerBody; // �����, �������� �� ������������
    public Transform cameraHolder; // ������ ��� ������ ������ ��� ������ (�� ����������� �������)

    [Header("����������������")]
    public float sensitivity = 0.2f;
    public float smoothing = 5f;

    private Vector2 lookInput;
    private float smoothLookX;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ������ �������������� ������� (��� X)
        float targetLookX = lookInput.x * sensitivity;
        smoothLookX = Mathf.Lerp(smoothLookX, targetLookX, Time.deltaTime * smoothing);

        // ������� ������ �����/������
        playerBody.Rotate(Vector3.up * smoothLookX);
    }
}
