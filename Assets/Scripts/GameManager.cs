using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public GameObject Joueur1;
    public GameObject Joueur2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.OnClientConnectedCallback += OnNouveauClientConnecte;
    }

    private void OnNouveauClientConnecte(ulong clientId)
    {
        if (!IsServer) return;


        if (NetworkManager.Singleton.ConnectedClients.Count == 1)
        {
            GameObject nouveauJoueur = Instantiate(Joueur1);
            nouveauJoueur.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        }
        else if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            GameObject nouveauJoueur = Instantiate(Joueur2);
            nouveauJoueur.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        }
    }

    // Fonctions appelées par les boutons du menu
    public void LancerHote()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void LancerClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    void Update()
    {
        if (!IsHost) return;

    }
}
