using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public float m_moveSpeed = 10;

    [SerializeField]
    private float m_rotationSpeed = 10;

    [SerializeField]
    private Transform m_characterTF;

    [SerializeField]
    private float m_jumpForce = 300; 

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Animator m_characterAnimator;

    [SerializeField]
    private Transform m_footTF;

    [SerializeField]
    private LayerMask m_GroundMask;

    [SerializeField]
    private float m_fallingTreshold = 0;

    Vector3 _direction;

    public Camera _cam;

    private float _turnSmoothVelocity;

    private float _turnSmoothTime = 0.05f;

    private bool isMoving 
    {
        get { return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;}
    }

    private bool isGrounded
    {
        get { return Physics.Raycast(m_footTF.position, Vector3.down, 0.5f, m_GroundMask); }
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f * 3, 0);
    }

    /*
    private void Update()
    {
        //saut
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(isGrounded);
            Jump();
        }
    }
    */

    // Update is called once per frame
    /*
    void FixedUpdate()
    {
        //animation
        m_characterAnimator.SetBool("isMoving", isMoving);

        //deplacement
        rb.MovePosition(m_characterTF.position + transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * m_moveSpeed);

        //rotation
        rb.MoveRotation(m_characterTF.rotation * Quaternion.Euler(0, Input.GetAxis("Horizontal") * Time.deltaTime * m_rotationSpeed, 0));
    }
    */
    // Start is called before the first frame update
    void FixedUpdate()
    {
        _direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        _direction = _direction.normalized;

        if (_direction.magnitude > 0.1f) MovePlayer();

        m_characterAnimator.SetBool("isMoving", _direction.magnitude > 0.1f);
        // MovePlayerCamera();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        bool _isGrounded = isGrounded;

        m_characterAnimator.SetBool("IsJumping", !_isGrounded);
    }

    void MovePlayer()
    {
        Debug.Log("blerp");
        float _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
        float _anle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, _anle, 0f);
        Vector3 _moveDir = Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;
        _moveDir = _moveDir.normalized;
        rb.MovePosition(transform.position + (_moveDir * m_moveSpeed * Time.deltaTime));
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }
    }
}
