using Unity.Netcode;
using UnityEngine;

public class Balle : NetworkBehaviour
{
    public float forceLance;
    bool lancee = false;
    bool pretALancer = false;
    Rigidbody2D rb;
    public Camera cameraJoueur;
    float minX, maxX, minY, maxY;
    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!IsOwner) return;

        cameraJoueur = OwnerClientId == 0 ? GameObject.Find("CameraJ1").GetComponent<Camera>() : GameObject.Find("CameraJ2").GetComponent<Camera>();

        float demiRayon = GetComponent<CircleCollider2D>().radius * transform.lossyScale.x;

        // Récupérer les limites depuis la caméra du joueur
        var limites = cameraJoueur.GetComponent<LimitesEcranJoueur>();
        limites.CalculerLimites(demiRayon, demiRayon);
        minX = limites.minX;
        maxX = limites.maxX;
        minY = limites.minY;
        maxY = limites.maxY;

        pretALancer = true;
    }

    void Update()
    {
        if (!IsOwner) return;

        if (pretALancer && !lancee)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                lancerBalle();
                lancee = true;
            }
        }

        if (lancee)
        {
          // Si la balle sort par le bas de l'écran, la désactiver
        if (transform.position.y <= minY)
        {
            GetComponent<NetworkObject>().Despawn();
        }  
        }
    }

  void lancerBalle()
    {
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * forceLance);
    }
}
