 
using Assets._Project.Scripts.UI.RegisterWindow;
using Mirror; 
using System.Threading.Tasks;
using UnityEngine; 

namespace Assets._Project.Scripts.Infrastructure.Services.GameFactory
{
    public interface IGameFactory
    {
        Task<GameObject> CreatePlayerNetwork(NetworkConnectionToClient conn, Vector3 worldPosition, Quaternion worldRotation);
        Task<IRegisterWindow> CreateRegisterWindow();
        Task<GameObject> CreateChatNetwork(  Vector3 worldPosition, Quaternion worldRotation);
    }

    public class GameFactory : IGameFactory
    {
        private IAssetProvider _assetProvider;
        public readonly string PLAYER = "PlayerArmature";
        public readonly string REGISTER_WINDOW = "RegisterWindowCanvas";
        public readonly string CHAT_WINDOW = "ChatCanvas";
        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async Task<GameObject> CreatePlayerNetwork(NetworkConnectionToClient conn, Vector3 worldPosition, Quaternion worldRotation)
        {
            GameObject playerInstance = await _assetProvider.InstantiateNetworkObjectAsync(PLAYER, checkForNetworkIdentity: true);

            if (playerInstance == null)
            { 
                Debug.LogError("PlayerInstance == null");
                return null;
            }
 
            bool result = NetworkServer.AddPlayerForConnection(conn, playerInstance);

            if (!result)
            {
                Debug.LogError($"Не получилось добавить игрока для коннекта {conn}   ");
                GameObject.Destroy(playerInstance);
            }
            else
            {
                playerInstance.transform.position = worldPosition;
                playerInstance.transform.rotation = worldRotation;
            }
            return playerInstance;
        }
        
        public async Task<IRegisterWindow> CreateRegisterWindow()
        {
            var task = _assetProvider.InstantiateAsync(REGISTER_WINDOW);
            await task;
            var go = task.Result;
            IRegisterWindow mainWindow = go.GetComponent<IRegisterWindow>();

            return mainWindow;
        }
       
        public async Task<GameObject> CreateChatNetwork(  Vector3 worldPosition, Quaternion worldRotation)
        {
            GameObject chatInstance = await _assetProvider.InstantiateNetworkObjectAsync(CHAT_WINDOW, checkForNetworkIdentity: true);
            NetworkServer.Spawn(chatInstance);
            return chatInstance;
        }
    }
}