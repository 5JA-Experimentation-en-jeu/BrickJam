using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class Balle : NetworkBehaviour
{
    public float forceLance;
    bool lancee = false;
    Rigidbody2D rb;
    public Camera cameraJoueur;
    float minX, maxX, minY, maxY;
    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!IsOwner) return;

        // Récupérer les limites depuis la caméra du joueur
        var limites = cameraJoueur.GetComponent<LimitesEcranJoueur>();

        float demiLargeur = GetComponent<CircleCollider2D>().radius * transform.lossyScale.x;
        float demiHauteur = demiLargeur;
        limites.CalculerLimites(demiLargeur, demiHauteur);

        minX = limites.minX;
        maxX = limites.maxX;
        minY = limites.minY;
        maxY = limites.maxY;

        if (!lancee)
        {
            lancerBalle();
            lancee = true;
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        Vector3 position = transform.position;
        
        // Re
    }

  void lancerBalle()
    {
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * 500);
    }
}
