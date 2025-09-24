using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompteARebours : MonoBehaviour
{
    public static CompteARebours Instance;

    public TextMeshProUGUI texte;

    void Awake()
    {
        Instance = this;
        texte.enabled = false;
    }

    public void AfficherTexte(string message)
    {
        texte.text = message;
        texte.enabled = true;
    }

    public void CacherTexte()
    {
        texte.enabled = false;
    }
}
