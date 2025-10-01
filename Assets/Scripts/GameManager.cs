using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;
using System.Collections;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject Joueur1;
    public GameObject Joueur2;
    public GameObject balle;

    public GameObject briquesManager;

    GenereBriques briques;

    public Action OnDebutPartie;

    bool partieLancee = false;

    // Suivi des joueurs connectés
    public Dictionary<ulong, GameObject> joueursConnectes = new();

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

    void GenererToutesLesBriques()
    {
        briques = briquesManager.GetComponent<GenereBriques>();

        foreach (var client in joueursConnectes)
        {
            ulong clientId = client.Key;
            Camera cam = ObtenirCameraJoueur(clientId);

            if (cam == null) continue;

            float distanceZ = Mathf.Abs(cam.transform.position.z);
            
            Vector3 gauche = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, distanceZ));
            Vector3 droite = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, distanceZ));
            Vector3 haut = cam.ViewportToWorldPoint(new Vector3(0.5f, 1, distanceZ));

            briques.GenererBriques(clientId, gauche, droite, haut);
        }
    }


    public void EnregistrerJoueur(ulong clientId, GameObject joueur)
    {
        if (!joueursConnectes.ContainsKey(clientId))
        {
            joueursConnectes.Add(clientId, joueur);
        }

        if (!partieLancee && joueursConnectes.Count == 2)
        {
            partieLancee = true;
            StartCoroutine(CompteAReboursDebutPartie());
        }
    }

    IEnumerator CompteAReboursDebutPartie()
    {
        yield return new WaitForSeconds(0.5f);

        // Afficher le compte à rebours
        AfficherTexteClientRpc("3");
        yield return new WaitForSeconds(1f);
        AfficherTexteClientRpc("2");
        yield return new WaitForSeconds(1f);
        AfficherTexteClientRpc("1");
        yield return new WaitForSeconds(1f);
        AfficherTexteClientRpc("GO!");

        CacherTexteClientRpc();

        OnDebutPartie?.Invoke();

        GenererToutesLesBriques();

        foreach (var client in joueursConnectes)
        {
            RespawnBalle(client.Key);
        }
    }

    [ClientRpc]
    void AfficherTexteClientRpc(string message)
    {
        CompteARebours.Instance.AfficherTexte(message);
    }

    [ClientRpc]
    void CacherTexteClientRpc()
    {
        CompteARebours.Instance.CacherTexte();
    }

    public Camera ObtenirCameraJoueur(ulong clientId)
    {
        if (clientId == 0)
        {
            return GameObject.Find("CameraJ1").GetComponent<Camera>();
        }
        else if (clientId == 1)
        {
            return GameObject.Find("CameraJ2").GetComponent<Camera>();
        }
        return null;
    }

    public void AjouterPointPourClient(ulong clientId)
    {
        if (joueursConnectes.TryGetValue(clientId, out var joueur))
        {
            var scoreManager = joueur.GetComponent<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AjouterPoint(); // Appelle la méthode serveur
            }
        }
    }

    public void AjouterPointAdversaire(ulong perdantId)
    {
        // Le clientId des joueurs est 0 ou 1
        ulong gagnantId = perdantId == 0 ? 1UL : 0UL;
        AjouterPointPourClient(gagnantId);
    }


    public void RespawnBalle(ulong clientId)
    {
        if (joueursConnectes.TryGetValue(clientId, out var joueur))
        {
            var controleur = joueur.GetComponent<ControlerBarre>();
            Vector3 positionSpawn = controleur.pointLancementBalle.position;
            GameObject nouvelleBalle = Instantiate(balle, positionSpawn, Quaternion.identity);

            nouvelleBalle.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            nouvelleBalle.GetComponent<Balle>().LancerViaServerRpc();
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
