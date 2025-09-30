using Unity.Netcode;
using UnityEngine;

public class BriqueNetwork : NetworkBehaviour
{
    public SpriteRenderer spriteRenderer;
    public NetworkVariable<Color> couleurBrique = new NetworkVariable<Color>(
       new Color(1, 1, 1, 1),
       NetworkVariableReadPermission.Everyone,
       NetworkVariableWritePermission.Server);

    public ulong proprietaireClientId;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        couleurBrique.OnValueChanged += OnCouleurBriqueChange;
        spriteRenderer.color = couleurBrique.Value;
    }

    private void OnCouleurBriqueChange(Color ancienneValeur, Color nouvelleValeur)
    {
        spriteRenderer.color = nouvelleValeur;
    }

    public void DefinirCouleur(Color couleur, ulong clientId)
    {
        if (IsServer)
        {
            if (couleur.a == 0) couleur.a = 1; // Assurer que l'alpha n'est pas zéro
            couleurBrique.Value = couleur;
            proprietaireClientId = clientId;
        }
    }
}
