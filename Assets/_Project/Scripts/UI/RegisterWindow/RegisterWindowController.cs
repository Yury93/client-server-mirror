
using Assets._Project.Scripts.Infrastructure.Services;
using System;

namespace Assets._Project.Scripts.UI.RegisterWindow
{
    public class RegisterWindowController : IDisposable
    {
        private IRegisterWindow _registerWindow;
        private IMessageService _nickService;
        private string _inputedName;
        public event Action onRegistered;
        public RegisterWindowController(IRegisterWindow registerWindow, IMessageService nickService)
        {
            _registerWindow = registerWindow;
            _nickService = nickService;
            Init();
        }
        private void Init()
        {
            _registerWindow.Init();
            _registerWindow.onInput += OnInput;
            _registerWindow.onSendInput += OnClickContinue;
            _registerWindow.SetNick(_nickService.GetNick());
        }
        private void OnClickContinue()
        {
            if (_inputedName == "" || _inputedName == null)
            {
                _inputedName = _nickService.GenerateNick();
            }

            _nickService.Save(_inputedName);
            onRegistered?.Invoke();
            Dispose();
        }
        private void OnInput(string input)
        {
            _inputedName = input;
        }
        public void Dispose()
        {
            _registerWindow.onInput -= OnInput;
            _registerWindow.onSendInput -= OnClickContinue;
        }
    }
}