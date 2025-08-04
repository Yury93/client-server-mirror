using Assets._Project.Scripts.Infrastructure.Services.GameFactory;
using Mirror;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;


namespace _Project.Scripts.Infrastructure.Network
{
    public class CustomNetworkManager : NetworkManager, INetworkService
    {
        private IGameFactory _gameFactory;

        [Inject]
        private void Construct(IGameFactory gameFactory)
        {
            this._gameFactory = gameFactory;
        }

        public void ConnectOrHost()
        {
            Debug.Log($"CustomNetworkManager: Попытка  подключения или запуска хоста на порту .");

            try
            {
                base.StartHost();
            }
            catch
            {
                Debug.Log($"CustomNetworkManager: Не удалось запустить хост (порт занят):. Попытка подключения как клиент...");
                StartCoroutine(ConnectAsClientDelayed());
            }
        }
        private IEnumerator ConnectAsClientDelayed()
        {
            yield return new WaitForSeconds(0.1f);
            StartClientConnect();
        }

        private void StartClientConnect()
        {
            try
            {
                Debug.Log($"CustomNetworkManager: Подключение клиента StartClient() ");
                base.StartClient();
            }
            catch (Exception ex)
            {
                Debug.LogError($"CustomNetworkManager: Ошибка при подключении клиента: {ex}");

            }
        }


        public override void OnStartHost()
        {
            base.OnStartHost();
            Debug.Log("CustomNetworkManager: Хост успешно запущен OnStartHost()");

        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log("CustomNetworkManager: Клиент начал подключение OnStartClient()");
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("CustomNetworkManager: Клиент успешно подключен к серверу OnClientConnect()");



            if (NetworkClient.ready)
            {
                AddPlayerForThisClient();
            }
            else
            {
                StartCoroutine(DelayedAddPlayer());
            }
        }



        public override void OnClientNotReady()
        {
            base.OnClientNotReady();
            Debug.LogWarning("CustomNetworkManager: Клиент не готов.");
        }

        private IEnumerator DelayedAddPlayer()
        {
            yield return new WaitForSeconds(0.5f);

            if (NetworkClient.ready)
            {
                AddPlayerForThisClient();
            }
            else
            {
                Debug.LogWarning("CustomNetworkManager: Клиент так и не стал ready после ожидания. Попытка добавить игрока");
                AddPlayerForThisClient();
            }
        }

        private void AddPlayerForThisClient()
        {
            if (NetworkClient.connection != null && NetworkClient.connection.isReady)
            {
                Debug.Log("CustomNetworkManager: Вызов NetworkClient.AddPlayer()...");
                bool playerAdded = NetworkClient.AddPlayer();
                if (!playerAdded)
                {
                    Debug.LogWarning("CustomNetworkManager: Не удалось добавить игрока: NetworkClient.AddPlayer() вернул false");
                }
                else
                {
                    Debug.Log("CustomNetworkManager: Запрос на добавление игрока отправлен.");
                }
            }
            else
            {
                Debug.LogWarning("CustomNetworkManager: Невозможно добавить игрока: подключение клиента не готово");
                Debug.Log($"CustomNetworkManager: Состояние подключения: {NetworkClient.connection}, isReady: {NetworkClient.connection?.isReady}");
            }
        }

        public async override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.Log($"CustomNetworkManager: Сервер: добавление игрока для подключения {conn}");

            try
            {
                Transform spawnPointTransform = await SpawnPlayer(conn);
                await SpawnChat(spawnPointTransform);
            }
            catch (Exception e)
            {
                Debug.LogError($"CustomNetworkManager: Ошибка создания игрока для подключения {conn}: {e}");
            }
        }

        private async Task SpawnChat(Transform spawnPointTransform)
        {
            GameObject chatWindow = await _gameFactory.CreateChatNetwork(spawnPointTransform.position, spawnPointTransform.rotation);
        }

        private async Task<Transform> SpawnPlayer(NetworkConnectionToClient conn)
        {
            Transform spawnPointTransform = GetStartPosition();
            if (spawnPointTransform == null) { Debug.LogError($"CustomNetworkManager: Ошибка - нет точки куда приземлить игрока (NetworkStartPoint)"); }
            GameObject player = await _gameFactory.CreatePlayerNetwork(conn, spawnPointTransform.position, spawnPointTransform.rotation);


            if (player)
            {
                Debug.Log($"CustomNetworkManager: Игрок успешно создан на сервере для подключения {conn}");
            }
            else
            {
                Debug.LogError($"CustomNetworkManager: GameFactory сообщила о неудаче создания игрока для подключения {conn}");
            }

            return spawnPointTransform;
        }
    }
}