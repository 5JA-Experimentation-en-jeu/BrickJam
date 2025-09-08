using UnityEngine;
using UnityEngine.SceneManagement;

public class LancerPartie : MonoBehaviour
{
    public static LancerPartie instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.LoadScene("Jeu");
    }
}
