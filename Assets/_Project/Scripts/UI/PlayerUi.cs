using Assets._Project.Scripts.Infrastructure.Services;
using Mirror;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.UI
{
    public class PlayerUi : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nickText; 

        private IMessageService _messageService;
         
        [SyncVar(hook = nameof(OnNickChanged))]
        private string _syncedNick = "";
         
        private void ResolveNickService()
        { 
                _messageService = ProjectContext.Instance.Container.Resolve<IMessageService>(); 
        }

        public override void OnStartLocalPlayer()
        {
            if (isLocalPlayer)
            {
                ResolveNickService();
                 
                if (_messageService != null)
                {
                    string localNick = _messageService.GetNick();
                    CmdSetPlayerNick(localNick);
                }
            }
        }

        [Command]
        private void CmdSetPlayerNick(string nick)
        { 
            _syncedNick = nick;
        }

        private void OnNickChanged(string oldValue, string newValue)
        {
            _nickText.text = _syncedNick;
        }
         
         
        private void LateUpdate()
        {
            if (_nickText == null) return;
            
                if (Camera.main != null)
                {
                    _nickText.transform.LookAt(Camera.main.transform);
                    _nickText.transform.Rotate(0, 180, 0);
                }
            
        }
    }
}