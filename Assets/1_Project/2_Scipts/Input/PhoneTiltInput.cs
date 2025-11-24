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

    [Tooltip("Responsiveness - How quickly steering adjusts to new input.")]
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

    public float fn_GetNormalizedSteerAngle() => _targetSteerAngle / maxSteerAngle;

    // Input Action references
    private InputAction _tiltAction;


#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void EnableSensors()
    {
        InputSystem.AddDevice<Accelerometer>();
        InputSystem.EnableDevice(Accelerometer.current);
    }
#endif

    void Start()
    {
        if (_isDebugging) Debug.Log("PhoneTiltInput: Start");

        // Check if accelerometer exists FIRST
        if (Accelerometer.current == null)
        {
            Debug.LogError("No accelerometer found on this device!");
        }
        else
        {
            // Only enable if it exists
            InputSystem.EnableDevice(Accelerometer.current);
            if (_isDebugging) Debug.Log("Accelerometer enabled successfully!");
        }

        // Setup Input Actions
        if (_playerInput != null)
        {
            _tiltAction = _playerInput.actions["Tilt"];
            _tiltAction.Enable();
            if (_isDebugging) Debug.Log("Tilt action enabled");
        }
    }

    void Update()
    {


        // Try accelerometer first (mobile devices)
        if (Accelerometer.current != null)
        {
            _rawTiltValue = Accelerometer.current.acceleration.ReadValue().x;
        }
        // Fallback to Input Action if available
        else if (_tiltAction != null && _tiltAction.enabled)
        {
            var acceleration = _tiltAction.ReadValue<Vector3>();
            _rawTiltValue = acceleration.x;
        }
        // Final fallback for testing
        else if (_playerInput != null)
        {
            Vector2 input_Move = _playerInput.actions["Move"].ReadValue<Vector2>();
            _rawTiltValue = input_Move.x * 0.5f;
            if (_isDebugging) Debug.LogWarning("Using keyboard fallback - no accelerometer available");
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

        // Update sliders with current values
        UpdateSliderOutputs();

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



    #region Sider Visuals Output

    [Header("Slider Output Controls")]
    [Tooltip("Slider for positive raw tilt values (0 to 1).")]
    public UI_SliderOutputControl _rawTiltPositiveSlider;

    [Tooltip("Slider for negative raw tilt values (0 to -1).")]
    public UI_SliderOutputControl _rawTiltNegativeSlider;

    [Tooltip("Slider for positive target steer angle values.")]
    public UI_SliderOutputControl _steerAnglePositiveSlider;

    [Tooltip("Slider for negative target steer angle values.")]
    public UI_SliderOutputControl _steerAngleNegativeSlider;

    [Tooltip("Use lerp mode for slider updates (smoother but slightly delayed).")]
    public bool _useLerpForSliders = false;


    private void UpdateSliderOutputs()
    {

        #region Raw Tilt
        // Update Raw Tilt Sliders
        // Raw tilt typically ranges from -1 to 1, so we normalize to 0-1 for display
        if (_isDebugging) Debug.Log($"PhoneTiltInput: _raw Tilt Value Steer Angle = {_rawTiltValue}");

        if (_rawTiltPositiveSlider != null && _rawTiltNegativeSlider != null)
        {


            if (_rawTiltValue >= 0)
            {
                // Positive tilt - update positive slider, zero out 
                _rawTiltPositiveSlider.fn_SetFillPct_NoLerp(_rawTiltValue);
                _rawTiltNegativeSlider.fn_SetFillPct_NoLerp(0f);
            }
            else
            {
                // Negative tilt - update negative slider, zero out positive
                _rawTiltNegativeSlider.fn_SetFillPct_NoLerp(Mathf.Abs(_rawTiltValue));
                _rawTiltPositiveSlider.fn_SetFillPct_NoLerp(0f);
            }
        }
        #endregion

        #region Normalized Steer Angle
        // Update Raw Tilt Sliders
        // Raw tilt typically ranges from -1 to 1, so we normalize to 0-1 for display
        var nsv = fn_GetNormalizedSteerAngle();

        if(_isDebugging) Debug.Log($"PhoneTiltInput: Normalized Steer Angle = {nsv}");

        if(_steerAnglePositiveSlider != null && _steerAngleNegativeSlider != null)
        {
            if (nsv >= 0)
            {
                // Positive tilt - update positive slider, zero out negative
                _steerAnglePositiveSlider.fn_SetFillPct_NoLerp(nsv);
                _steerAngleNegativeSlider.fn_SetFillPct_NoLerp(0f);
            }
            else
            {
                // Negative tilt - update negative slider, zero out positive
                _steerAngleNegativeSlider.fn_SetFillPct_NoLerp(Mathf.Abs(nsv));
                _steerAnglePositiveSlider.fn_SetFillPct_NoLerp(0f);
            }
        }


        #endregion


        //fn_GetNormalizedSteerAngle()
    }

    #endregion
}