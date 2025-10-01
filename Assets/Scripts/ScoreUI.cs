using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI texteScore;
    public ulong joueurId;

    void Start()
    {
        StartCoroutine(AttendreJoueur());
    }

    IEnumerator AttendreJoueur()
    {
        while (GameManager.Instance == null || !GameManager.Instance.joueursConnectes.ContainsKey(joueurId))
        {
            yield return null;
        }

        var joueur = GameManager.Instance.joueursConnectes[joueurId];
        var scoreManager = joueur.GetComponent<ScoreManager>();

        scoreManager.Score.OnValueChanged += (ancien, nouveau) =>
        {
            texteScore.text = "Score : " + nouveau;
        };

        texteScore.text = "Score : " + scoreManager.Score.Value;
    }
}
