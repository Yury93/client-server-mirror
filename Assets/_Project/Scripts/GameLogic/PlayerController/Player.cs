
using Assets._Project.Scripts.Infrastructure.Services;
using Mirror;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    [RequireComponent(typeof(CharacterController),typeof(AnimatorController))]
    public class Player : NetworkBehaviour
    {
        [SerializeField] private MoverData _moverData;
        [SerializeField] private RotationData _rotationData;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private CameraFollowTarget _cameraFollowTarget;
        [SerializeField] private AnimatorController _animatorController;
       
        private IInputHandler _inputHandler = null;
        private IMoveController _moveController = null;
       
        
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            if (isLocalPlayer)
            { 
                _inputHandler = new InputHandler();
                _moveController = new MoveController(
                    _characterController,
                    _cameraFollowTarget.transform,
                    _inputHandler, _moverData,
                    _rotationData);

                _animatorController = GetComponent<AnimatorController>();
                _animatorController.InitLocalPlayer(_moveController);
            }
        }
        private void Update()
        {
            if(isLocalPlayer )
            _moveController.Update(transform);
        }
    }
}