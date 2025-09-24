using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject Joueur1;
    public GameObject Joueur2;
    public GameObject balle;

    public Action OnDebutPartie;

    // Suivi des joueurs connectés
    Dictionary<ulong, GameObject> joueursConnectes = new();

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

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnClientConnectedCallback -= OnNouveauClientConnecte;
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

    public void EnregistrerJoueur(ulong clientId, GameObject joueur)
    {
        if (!joueursConnectes.ContainsKey(clientId))
        {
            joueursConnectes.Add(clientId, joueur);
        }

        if (joueursConnectes.Count == 2)
        {
            DebuterPartie();
        }
    }

    void DebuterPartie()
    {
        OnDebutPartie?.Invoke();

        foreach (var client in joueursConnectes)
        {
            ulong clientId = client.Key;

            Vector3 positionSpawn = client.Value.transform.position + Vector3.up * 8f;
            GameObject nouvelleBarre = Instantiate(balle, positionSpawn, Quaternion.identity);
            nouvelleBarre.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
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
}
