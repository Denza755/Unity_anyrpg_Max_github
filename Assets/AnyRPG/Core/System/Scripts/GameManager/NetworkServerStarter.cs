using FishNet;
using FishNet.Managing;
using UnityEngine;

public class NetworkServerStarter : MonoBehaviour
{
    private void Start()
    {
        // Этот код выполняется только в серверной сборке
        #if UNITY_SERVER
            StartCoroutine(WaitForNetworkManagerAndStart());
        #endif
    }

    private System.Collections.IEnumerator WaitForNetworkManagerAndStart()
    {
        // Ждем, пока InstanceFinder найдет активный NetworkManager
        while (InstanceFinder.NetworkManager == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        NetworkManager networkManager = InstanceFinder.NetworkManager;
        ApplyCommandLineArguments(networkManager);
        
        // Запускаем сервер
        networkManager.ServerManager.StartConnection();
        Debug.Log("[Server] Сервер успешно запущен.");
    }

    private void ApplyCommandLineArguments(NetworkManager networkManager)
    {
        string[] args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            // Установка порта: -port 7770
            if (args[i] == "-port" && i + 1 < args.Length)
            {
                ushort port = ushort.Parse(args[i + 1]);
                networkManager.TransportManager.Transport.SetPort(port);
                Debug.Log($"[Server] Порт установлен на: {port}");
            }
            // Установка IP-адреса для привязки: -ip 0.0.0.0
            else if (args[i] == "-ip" && i + 1 < args.Length)
            {
                string ip = args[i + 1];
                networkManager.TransportManager.Transport.SetServerBindAddress(ip, 0);
                Debug.Log($"[Server] Адрес привязки установлен на: {ip}");
            }
        }
    }
}