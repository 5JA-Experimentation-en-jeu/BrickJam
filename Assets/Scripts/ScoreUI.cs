using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI texteScore;
    public ulong joueurId;

    void Start()
    {
        if (texteScore == null) return;
        StartCoroutine(AttendreJoueur());
    }

    IEnumerator AttendreJoueur()
    {
        while (GameManager.Instance == null || !GameManager.Instance.joueursConnectes.ContainsKey(joueurId))
        {
            yield return null;
        }

        GameObject joueur = GameManager.Instance.joueursConnectes[joueurId];

        ScoreManager scoreManager = null;

        while (scoreManager == null)
        {
            scoreManager = joueur.GetComponent<ScoreManager>();
            yield return null;
        }

        texteScore.text = "Score : " + scoreManager.Score.Value;

        scoreManager.Score.OnValueChanged += (ancien, nouveau) =>
        {
            texteScore.text = "Score : " + nouveau;
        };
    }
}
