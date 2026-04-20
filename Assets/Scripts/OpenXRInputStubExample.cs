using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenXRInputStubExample : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool logButtonEvents = true;
    [SerializeField] private bool logAnalogValues;
    [SerializeField, Range(0.01f, 0.25f)] private float analogLogThreshold = 0.1f;

    [Header("Optional Pose Targets")]
    [SerializeField] private Transform leftControllerTarget;
    [SerializeField] private Transform rightControllerTarget;

    private readonly List<InputAction> actions = new();

    private InputAction leftTrigger;
    private InputAction rightTrigger;
    private InputAction leftGrip;
    private InputAction rightGrip;
    private InputAction leftThumbstick;
    private InputAction rightThumbstick;
    private InputAction leftThumbstickClick;
    private InputAction rightThumbstickClick;
    private InputAction leftPrimaryButton;
    private InputAction leftSecondaryButton;
    private InputAction rightPrimaryButton;
    private InputAction rightSecondaryButton;
    private InputAction leftIsTracked;
    private InputAction rightIsTracked;
    private InputAction leftDevicePosition;
    private InputAction rightDevicePosition;
    private InputAction leftDeviceRotation;
    private InputAction rightDeviceRotation;

    private float lastLeftTrigger = -1f;
    private float lastRightTrigger = -1f;
    private float lastLeftGrip = -1f;
    private float lastRightGrip = -1f;
    private Vector2 lastLeftThumbstick = Vector2.one * float.MinValue;
    private Vector2 lastRightThumbstick = Vector2.one * float.MinValue;

    private void Awake()
    {
        CreateActions();
    }

    private void OnEnable()
    {
        foreach (var action in actions)
        {
            action.Enable();
        }
    }

    private void OnDisable()
    {
        foreach (var action in actions)
        {
            action.Disable();
        }
    }

    private void OnDestroy()
    {
        foreach (var action in actions)
        {
            action.Dispose();
        }

        actions.Clear();
    }

    private void Update()
    {
        ExampleReadAnalogInputs();
        ExampleReadThumbsticks();
        ExampleApplyControllerPose(leftIsTracked, leftDevicePosition, leftDeviceRotation, leftControllerTarget);
        ExampleApplyControllerPose(rightIsTracked, rightDevicePosition, rightDeviceRotation, rightControllerTarget);
    }

    [ContextMenu("Log Connected XR Devices")]
    private void LogConnectedXRDevices()
    {
        foreach (var device in InputSystem.devices)
        {
            if (!device.layout.Contains("XR"))
            {
                continue;
            }

            Debug.Log($"[OpenXRInputStubExample] Device: {device.displayName} | Layout: {device.layout}");
        }
    }

    private void CreateActions()
    {
        if (actions.Count > 0)
        {
            return;
        }

        leftTrigger = CreateValueAction("Left Trigger", "<XRController>{LeftHand}/trigger");
        rightTrigger = CreateValueAction("Right Trigger", "<XRController>{RightHand}/trigger");
        leftGrip = CreateValueAction("Left Grip", "<XRController>{LeftHand}/grip");
        rightGrip = CreateValueAction("Right Grip", "<XRController>{RightHand}/grip");

        leftThumbstick = CreateValueAction("Left Thumbstick", "<XRController>{LeftHand}/primary2DAxis");
        rightThumbstick = CreateValueAction("Right Thumbstick", "<XRController>{RightHand}/primary2DAxis");

        leftThumbstickClick = CreateButtonAction("Left Thumbstick Click", "<XRController>{LeftHand}/primary2DAxisClick");
        rightThumbstickClick = CreateButtonAction("Right Thumbstick Click", "<XRController>{RightHand}/primary2DAxisClick");

        leftPrimaryButton = CreateButtonAction("Left Primary Button (X)", "<XRController>{LeftHand}/primaryButton");
        leftSecondaryButton = CreateButtonAction("Left Secondary Button (Y)", "<XRController>{LeftHand}/secondaryButton");
        rightPrimaryButton = CreateButtonAction("Right Primary Button (A)", "<XRController>{RightHand}/primaryButton");
        rightSecondaryButton = CreateButtonAction("Right Secondary Button (B)", "<XRController>{RightHand}/secondaryButton");

        leftIsTracked = CreateButtonAction("Left Is Tracked", "<XRController>{LeftHand}/isTracked");
        rightIsTracked = CreateButtonAction("Right Is Tracked", "<XRController>{RightHand}/isTracked");
        leftDevicePosition = CreatePassThroughAction("Left Device Position", "<XRController>{LeftHand}/devicePosition");
        rightDevicePosition = CreatePassThroughAction("Right Device Position", "<XRController>{RightHand}/devicePosition");
        leftDeviceRotation = CreatePassThroughAction("Left Device Rotation", "<XRController>{LeftHand}/deviceRotation");
        rightDeviceRotation = CreatePassThroughAction("Right Device Rotation", "<XRController>{RightHand}/deviceRotation");

        RegisterButtonExample(leftThumbstickClick);
        RegisterButtonExample(rightThumbstickClick);
        RegisterButtonExample(leftPrimaryButton);
        RegisterButtonExample(leftSecondaryButton);
        RegisterButtonExample(rightPrimaryButton);
        RegisterButtonExample(rightSecondaryButton);
    }

    private InputAction CreateValueAction(string actionName, string binding)
    {
        var action = new InputAction(actionName, InputActionType.Value, binding);
        actions.Add(action);
        return action;
    }

    private InputAction CreateButtonAction(string actionName, string binding)
    {
        var action = new InputAction(actionName, InputActionType.Button, binding);
        actions.Add(action);
        return action;
    }

    private InputAction CreatePassThroughAction(string actionName, string binding)
    {
        var action = new InputAction(actionName, InputActionType.PassThrough, binding);
        actions.Add(action);
        return action;
    }

    private void RegisterButtonExample(InputAction action)
    {
        action.performed += _ =>
        {
            if (logButtonEvents)
            {
                Debug.Log($"[OpenXRInputStubExample] {action.name} pressed");
            }
        };

        action.canceled += _ =>
        {
            if (logButtonEvents)
            {
                Debug.Log($"[OpenXRInputStubExample] {action.name} released");
            }
        };
    }

    private void ExampleReadAnalogInputs()
    {
        // Esempio 1: trigger analogici (indice).
        LogFloatIfChanged(leftTrigger, ref lastLeftTrigger);
        LogFloatIfChanged(rightTrigger, ref lastRightTrigger);

        // Esempio 2: grip analogici (presa della mano).
        LogFloatIfChanged(leftGrip, ref lastLeftGrip);
        LogFloatIfChanged(rightGrip, ref lastRightGrip);
    }

    private void ExampleReadThumbsticks()
    {
        // Esempio 3: thumbstick sinistro e destro come Vector2.
        LogVector2IfChanged(leftThumbstick, ref lastLeftThumbstick);
        LogVector2IfChanged(rightThumbstick, ref lastRightThumbstick);
    }

    private void ExampleApplyControllerPose(
        InputAction trackedAction,
        InputAction positionAction,
        InputAction rotationAction,
        Transform target)
    {
        if (target == null || !trackedAction.IsPressed())
        {
            return;
        }

        // Esempio 4: posizione/rotazione del controller tramite OpenXR.
        target.localPosition = positionAction.ReadValue<Vector3>();
        target.localRotation = rotationAction.ReadValue<Quaternion>();
    }

    private void LogFloatIfChanged(InputAction action, ref float lastValue)
    {
        float currentValue = action.ReadValue<float>();

        if (!logAnalogValues || Mathf.Abs(currentValue - lastValue) < analogLogThreshold)
        {
            return;
        }

        lastValue = currentValue;
        Debug.Log($"[OpenXRInputStubExample] {action.name}: {currentValue:0.00}");
    }

    private void LogVector2IfChanged(InputAction action, ref Vector2 lastValue)
    {
        Vector2 currentValue = action.ReadValue<Vector2>();

        if (!logAnalogValues || Vector2.Distance(currentValue, lastValue) < analogLogThreshold)
        {
            return;
        }

        lastValue = currentValue;
        Debug.Log($"[OpenXRInputStubExample] {action.name}: {currentValue}");
    }
}
