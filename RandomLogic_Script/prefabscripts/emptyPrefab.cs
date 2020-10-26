using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emptyPrefab : MonoBehaviour
{
    GameObject collectioncanvas;
    // Start is called before the first frame update
    void Start()
    {
        collectioncanvas = GameObject.Find("CollectionCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickThis()
    {
        collectioncanvas.SetActive(false);
        Destroy(this.gameObject);
    }

}
