using Assets._Project.Scripts.Infrastructure.Services;
using Assets._Project.Scripts.UI.RegisterWindow;
using Mirror; 
using System;
using TMPro; 
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets._Project.Scripts.UI.Chat
{
    public class ChatWindow : NetworkBehaviour, IWindow, IInputFieldAction
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _sendButton; 

        public event Action<string> onInput;
        public event Action onSendInput;
        private IMessageService _messageService;
     
        
        public void Start ()
        { 
            Init();
        }
        public void Init()
        {
            _messageService = ProjectContext.Instance.Container.Resolve<IMessageService>();
          
            _inputField.onValueChanged.AddListener(OnInput);
            _sendButton.onClick.AddListener(OnSend);
        }

        private void OnSend()
        {
            if (_inputField.text== "" || _inputField == null) return;

            onSendInput?.Invoke();
           
            _messageService.Send($" msg: {_inputField.text}");
            _inputField.text = "";
        }

        
         
        private void OnInput(string input)
        {
            onInput?.Invoke(input);
        }

        public void Close()
        {
            _inputField.onValueChanged.RemoveListener(OnInput);
            _sendButton.onClick.RemoveListener(OnSend);
        }
    }
}