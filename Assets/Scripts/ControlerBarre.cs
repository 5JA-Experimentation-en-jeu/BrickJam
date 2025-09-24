using System;
using Unity.Netcode;
using UnityEngine;

public class ControlerBarre : NetworkBehaviour
{
    public GameObject balle;
    GameObject nouvelleBalle;
    public Transform pointLancementBalle;
    public float vitesse;

    float minX, maxX;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer && GameManager.Instance != null)
        {
            GameManager.Instance.EnregistrerJoueur(OwnerClientId, gameObject);
        }
    }

    void Start()
    {
        if (!IsOwner) return;

        GameManager.Instance.EnregistrerJoueur(OwnerClientId, gameObject);

        Camera cameraJoueur = GameManager.Instance.ObtenirCameraJoueur(OwnerClientId);
        // Récupérer les limites de l'écran pour ce joueur
        var limites = cameraJoueur.GetComponent<LimitesEcranJoueur>();

        // Calculer les limites avec la taille de la barre
        float demiLargeurBarre = GetComponent<BoxCollider2D>().size.x * transform.lossyScale.x / 2;
        float demiHauteurBarre = GetComponent<BoxCollider2D>().size.y * transform.lossyScale.y / 2;
        limites.CalculerLimites(demiLargeurBarre, demiHauteurBarre);

        minX = limites.minX;
        maxX = limites.maxX;
    }

    void Update()
    {
        if (!IsOwner) return;

        float input = Input.GetAxis("Horizontal");
        Vector3 deplacement = new Vector3(input, 0, 0) * vitesse * Time.deltaTime;
        Vector3 nouvellePosition = transform.position + deplacement;

        // Verifier les limites
        nouvellePosition.x = Mathf.Clamp(nouvellePosition.x, minX, maxX);
        transform.position = nouvellePosition;
    }
}