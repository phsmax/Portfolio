using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 블럭 개별 유닛
/// </summary>
public class Tetrino : MonoBehaviour
{
    [SerializeField]
    int Myx;
    [SerializeField]
    int Myy;

    public int x
    {
        get
        {
            return Myx;
        }
        set
        {
            Myx = value;
        }
    }
    public int y
    {
        get
        {
            return Myy;
        }
        set
        {
            Myy = value;
        }
    }

    private void OnDestroy()
    {
        if (this.gameObject.tag == "fixed")
        {
            GameObject temp = Instantiate<GameObject>(Resources.Load("02prefab/tetrinoani", typeof(GameObject)) as GameObject);
            temp.transform.position = this.gameObject.transform.position;
            temp.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
        }
    }

}
