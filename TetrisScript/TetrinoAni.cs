using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrinoAni : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestroy", 1);
    }

    private void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
