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
            Debug.Log($"CustomNetworkManager: �������  ����������� ��� ������� ����� �� ����� .");

            try
            {
                base.StartHost();
            }
            catch
            {
                Debug.Log($"CustomNetworkManager: �� ������� ��������� ���� (���� �����):. ������� ����������� ��� ������...");
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
                Debug.Log($"CustomNetworkManager: ����������� ������� StartClient() ");
                base.StartClient();
            }
            catch (Exception ex)
            {
                Debug.LogError($"CustomNetworkManager: ������ ��� ����������� �������: {ex}");

            }
        }


        public override void OnStartHost()
        {
            base.OnStartHost();
            Debug.Log("CustomNetworkManager: ���� ������� ������� OnStartHost()");

        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log("CustomNetworkManager: ������ ����� ����������� OnStartClient()");
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("CustomNetworkManager: ������ ������� ��������� � ������� OnClientConnect()");



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
            Debug.LogWarning("CustomNetworkManager: ������ �� �����.");
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
                Debug.LogWarning("CustomNetworkManager: ������ ��� � �� ���� ready ����� ��������. ������� �������� ������");
                AddPlayerForThisClient();
            }
        }

        private void AddPlayerForThisClient()
        {
            if (NetworkClient.connection != null && NetworkClient.connection.isReady)
            {
                Debug.Log("CustomNetworkManager: ����� NetworkClient.AddPlayer()...");
                bool playerAdded = NetworkClient.AddPlayer();
                if (!playerAdded)
                {
                    Debug.LogWarning("CustomNetworkManager: �� ������� �������� ������: NetworkClient.AddPlayer() ������ false");
                }
                else
                {
                    Debug.Log("CustomNetworkManager: ������ �� ���������� ������ ���������.");
                }
            }
            else
            {
                Debug.LogWarning("CustomNetworkManager: ���������� �������� ������: ����������� ������� �� ������");
                Debug.Log($"CustomNetworkManager: ��������� �����������: {NetworkClient.connection}, isReady: {NetworkClient.connection?.isReady}");
            }
        }

        public async override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.Log($"CustomNetworkManager: ������: ���������� ������ ��� ����������� {conn}");

            try
            {
                Transform spawnPointTransform = await SpawnPlayer(conn);
                await SpawnChat(spawnPointTransform);
            }
            catch (Exception e)
            {
                Debug.LogError($"CustomNetworkManager: ������ �������� ������ ��� ����������� {conn}: {e}");
            }
        }

        private async Task SpawnChat(Transform spawnPointTransform)
        {
            GameObject chatWindow = await _gameFactory.CreateChatNetwork(spawnPointTransform.position, spawnPointTransform.rotation);
        }

        private async Task<Transform> SpawnPlayer(NetworkConnectionToClient conn)
        {
            Transform spawnPointTransform = GetStartPosition();
            if (spawnPointTransform == null) { Debug.LogError($"CustomNetworkManager: ������ - ��� ����� ���� ���������� ������ (NetworkStartPoint)"); }
            GameObject player = await _gameFactory.CreatePlayerNetwork(conn, spawnPointTransform.position, spawnPointTransform.rotation);


            if (player)
            {
                Debug.Log($"CustomNetworkManager: ����� ������� ������ �� ������� ��� ����������� {conn}");
            }
            else
            {
                Debug.LogError($"CustomNetworkManager: GameFactory �������� � ������� �������� ������ ��� ����������� {conn}");
            }

            return spawnPointTransform;
        }
    }
}