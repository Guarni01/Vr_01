using UnityEngine;
using UnityEngine.InputSystem;

public class TestNewUnityInputSystem : MonoBehaviour

{
    public InputAction TestAction;
    public InputAction TestAxis;
    public bool ActionDebug;
    public float AxisDebug;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TestAction.Enable();
        TestAxis.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        ActionDebug = TestAction.IsPressed();
        AxisDebug = TestAxis.ReadValue<float>();
    }
}
