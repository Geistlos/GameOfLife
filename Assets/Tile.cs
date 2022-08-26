using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int coordX;
    public int coordY;
    GridGenerator Gm;

    private void Start()
    {
        Gm = GridGenerator.Instance;
    }

    private void OnMouseDown()
    {
        if (Gm.gridValue[coordX, coordY] == 0)
        {
            Gm.gridValue[coordX, coordY] = 1;
            transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            Gm.gridValue[coordX, coordY] = 0;
            transform.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }
}
