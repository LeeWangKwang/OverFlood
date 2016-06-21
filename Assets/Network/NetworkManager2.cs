using UnityEngine;
using System.Collections;

public class NetworkManager2 : MonoBehaviour
{

    public GameObject playerPrefab;
    private const string gameName = "OVER Flood";

    private bool refreshing;
    private HostData[] hostData;

    private bool isRefresh = false;

    private void StartServer()
    {
        Network.InitializeServer(32, 25001, !Network.HavePublicAddress());
        MasterServer.RegisterHost(gameName, "OF Tutorial Game");
    }

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(gameName);
        refreshing = true;
    }

    void Update()
    {
        if (refreshing)
        {
            if (MasterServer.PollHostList().Length > 0)
            {
                refreshing = false;
                Debug.Log(MasterServer.PollHostList().Length);
                hostData = MasterServer.PollHostList();
            }
        }

    }

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void spawnPlayer()
    {
        Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }

    //Messages
    void OnServerInitialized()
    {
        Debug.Log("Server initialized!");
        spawnPlayer();
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
        spawnPlayer();
    }

    void OnMasterServerEvent(MasterServerEvent mse)
    {
        if (mse == MasterServerEvent.HostListReceived)
        {
            hostData = MasterServer.PollHostList();
            Debug.Log("Registered Server!");
        }

    }
    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (!isRefresh)
            {


                if (GUI.Button(new Rect(100, 100, 250, 100), "방 만들기"))
                    StartServer();

                if (GUI.Button(new Rect(100, 250, 250, 100), "방 새로고침"))
                {
                    isRefresh = true;
                    RefreshHostList();
                }

            }
          

                if (hostData != null)
                {
                    for (int i = 0; i < hostData.Length; i++)
                    {
                        if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostData[i].gameName + " 참가"))

                            JoinServer(hostData[i]);
                    }
                }
            if (isRefresh) {
                if (GUI.Button(new Rect(100, 400, 300, 100), "돌아가기"))
                {
                    isRefresh = false;
                }
            }
            
        }
    }
}