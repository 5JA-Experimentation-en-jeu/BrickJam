using Unity.Netcode;
using UnityEngine;

public class Balle : NetworkBehaviour
{
    public float forceLance;
    bool lancee = false;
    Rigidbody2D rb;
    float minX, maxX, minY, maxY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Camera cameraJoueur = GameManager.Instance.ObtenirCameraJoueur(OwnerClientId);

        var limites = cameraJoueur.GetComponent<LimitesEcranJoueur>();

        float demiRayon = GetComponent<CircleCollider2D>().radius * transform.lossyScale.x;

        limites.CalculerLimites(demiRayon, demiRayon);
        minX = limites.minX;
        maxX = limites.maxX;
        minY = limites.minY;
        maxY = limites.maxY;
    }


    [ServerRpc(RequireOwnership = false)]
    public void LancerViaServerRpc()
    {
        if (!IsServer || lancee) return;

        lancee = true;

        Vector2 directionRandom = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
        rb.AddForce(directionRandom * forceLance, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (!IsServer || !lancee) return;

        // Si la balle sort par le bas de l'écran, la désactiver
        if (transform.position.y <= minY)
        {
            DespawnBalleServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnBalleServerRpc()
    {
        if (IsServer && NetworkObject != null)
        {
            NetworkObject.Despawn();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        BriqueNetwork brique = collision.gameObject.GetComponent<BriqueNetwork>();
        if (brique != null)
        {
            collision.gameObject.GetComponent<NetworkObject>().Despawn();
        }
  }
}
