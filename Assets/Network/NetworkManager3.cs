using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{

    GameObject playerPrefab;
    Transform spawnObject;
    private const string gameName = "OVER Flood";

    private bool refreshing;
    private HostData[] hostData;

    private float btnX = 100;
    private float btnY = 100;
    private float btnW = 250;
    private float btnH = 100;


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
        Network.Instantiate(playerPrefab, spawnObject.position, Quaternion.identity, 0);
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
            if (GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server"))
            {
                Debug.Log("Starting Server");
                StartServer();
            }

            if (GUI.Button(new Rect(btnX, btnY * 2 + btnH, btnW, btnH), "Refresh Hosts"))
            {
                Debug.Log("Refreshing");
                RefreshHostList();
            }
            if (hostData != null)
            {
                for (int i = 0; i < hostData.Length; i++)
                {
                    if (GUI.Button(new Rect(btnX * 3 + btnW, btnY * 2 + (btnH * i), btnW * 3, btnH * 1), hostData[i].gameName))
                    {
                        JoinServer(hostData[i]);
                    }
                }
            }
        }


    }
}