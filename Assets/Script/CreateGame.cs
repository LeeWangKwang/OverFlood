using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.OverFlood
{
    public class CreateGame : MonoBehaviour
    {
        public string gameName = "OVER FLOOD";
        public LobbyManager lobbyManager;

        public InputField RoomName;
        public void OnCreateBtn()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.matchMaker.CreateMatch(
                RoomName.text,
                4,
                true,
                gameName,
                lobbyManager.OnMatchCreate);
            Network.InitializeServer(32, 25001, !Network.HavePublicAddress());
            MasterServer.RegisterHost(gameName, "OF");
            lobbyManager.ChangeTo(lobbyManager.lobbyPanel);
        }
        public void OnCancelBtn()
        {
            lobbyManager.ChangeTo(lobbyManager.mainPanel);
        }
    }

}