namespace Assets._Project.Scripts.UI.RegisterWindow
{
    public interface  IRegisterWindow : IWindow, IInputFieldAction
    { 
        void SetNick(string nick);
    }
}