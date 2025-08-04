 using UnityEngine; 

namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    public interface IMover
    {
        public float CurrentSpeed { get;  }
        public bool IsGround { get; } 
        public void Move(Vector2 moveInput,Vector3 direction, bool sprintInput); 
        public void HandleJumpAndGravity(bool _jumpInput); 
        public void CheckGrounded(Transform transform);
    }
    public class Mover : IMover
    { 
        private MoverData _moverData;
        private float _speed;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _jumpTimeoutDelta; 
        private CharacterController _controller;
        private bool _grounded = true;

        public float CurrentSpeed => _speed; 
        public bool IsGround => _grounded;  

        public Mover(CharacterController characterController, MoverData moverData)
        { 
            _jumpTimeoutDelta = _moverData.JumpTimeout;
            _moverData = moverData;
            _controller = characterController;
        } 
        public void Move(Vector2 moveInput, Vector3 direction, bool sprintInput)
        {
            _speed = sprintInput ? _moverData.SprintSpeed : _moverData.MoveSpeed;
            if (moveInput == Vector2.zero) _speed = 0.0f;
             
            Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;
             
            Vector3 cameraForward = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
             
            if (cameraForward != Vector3.zero)
            {
                cameraRight = Vector3.Cross(Vector3.up, cameraForward).normalized;
            }
            else
            { 
                cameraForward = Vector3.forward;
                cameraRight = Vector3.right;
            }
             
            Vector3 moveDirection = inputDirection.x * cameraRight + inputDirection.z * cameraForward;
            moveDirection = moveDirection.normalized; 
             
            Vector3 moveVector = moveDirection * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
            _controller.Move(moveVector);
            RotateFromDirection(moveDirection);
        }
        private void RotateFromDirection(Vector3 direction)
        {
            if (_speed < 0.01) return;
            Vector3 targetDirection = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
             
            
            float targetRotation = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
             
            float currentRotation = _controller.transform.eulerAngles.y; 
            float smoothedRotation = Mathf.LerpAngle(currentRotation, targetRotation,
                                                     _moverData.RotationLerpSpeed * Time.deltaTime);
        
            _controller.transform.rotation = Quaternion.Euler(0.0f, smoothedRotation, 0.0f); 
        }
        public void HandleJumpAndGravity(bool _jumpInput)
        {
            if (_grounded)
            {
                if (_verticalVelocity < 0.0f) _verticalVelocity = -2f;
                if (_jumpInput && _jumpTimeoutDelta <= 0.0f)
                { 
                    _verticalVelocity = Mathf.Sqrt(_moverData.JumpHeight * -2f * _moverData.Gravity);
                }
                if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = _moverData.JumpTimeout;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += _moverData.Gravity * Time.deltaTime;
            }
        }

        public void CheckGrounded(Transform transform)
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _moverData.GroundedOffset, transform.position.z);
            _grounded = Physics.CheckSphere(spherePosition, _moverData.GroundedRadius, _moverData.GroundLayers, QueryTriggerInteraction.Ignore);
        } 
    }
}