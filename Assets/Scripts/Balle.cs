using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class Balle : NetworkBehaviour
{
    public Camera cameraJoueur;
    float minX, maxX;
    bool lancee = false;

    void Start()
    {
        if (IsOwner && !lancee)
        {
            lancerBalle();
            lancee = true;
        }
    }
    
    void lancerBalle()
    {
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * 500);
    }
}
