using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fonctions appelées par les boutons du menu
    public void LancerHote()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void LancerClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    void Update()
    {
        if (!IsHost) return;

    }
}
