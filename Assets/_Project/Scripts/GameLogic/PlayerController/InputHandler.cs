using UnityEngine;

namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    public interface IInputHandler
    {
        public Vector2 MoveInput { get; }
        public bool JumpInput { get; }
        public bool SprintInput { get; }
        public Vector2 LookInput { get; }
    }
    public class InputHandler : IInputHandler
    {
        public Vector2 MoveInput => GetMoveInput();
        public bool JumpInput => GetJumpInput();
        public bool SprintInput => GetSprintInput(); 
        public Vector2 LookInput => GetLookInput();

        private Vector2 GetMoveInput()
        { 
            var inputMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); 
            inputMove = Vector2.ClampMagnitude(inputMove, 1f);
            return inputMove;
        } 
        private bool GetJumpInput() => Input.GetButtonDown("Jump"); 
        private bool GetSprintInput() => Input.GetKey(KeyCode.LeftShift);
        private Vector2 GetLookInput() => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); 
    }
}