using Unity.Netcode;
using UnityEngine;

public class GenereBriques : NetworkBehaviour
{
    public GameObject brique;
    public int rangee;
    public int colonne;
    public float espaceBriques;
    public Color couleurBriqueJ1;
    public Color couleurBriqueJ2;

    float demiLargeurBrique;
    float demiHauteurBrique;

    public void GenererBriques(ulong clientId, Vector3 gauche, Vector3 droite, Vector3 haut)
    {
        if (!IsServer) return;


        var boxCollider = brique.GetComponent<BoxCollider2D>();
        // Calculer les limites avec la taille d'une brique
        demiLargeurBrique = boxCollider.size.x * brique.transform.lossyScale.x / 2;
        demiHauteurBrique = boxCollider.size.y * brique.transform.lossyScale.y / 2;

        float largeurTotale = colonne * (demiLargeurBrique * 2 + espaceBriques) - espaceBriques;
        float debutX = (droite.x + gauche.x - largeurTotale) / 2f + demiLargeurBrique;
        float debutY = haut.y - espaceBriques;

        Color couleurBrique = (clientId == 0) ? couleurBriqueJ1 : couleurBriqueJ2;

        for (int i = 0; i < rangee; i++)
        {
            for (int j = 0; j < colonne; j++)
            {
                float x = debutX + j * (demiLargeurBrique * 2 + espaceBriques);
                float y = debutY - i * (demiHauteurBrique * 2 + espaceBriques);
                Vector3 positionBrique = new Vector3(x, y, 0);

                GameObject nouvelleBrique = Instantiate(brique, positionBrique, Quaternion.identity);
                nouvelleBrique.GetComponent<NetworkObject>().Spawn();

                // Définir la couleur de la brique via le script réseau
                BriqueNetwork briqueNetwork = nouvelleBrique.GetComponent<BriqueNetwork>();
                briqueNetwork.DefinirCouleur(couleurBrique, clientId);
            }
        }
    }
}