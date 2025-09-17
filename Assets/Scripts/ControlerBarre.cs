using System;
using Unity.Netcode;
using UnityEngine;

public class ControlerBarre : NetworkBehaviour
{
    public GameObject balle;
    public float vitesse;
    public Camera cameraJoueur;

    float minX, maxX;

    void Start()
    {
        if (!IsOwner) return;

        if (OwnerClientId == 0)
        {
            cameraJoueur = GameObject.Find("CameraJ1").GetComponent<Camera>();
        }
        else if (OwnerClientId == 1)
        {
            cameraJoueur = GameObject.Find("CameraJ2").GetComponent<Camera>();
        }

        // Récupérer les limites de l'écran pour ce joueur
        var limites = cameraJoueur.GetComponent<LimitesEcranJoueur>();

        // Calculer les limites avec la taille de la barre
        float demiLargeurBarre = GetComponent<BoxCollider2D>().size.x * transform.lossyScale.x / 2;
        limites.CalculerLimites(demiLargeurBarre);

        minX = limites.minX;
        maxX = limites.maxX;

        // Spawner la balle
        if (IsOwner)
        {
            GameObject nouvelleBalle = Instantiate(balle);
            nouvelleBalle.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        }
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