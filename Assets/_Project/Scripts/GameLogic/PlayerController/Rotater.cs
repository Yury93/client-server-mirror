using UnityEngine;

namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    public interface IRotater
    { 
        void RotateTarget(Transform targetRotation, float inputHor, float inputVert);
    }

    public class Rotater : IRotater
    {
        private RotationData _rotationData;
         
        private float _rotationY;  
        private float _rotationX; 

        public Rotater(RotationData rotationData)
        {
            _rotationData = rotationData; 
        }

        public void RotateTarget(Transform targetRotation, float inputX, float inputY)
        {
            if (targetRotation == null) return;
             
            _rotationY += inputX * _rotationData.SensivityHorizontal;
            _rotationX -= inputY * _rotationData.SensivityVertical;  
             
            _rotationX = Mathf.Clamp(_rotationX, _rotationData.BottomClamp, _rotationData.TopClamp);
             
            targetRotation.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
        }
    }
}