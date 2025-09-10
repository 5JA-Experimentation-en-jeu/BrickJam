using Unity.Netcode;
using UnityEngine;

public class Balle : NetworkBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        lancerBalle();
    }
    
    void lancerBalle()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector3(1, 1, 0) * 500);
    }
}
