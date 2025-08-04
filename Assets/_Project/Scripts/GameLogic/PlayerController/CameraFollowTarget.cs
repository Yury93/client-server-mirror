
using Mirror;
using UnityEngine;

namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    public class CameraFollowTarget : NetworkBehaviour
    {
        [SerializeField] private Cinemachine.CinemachineVirtualCamera _camera;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            if (isLocalPlayer)
            {
                _camera = FindAnyObjectByType<Cinemachine.CinemachineVirtualCamera>();
                _camera.Follow = this.transform;
            }
        }
    }
}