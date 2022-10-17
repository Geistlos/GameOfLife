using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //CONTROLS
    public int coordX;
    public int coordY;
    GridGenerator Gm;

    private void Start()
    {
        Gm = GridGenerator.Instance;
    }

    //Click on tile to switch from alive to dead or dead to alive
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
