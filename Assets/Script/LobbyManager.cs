using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;

namespace Prototype.OverFlood
{

    public class LobbyManager : NetworkLobbyManager
    {

        public string gameName = "OVER FLOOD";
        public int _playerNumber = 0;
        static public LobbyManager s_Singleton;

        public RectTransform mainPanel;
        public RectTransform lobbyPanel;
        public RectTransform CreatePanel;
        protected RectTransform currentPanel;
        public GameObject addPlayerButton;
        public GameObject refreshButton;
        CreateGame create;

        private HostData[] hostData;


        void start()
        {
            currentPanel = mainPanel;
            s_Singleton = this;
        }
        public void OnClickCreateBtn()
        {
            ChangeTo(CreatePanel);
        }

        public void OnClickRefreshBtn()
        {
            MasterServer.RequestHostList(gameName);
            hostData = MasterServer.PollHostList();
            Debug.Log(MasterServer.PollHostList().Length);

        }
        void OnGUI()
        {
            HostData[] data = MasterServer.PollHostList();
            // Go through all the hosts in the host list
            foreach (var element in data)
            {
                GUILayout.BeginHorizontal();
                var name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
                GUILayout.Label(name);
                GUILayout.Space(5);
                string hostInfo;
                hostInfo = "[";
                foreach (var host in element.ip)
                    hostInfo = hostInfo + host + ":" + element.port + " ";
                hostInfo = hostInfo + "]";
                GUILayout.Label(hostInfo);
                GUILayout.Space(5);
                GUILayout.Label(element.comment);
                GUILayout.Space(5);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Connect"))
                {
                    // Connect to HostData struct, internally the correct method is used (GUID when using NAT).
                    Network.Connect(element);
                }
                GUILayout.EndHorizontal();
            }
        }


        public void ChangeTo(RectTransform newPanel)
        {
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null)
            {
                newPanel.gameObject.SetActive(true);
            }

            currentPanel = newPanel;
            /*
            if (currentPanel != mainPanel)
            {
                backButton.gameObject.SetActive(true);
            }
            else
            {
                backButton.gameObject.SetActive(false);
                SetServerInfo("Offline", "None");
                _isMatchmaking = false;
            }*/
        }


    }
    


}