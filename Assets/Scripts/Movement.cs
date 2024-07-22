using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float fWalkSpeed = 8.0f;
    [SerializeField] private float fSprintSpeed = 14.0f;
    [SerializeField] private float fMaxVelocityChange = 10.0f;
    [SerializeField] private float fAirControl = 0.5f;
    [SerializeField] private float fJumpHeight = 30.0f;
    private Vector2 input;
    private Rigidbody rb;
    private bool bSprinting = false;
    private bool bJumping = false;
    private bool bGrounded = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
        bSprinting = Input.GetButton("Sprint");
        bJumping = Input.GetButton("Jump");
    }

    private void OnTriggerStay(Collider _other)
    {
        bGrounded = true;
    }

    private void FixedUpdate()
    {
        if (!bGrounded)
        {
            Move(fAirControl);
            return;
        }

        if (bJumping)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, fJumpHeight, rb.linearVelocity.z);
        else
            Move(1.0f);

        bGrounded = false;
    }

    private void Move(float _control)
    {
        if (input.magnitude > 0.5f)
        {
            rb.AddForce(CalculateMovement(bSprinting ? fSprintSpeed * _control : fWalkSpeed * _control), ForceMode.VelocityChange);
        }
        else
        {
            Vector3 _velocity = rb.linearVelocity;
            _velocity = new Vector3(_velocity.x * 0.2f * Time.fixedDeltaTime, _velocity.y,
                _velocity.z * 0.2f * Time.fixedDeltaTime);
            rb.linearVelocity = _velocity;
        }
    }
    
    private Vector3 CalculateMovement(float _speed)
    {
        Vector3 _targetVelocity = new Vector3(input.x, 0, input.y);
        _targetVelocity = transform.TransformDirection(_targetVelocity);
        _targetVelocity *= _speed;

        Vector3 _velocity = rb.linearVelocity;

        if (input.magnitude <= 0.5f) 
            return Vector3.zero;
        
        Vector3 _velocityChange = _targetVelocity - _velocity;
        _velocityChange.x = Mathf.Clamp(_velocityChange.x, -fMaxVelocityChange, fMaxVelocityChange);
        _velocityChange.y = 0;
        _velocityChange.z = Mathf.Clamp(_velocityChange.z, -fMaxVelocityChange, fMaxVelocityChange);
        return _velocityChange;

    }
}
