using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using UnityEngine.InputSystem;

public class PhoneTiltInput : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool _isDebugging = false;

    [Header("Input Actions")]
    [SerializeField] private PlayerInput _playerInput;

    [Header("Steering Settings")]
    [Tooltip("How sensitive steering is to phone tilt.")]
    public float steeringSensitivity = 2f;

    [Tooltip("Maximum steering angle in degrees.")]
    public float maxSteerAngle = 30f;

    [Tooltip("How quickly steering adjusts to new input.")]
    public float steeringSmoothness = 5f;

    [Tooltip("Base dead zone to ignore small tilt inputs.")]
    public float baseDeadZone = 0.05f;

    [Tooltip("Extra dead zone scaling based on speed (optional).")]
    public float deadZoneScaling = 0.02f;

    [Header("Steering Curve")]
    [Tooltip("Maps raw tilt input to steering. X = tilt, Y = steering strength.")]
    public AnimationCurve steeringCurve = AnimationCurve.EaseInOut(-1f, -1f, 1f, 1f);

    [Header("Debug UI")]
    [Tooltip("Optional UI Image to show steering input (fill amount).")]
    public Image steeringDebugBar;

    [Tooltip("TMP text to show raw tilt input.")]
    public TMP_Text _tiltText;

    [Tooltip("TMP text to show processed steering angle.")]
    public TMP_Text _steerAngleText;

    [Tooltip("TMP text to show processed steering angle.")]
    public TMP_Text _normalisedAngleText;



    // Exposed value you can read from elsewhere (e.g. car controller)
    public float CurrentSteerAngle { get; private set; }

    private float _targetSteerAngle;
    private float _rawTiltValue;

    public float fn_GetNormalizedSteerAngle () => _targetSteerAngle / maxSteerAngle;

    // Input Action references
    private InputAction _tiltAction;

    void Start()
    {
        // Setup Input Actions
        if (_playerInput != null)
        {
            _tiltAction = _playerInput.actions["Tilt"];
            _tiltAction.Enable();
        }

        // Fallback: Enable the accelerometer directly if no Input Actions
        if (_playerInput == null && Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
            if(_isDebugging) Debug.Log("Using direct accelerometer access");
        }

        if (Accelerometer.current == null)
        {
            if (_isDebugging) Debug.LogWarning("No accelerometer found on this device!");
        }
    }

    void Update()
    {
        Vector3 acceleration = Vector3.zero;

        //// Method 1: Use Input Action if available
        //if (_tiltAction != null && _tiltAction.enabled)
        //{
        //    acceleration = _tiltAction.ReadValue<Vector3>();
        //    _rawTiltValue = acceleration.x;
        //}
        // Method 2: Fallback to direct accelerometer access
        if (Accelerometer.current != null)
        {
            acceleration = Accelerometer.current.acceleration.ReadValue();
            _rawTiltValue = acceleration.x;
        }
        else
        {
            Vector2 input_Move = _playerInput.actions["Move"].ReadValue<Vector2>();

            // Fallback for testing in editor or devices without accelerometer
            _rawTiltValue = input_Move.x * 0.5f;
            if(_isDebugging) Debug.LogWarning("Using fallback input - no accelerometer available");
        }

        // Apply dynamic dead zone
        float dynamicDeadZone = baseDeadZone + Mathf.Abs(_rawTiltValue) * deadZoneScaling;
        float processedTilt = Mathf.Abs(_rawTiltValue) < dynamicDeadZone ? 0f : _rawTiltValue;

        // Apply non-linear curve
        float curvedTilt = steeringCurve.Evaluate(processedTilt);

        // Map tilt to steering target
        _targetSteerAngle = Mathf.Clamp(
            curvedTilt * steeringSensitivity * maxSteerAngle,
            -maxSteerAngle,
            maxSteerAngle
        );

        // Smoothly interpolate towards target steering angle
        CurrentSteerAngle = Mathf.Lerp(CurrentSteerAngle, _targetSteerAngle, Time.deltaTime * steeringSmoothness);

        // Apply rotation
        // transform.localRotation = Quaternion.Euler(0f, CurrentSteerAngle, 0f);

        // Debug visualization
        UpdateDebugUI();
    }
    void OnDestroy()
    {
        // Clean up Input Actions
        if (_tiltAction != null)
        {
            _tiltAction.Disable();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        // Handle app pause/resume for mobile
        if (!pauseStatus)
        {
            if (_tiltAction != null)
                _tiltAction.Enable();
            else if (Accelerometer.current != null)
                InputSystem.EnableDevice(Accelerometer.current);
        }
    }

    private void UpdateDebugUI()
    {
        // Update steering debug bar
        if (steeringDebugBar != null)
        {
            float normalized = Mathf.InverseLerp(-maxSteerAngle, maxSteerAngle, CurrentSteerAngle);
            steeringDebugBar.fillAmount = normalized;
        }

        // Update debug text with actual values being processed
        if (_tiltText != null)
            _tiltText.text = $"Raw Tilt: {_rawTiltValue:F3}";

        if (_steerAngleText != null)
            _steerAngleText.text = $"Steer Angle: {CurrentSteerAngle:F2}°";

        if (_normalisedAngleText != null)
            _normalisedAngleText.text = $"Steer Angle: {fn_GetNormalizedSteerAngle():F2}°";

    }


}


