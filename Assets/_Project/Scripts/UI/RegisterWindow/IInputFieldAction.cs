using System;

namespace Assets._Project.Scripts.UI.RegisterWindow
{
    public interface IInputFieldAction
    {
        event Action<string> onInput;
        event Action onSendInput;
    }
}