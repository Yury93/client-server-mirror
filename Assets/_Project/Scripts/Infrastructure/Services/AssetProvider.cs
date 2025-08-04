using Mirror;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets._Project.Scripts.Infrastructure.Services
{
    public interface IAssetProvider
    {
        Task<GameObject> InstantiateAsync(string name);
        Task<GameObject> InstantiateAsync(string name, Vector3 position);
        Task<GameObject> InstantiateAsync(string name, Transform parent);
        Task<Material> InstantiateMaterialAsync(string name);
        Task<GameObject> InstantiateNetworkObjectAsync(string name, bool checkForNetworkIdentity = true);
        Task<T> LoadAsync<T>(string name) where T : class; 
    }
    public class AssetProvider : IAssetProvider
    {

        public Dictionary<string, AsyncOperationHandle> _handleResources = new Dictionary<string, AsyncOperationHandle>();
        public Dictionary<string, AsyncOperationHandle> _cachedResources = new Dictionary<string, AsyncOperationHandle>();
        public AssetProvider()
        {
            Addressables.InitializeAsync();
        }

        public async Task<GameObject> InstantiateAsync(string name)
        {
            var task = LoadAsync<GameObject>(name);
            await task;
            var result = task.Result;
            var go = GameObject.Instantiate(result);
            return go;
        }
        public async Task<GameObject> InstantiateAsync(string name, Vector3 position)
        {
            var task = LoadAsync<GameObject>(name);
            await task;
            var result = task.Result;
            var go = GameObject.Instantiate(result);
            go.transform.position = position;
            return go;
        }
        public async Task<GameObject> InstantiateAsync(string name, Transform parent)
        {
            var task = LoadAsync<GameObject>(name);
            await task;
            var result = task.Result;
            var go = GameObject.Instantiate(result);
            go.transform.SetParent(parent);
            return go;
        }
        public async Task<Material> InstantiateMaterialAsync(string name)
        {
            var task = LoadAsync<Material>(name);
            await task;
            Material newMaterial = UnityEngine.Object.Instantiate(task.Result);
            return newMaterial;
        }

        public async Task<T> LoadAsync<T>(string name) where T : class
        {
            if (_cachedResources.ContainsKey(name))
            {
                return _cachedResources[name].Result as T;
            }
            if (_handleResources.ContainsKey(name))
            {
                Debug.Log($"{name} в процессе загрузки");
                return _handleResources[name].Result as T;
            }
            var locations = await Addressables.LoadResourceLocationsAsync(name).Task;
            if (locations == null || locations.Count == 0)
            {
                Debug.LogError($"{name} не найден в группах");
                return null;
            }

          

            var handle = Addressables.LoadAssetAsync<T>(name);
            _handleResources[name] = handle;
            handle.Completed += (asset) =>
            {
                _cachedResources[name] = asset;
            };
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
              
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Ошибка загрузки {name}");
                return null;
            }
        }
        public async Task<GameObject> InstantiateNetworkObjectAsync(string name, bool checkForNetworkIdentity = true)
        { 
            GameObject prefab = await LoadAsync<GameObject>(name);

            if (prefab == null)
            {
                Debug.LogError($"AssetProvider: Префаб '{name}' не удалось загрузить из Addressables.");
                return null;
            }
             
            if (checkForNetworkIdentity)
            {
                NetworkIdentity networkIdentity = prefab.GetComponent<NetworkIdentity>();
                if (networkIdentity == null)
                {
                    Debug.LogError($"AssetProvider: На префабе '{prefab.name}' отсутствует компонент NetworkIdentity. Это необходимо для сетевых объектов.");
                    return null;
                }
            }
             
            GameObject instance = GameObject.Instantiate(prefab);
             
            if (instance == null)
            {
                Debug.LogError($"AssetProvider: Не удалось создать инстанс префаба '{prefab.name}'.");
                return null;
            }

            Debug.Log($"AssetProvider: Успешно создан сетевой объект '{prefab.name}' (Инстанс: {instance.name})");
            return instance;
        }
    }

}