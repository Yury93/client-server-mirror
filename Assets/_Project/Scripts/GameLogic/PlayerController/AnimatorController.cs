using Mirror;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorController : NetworkBehaviour
    {

        [SerializeField] private Animator _animator;
        [SerializeField] private float _speedChangeRate = 10;
        private readonly int SPEED = Animator.StringToHash("Speed");
        private readonly int MOTION_SPEED = Animator.StringToHash("MotionSpeed");
        private readonly int GROUNDED = Animator.StringToHash("Grounded");
        private readonly int JUMP = Animator.StringToHash("Jump");
        private float _animationBlend;
        private IMoveController _moveController;

        private bool _lastJumpState;
        private bool _lastGroundState; 

        [SyncVar(hook = nameof(OnJumpChanged))]
        private bool _syncJump;
        [SyncVar(hook = nameof(OnGroundChanged))]
        private bool _syncGround;
        public void InitLocalPlayer(IMoveController moveController)
        {
            _animator = GetComponent<Animator>();
            _moveController = moveController;
        }
        private void OnJumpChanged(bool oldValue, bool newValue)
        {
            if (_animator != null)
                _animator.SetBool(JUMP, newValue);
        }

        private void OnGroundChanged(bool oldValue, bool newValue)
        {
            if (_animator != null)
                _animator.SetBool(GROUNDED, newValue);
        }

        private void Update()
        {
            if (!isLocalPlayer || _moveController == null) return;

            bool currentJump = _moveController.IsJump;
            bool currentGround = _moveController.IsGround;
             
            if (currentJump != _lastJumpState || currentGround != _lastGroundState)  
            {
                CmdUpdateJumpState(currentJump, currentGround);
                _lastJumpState = currentJump;
                _lastGroundState = currentGround;
              
            }
        }
        private void LateUpdate()
        {

            if (_animator == null || _moveController == null) { return; }
            _animationBlend = Mathf.Lerp(_animationBlend, _moveController.Speed, Time.deltaTime * _speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            _animator.SetFloat(SPEED, _animationBlend);
            _animator.SetFloat(MOTION_SPEED, _moveController.MoveMagnitude);

            if (isLocalPlayer)
            {
                _animator.SetBool(JUMP, _moveController.IsJump);
                _animator.SetBool(GROUNDED, _moveController.IsGround);
            }
            else
            {
                _animator.SetBool(JUMP, _syncJump);
                _animator.SetBool(GROUNDED, _syncGround);
            }

        }
        [Command]
        private void CmdUpdateJumpState(bool jump, bool ground)
        {
            _syncJump = jump;
            _syncGround = ground;
        }
    }
}