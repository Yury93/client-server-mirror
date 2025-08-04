using System;
using TMPro; 
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.UI.RegisterWindow
{
    public class RegisterWindowView : MonoBehaviour, IRegisterWindow
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _continueButton;

        public event Action<string> onInput;
        public event Action onSendInput;

        public void Init()
        {
            _inputField.onValueChanged.AddListener(OnInputChange);
            _continueButton.onClick.AddListener(ClickContinue);
        }
        public void SetNick(string nick)
        {
            _inputField.text = nick;
        }
        public void Close()
        {
            _inputField.onValueChanged.RemoveListener(OnInputChange);
            _continueButton.onClick.RemoveListener(ClickContinue); 
        }
        private void ClickContinue()
        {
            onSendInput?.Invoke();
        } 
        private void OnInputChange(string input)
        {
            onInput?.Invoke(input);
        }
        private void OnDestroy()
        {
            Close();
        }

       
    }
}