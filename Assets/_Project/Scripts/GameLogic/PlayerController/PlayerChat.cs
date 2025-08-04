
using Assets._Project.Scripts.Infrastructure.Services;
using Mirror;
using UnityEngine;
using Zenject;
namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    public class PlayerChat : NetworkBehaviour
    {
        [SyncVar] public string playerName;
        private IMessageService _messageService;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            if (isLocalPlayer)
            {
                _messageService = ProjectContext.Instance.Container.Resolve<IMessageService>();
                playerName = _messageService.GetNick();
                _messageService.onSendMessage += OnSendMessage;
            }
        }

        private void OnSendMessage(string message)
        {
            if (isLocalPlayer)
                CmdSendMessage($"player: {playerName} \n {message}");
        }
        [Command]
        private void CmdSendMessage(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            RpcReceiveMessage(text);
        }

        [ClientRpc]
        private void RpcReceiveMessage(string message)
        {
            Debug.Log($"{message}");
        }
        public override void OnStopLocalPlayer()
        {
            if (isLocalPlayer)
            {
                _messageService.onSendMessage -= OnSendMessage;
            }
        }
    }
}
