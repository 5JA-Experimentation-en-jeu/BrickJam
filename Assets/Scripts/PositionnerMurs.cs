using UnityEngine;

public class PositionnerMurs : MonoBehaviour
{
    public Camera cameraJoueur;
    public BoxCollider2D murGauche, murDroite, murHaut;

    void Start()
    {
        float distanceZ = Mathf.Abs(cameraJoueur.transform.position.z);
        Vector3 gauche = cameraJoueur.ViewportToWorldPoint(new Vector3(0, 0.5f, distanceZ));
        Vector3 droite = cameraJoueur.ViewportToWorldPoint(new Vector3(1, 0.5f, distanceZ));
        Vector3 haut = cameraJoueur.ViewportToWorldPoint(new Vector3(0.5f, 1, distanceZ));

        float hauteurMur = 10f;
        float epaisseurMur = 1f;

        // Positionnement et taille des murs
        murGauche.transform.position = new Vector3(gauche.x - epaisseurMur / 2, gauche.y, 0);
        murGauche.size = new Vector2(epaisseurMur, hauteurMur);

        murDroite.transform.position = new Vector3(droite.x + epaisseurMur / 2, droite.y, 0);
        murDroite.size = new Vector2(epaisseurMur, hauteurMur);

        murHaut.transform.position = new Vector3(haut.x, haut.y + epaisseurMur / 2, 0);
        murHaut.size = new Vector2(droite.x - gauche.x + epaisseurMur * 2, epaisseurMur);
    }
}

