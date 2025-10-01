using System.Collections;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class Balle : NetworkBehaviour
{
    public float forceLance;
    bool lancee = false;
    Rigidbody2D rb;
    float minY;

    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calculer les limites selon la caméra du joueur
        Camera cameraJoueur = GameManager.Instance.ObtenirCameraJoueur(OwnerClientId);
        var limites = cameraJoueur.GetComponent<LimitesEcranJoueur>();

        float demiRayon = GetComponent<CircleCollider2D>().radius * transform.lossyScale.x;
        limites.CalculerLimites(demiRayon, demiRayon);
        minY = limites.minY;

        if (IsOwner)
        {
            Invoke(nameof(DemarrerLancement), 2f);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void DemarrerLancement()
    {
        LancerViaServerRpc();
    }

    [ServerRpc (RequireOwnership = false)]
    public void LancerViaServerRpc()
    {
        if (lancee) return;

        lancee = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 directionLance = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        rb.AddForce(directionLance * forceLance, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (!IsServer || !lancee) return;

        // Si la balle sort par le bas de l'écran, la désactiver
        if (transform.position.y <= minY)
        {
            PointAdversaire();
        }
    }

    void PointAdversaire()
    {
        // Donner un point à l'adversaire
        GameManager.Instance.AjouterPointAdversaire(OwnerClientId);

        StartCoroutine(AttendreRespawn());
    }

    IEnumerator AttendreRespawn()
    {
        yield return new WaitForSeconds(2f);

        if (IsServer)
        {
            GameManager.Instance.RespawnBalle(OwnerClientId);
            NetworkObject.Despawn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void RespawnBalleServerRpc(ulong clientId)
    {
        GameManager.Instance.RespawnBalle(clientId);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        var brique = collision.gameObject.GetComponent<BriqueNetwork>();
        if (brique != null)
        {
            collision.gameObject.GetComponent<NetworkObject>().Despawn();
            // Ajouter un point au joueur qui possède la brique
            GameManager.Instance.AjouterPointPourClient(brique.proprietaireClientId);
        }
  }
}
