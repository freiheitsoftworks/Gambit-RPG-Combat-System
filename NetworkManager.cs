using Photon.Pun;
using UnityEngine;

// Script responsável por gerenciar a conexão com o Photon Cloud - conexão multiplayer Online
// usado no GameObject NetworkManager
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Conecta-se ao Photon Cloud
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao servidor Photon!");
        PhotonNetwork.JoinLobby(); // Entra no lobby
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Entrou no lobby, agora conectando à sala...");
        PhotonNetwork.JoinOrCreateRoom("SalaPrincipal", new Photon.Realtime.RoomOptions { MaxPlayers = 10 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Entrou na sala!");
        Vector3 spawnPosition = new Vector3(0, 2f, 0);
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}
