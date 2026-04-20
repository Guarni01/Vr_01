using UnityEngine;
using UnityEngine.InputSystem;

public class OpenXRInputBareboneExample : MonoBehaviour
{
    [SerializeField] private bool logValues = true;

    private InputAction thumbstickX;
    private InputAction primaryButton;

    private void Awake()
    {
        thumbstickX = new InputAction("Thumbstick X", InputActionType.Value, "<XRController>{RightHand}/primary2DAxis/x");
        primaryButton = new InputAction("Primary Button", InputActionType.Button, "<XRController>{RightHand}/primaryButton");

        primaryButton.performed += _ =>
        {
            if (logValues)
            {
                Debug.Log("[OpenXRInputBareboneExample] Primary button premuto");
            }
        };

        primaryButton.canceled += _ =>
        {
            if (logValues)
            {
                Debug.Log("[OpenXRInputBareboneExample] Primary button rilasciato");
            }
        };
    }

    private void OnEnable()
    {
        thumbstickX.Enable();
        primaryButton.Enable();
    }

    private void OnDisable()
    {
        thumbstickX.Disable();
        primaryButton.Disable();
    }

    private void OnDestroy()
    {
        thumbstickX.Dispose();
        primaryButton.Dispose();
    }

    private void Update()
    {
        float axisValue = thumbstickX.ReadValue<float>();

        if (logValues && Mathf.Abs(axisValue) > 0.1f)
        {
            Debug.Log($"[OpenXRInputBareboneExample] Thumbstick X: {axisValue:0.00}");
        }
    }
}
