#if ENABLE_UNET
 
namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class NetworkGUI : MonoBehaviour
    {
        [SerializeField] public NetworkManager manager;
        [SerializeField] public bool showGUI = true;
        [SerializeField] public int offsetX;
        [SerializeField] public int offsetY;
 
        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }
 
        void Update()
        {
            if (!showGUI)
                return;
 
            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    manager.StartHost();
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    manager.StartServer();
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    manager.StartClient();
                }
            }
            if (NetworkServer.active && NetworkClient.active)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    manager.StopHost();
                }
            }
        }
 
        void OnGUI()
        {
            if (!showGUI)
                return;
 
            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            int spacing = 24;
 
            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(Q)"))
                {
                    manager.StartHost();
                }
                ypos += spacing;
 
                if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(W)"))
                {
                    manager.StartClient();
                }
                manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
                ypos += spacing;
 
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(E)"))
                {
                    manager.StartServer();
                }
                ypos += spacing;
            }
            else
            {
                if (NetworkServer.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
                    ypos += spacing;
                }
                if (NetworkClient.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                    ypos += spacing;
                }
            }
 
            if (NetworkClient.active && !ClientScene.ready)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready")||Input.GetKeyDown(KeyCode.R))
                {
                    ClientScene.Ready(manager.client.connection);
               
                    if (ClientScene.localPlayers.Count == 0)
                    {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }
 
            if (NetworkServer.active || NetworkClient.active)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (T)")|| Input.GetKeyDown(KeyCode.T))
                {
                    manager.StopHost();
                }
                ypos += spacing;
            }
        }
    }
};
#endif //ENABLE_UNET