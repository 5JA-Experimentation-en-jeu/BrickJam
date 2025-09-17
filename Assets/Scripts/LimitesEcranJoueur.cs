using UnityEngine;

public class LimitesEcranJoueur : MonoBehaviour
{
    [HideInInspector] public float minX, maxX;

    public void CalculerLimites(float demiLargeurObjet)
    {
        Camera cameraJoueur = GetComponent<Camera>();
        float distanceZ = Mathf.Abs(cameraJoueur.transform.position.z);

        // Calculer les limites visibles de la caméra
        Vector3 gauche = cameraJoueur.ViewportToWorldPoint(new Vector3(0, 0.5f, distanceZ));
        Vector3 droite = cameraJoueur.ViewportToWorldPoint(new Vector3(1, 0.5f, distanceZ));

        minX = gauche.x + demiLargeurObjet;
        maxX = droite.x - demiLargeurObjet;
    }
}
