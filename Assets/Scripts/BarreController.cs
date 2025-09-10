using Unity.Netcode;
using UnityEngine;

public class BarreController : NetworkBehaviour
{
    public float vitesse;

    void Update()
    {
        if (!IsOwner) return;

        float input = Input.GetAxis("Horizontal");
        Vector3 deplacement = new Vector3(input, 0, 0) * vitesse * Time.deltaTime;
        transform.position += deplacement;
    }
}
