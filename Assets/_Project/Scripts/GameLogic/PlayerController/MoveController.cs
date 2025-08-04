using UnityEngine;

namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    public interface ITransformUpdater
    {
        void Update(Transform transform);
    }
    public interface IMoveController : ITransformUpdater
    {
        float Speed { get; }
        bool IsGround { get; }
        bool IsJump { get; }
        float MoveMagnitude { get; }
    }
    public class MoveController : IMoveController
    {
        public float Speed => _moveLogic.CurrentSpeed;
        public bool IsGround => _moveLogic.IsGround;
        public bool IsJump => _inputHandler.JumpInput;

        public float MoveMagnitude => _inputHandler.MoveInput.magnitude;

        private IMover _moveLogic;
        private IRotater _rotateLogic;
        private Transform _cameraFollowTarget;
        private IInputHandler _inputHandler;

        public MoveController(CharacterController characterController, Transform cameraFollowTarget, IInputHandler inputHandler, MoverData moverData, RotationData rotationData)
        {
            _moveLogic = new Mover(characterController, moverData);
            _rotateLogic = new Rotater(rotationData);
            _cameraFollowTarget = cameraFollowTarget;
            _inputHandler = inputHandler;
        }

        public void Update(Transform transform)
        {
            _moveLogic.CheckGrounded(transform);
            _moveLogic.Move(_inputHandler.MoveInput,
                _cameraFollowTarget.transform.forward,
                _inputHandler.SprintInput);
            _moveLogic.HandleJumpAndGravity(_inputHandler.JumpInput);
            _rotateLogic.RotateTarget(_cameraFollowTarget.transform,
                _inputHandler.LookInput.x,
                _inputHandler.LookInput.y);
        }
    }
}