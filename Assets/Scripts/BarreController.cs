using Unity.Netcode;
using UnityEngine;

public class BarreController : NetworkBehaviour
{
    public float vitesse;

    [Header("Limites de déplacement")]
    public float marge = 0.5f;
    public Camera cameraJoueur;
    float minX, maxX;

    void Update()
    {
        if (!IsOwner) return;

        float input = Input.GetAxis("Horizontal");
        Vector3 deplacement = new Vector3(input, 0, 0) * vitesse * Time.deltaTime;
        Vector3 nouvellePosition = transform.position + deplacement;

        nouvellePosition.x = Mathf.Clamp(nouvellePosition.x, minX, maxX);

        transform.position = nouvellePosition;
    }
}
