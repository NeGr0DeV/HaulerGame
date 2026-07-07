using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class CarController : MonoBehaviour
{
    [SerializeField] private Transform _centreOfMass;
    [SerializeField] private Wheel[] _wheels;
    [SerializeField] private GameObject _lights;
    [SerializeField] private GameObject _lightsBackMove;


    private Rigidbody _rb;
    [SerializeField] private int _motorForce;
    [SerializeField] private int _brakeForce;
    [SerializeField] private float _engineBrakeForce;
    [SerializeField] private float _maxSpeedForvard;
    [SerializeField] private float _maxSpeedRevers;

    [SerializeField] private float _brakeInput;
    private float _verticalInput;
    private float _horizontalInput;

    [SerializeField] public float _speed;

    [SerializeField] private AnimationCurve _sterlingCurve;

    private float smallAngleTimer = 0f;
    private const float REQUIRED_SMALL_ANGLE_TIME = 2f;

    private JointSpring[] initialSprings;
    private WheelFrictionCurve[] initialForwardFrictions;
    private WheelFrictionCurve[] initialSidewaysFrictions;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centreOfMass != null ? _centreOfMass.localPosition : new Vector3(0, -0.5f, 0);
        _rb.mass = 1500f;
        _rb.linearDamping = 0.5f;
        _rb.angularDamping = 0.5f;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        foreach (Wheel wheel in _wheels)
        {
            var c = wheel.WheelCollider;

            // Подвеска
            var spring = c.suspensionSpring;
            spring.spring = 35000f;
            spring.damper = 1500f;
            spring.targetPosition = 0.5f;
            c.suspensionSpring = spring;

            // Радиус и масса
            // c.radius = 0.5f;
            // c.mass = 40f;

            // Настройка трения (рекомендую эти значения как стартовые)
            WheelFrictionCurve fwd = c.forwardFriction;
            fwd.extremumSlip = 0.4f;
            fwd.extremumValue = 1f;
            fwd.asymptoteSlip = 0.8f;
            fwd.asymptoteValue = 0.5f;
            c.forwardFriction = fwd;

            WheelFrictionCurve side = c.sidewaysFriction;
            side.extremumSlip = 0.2f;
            side.extremumValue = 1f;
            side.asymptoteSlip = 0.5f;
            side.asymptoteValue = 0.75f;
            c.sidewaysFriction = side;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckInput();
        Move();
        Brake();
        Steerling();
        
    }

    private void FixedUpdate()
    {
        foreach (Wheel wheel in _wheels)
        {
            if (wheel.IsForwardWheels)
            {
                float slipAngel = Vector3.Angle(transform.forward, _rb.linearVelocity - transform.forward);
            }
        }

    }


    private void Move()
    {
        _speed = _rb.linearVelocity.magnitude;

        foreach (Wheel wheel in _wheels)
        {
            float motorTorque = 0f;
            float currentMaxSpeed = _verticalInput > 0 ? _maxSpeedForvard : _maxSpeedRevers;

            if (Mathf.Abs(_verticalInput) > 0.01f && _speed < currentMaxSpeed)
            {
                float speedLimit = Mathf.Clamp01(1f - (_speed / currentMaxSpeed));
                motorTorque = _motorForce * _verticalInput * speedLimit;
            }
            else if (_speed > 0.5f && Mathf.Abs(_verticalInput) < 0.01f)
            {
                // Двигательное торможение (engine brake)
                motorTorque = -Mathf.Sign(Vector3.Dot(_rb.linearVelocity, transform.forward)) * _engineBrakeForce;
                if (Vector3.Dot(transform.forward * motorTorque, _rb.linearVelocity.normalized) > 0)
                    motorTorque = 0;
            }

            wheel.WheelCollider.motorTorque = motorTorque;
            wheel.UpdateMeshPosition();
        }

        // Логика фар
        if (_verticalInput < 0)
            _lightsBackMove.SetActive(true);
        else
            _lightsBackMove.SetActive(false);

        if (_verticalInput > 0)
            _lights.SetActive(true);
        else
            _lights.SetActive(false);
    }

    public bool IsMovingForward
    {
        get
        {
            return Vector3.Dot(transform.forward, _rb.linearVelocity) > 0.5f;
        }
    }

    private void CheckInput()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");

        float movingDirectional = Vector3.Dot(transform.forward, _rb.linearVelocity);

        if ((movingDirectional < -0.5f && _verticalInput > 0) || (movingDirectional > 0.5f && _verticalInput < 0))
            _brakeInput = Mathf.Abs(_verticalInput);
        else
            _brakeInput = 0;

    }

    private void Brake()
    {
        foreach (Wheel wheel in _wheels)
        {
            if (wheel.IsForwardWheels)
            {
                wheel.WheelCollider.brakeTorque = _brakeForce * _brakeInput * 0.7f;
            }
            else
            {
                wheel.WheelCollider.brakeTorque = _brakeForce * _brakeInput * 0.3f;
            }
        }

    }

    private void Steerling()
    {
        float steeringAngle = _horizontalInput * _sterlingCurve.Evaluate(_speed);
        float slipAngel = Vector3.Angle(transform.forward, _rb.linearVelocity - transform.forward);

        if (slipAngel < 120 && slipAngel > 10)
            steeringAngle += Vector3.SignedAngle(transform.forward, _rb.linearVelocity, Vector3.up);

        steeringAngle = Mathf.Clamp(steeringAngle, -48, 48);

        foreach (Wheel wheel in _wheels)
        {
            if (wheel.IsForwardWheels)
            {
                wheel.WheelCollider.steerAngle = steeringAngle;
            }
        }
    }

}

[System.Serializable]
public struct Wheel
{
    public Transform WheelMesh;
    public WheelCollider WheelCollider;
    public bool IsForwardWheels;

    public void UpdateMeshPosition()
    {
        Vector3 position;
        Quaternion rotation;

        WheelCollider.GetWorldPose(out position, out rotation);

        WheelMesh.position = position;
        WheelMesh.rotation = rotation;
    }
}