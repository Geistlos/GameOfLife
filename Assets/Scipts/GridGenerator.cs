using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    //SINGLETON
    public static GridGenerator Instance { get; private set; }

    //CONTROLS
    bool mapGenerated;
    bool update;
    Coroutine co;

    //LINKS IN INSPECTOR
    [SerializeField]
    GameObject tilePrefab;
    public float speed;

    //STATS
    static int mapWidth = 100;
    static int mapHeight = 100;

    //GRIDS
    SpriteRenderer[,] tileRenderer = new SpriteRenderer[mapWidth, mapHeight];
    Tile[,] gridScript = new Tile[mapWidth, mapHeight];
    public int[,] gridValue = new int[mapWidth, mapHeight];
    int[,] newGridValue = new int[mapWidth, mapHeight];

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;

        InstantiationTiles();
    }

    private void Start()
    {
        mapGenerated = true;
    }

    //Instantiate tiles
    void InstantiationTiles()
    {
        for (int i = 0; i < mapWidth - 1; i++)
        {
            for (int j = 0; j < mapHeight - 1; j++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity, gameObject.transform);
                var script = tile.GetComponent<Tile>();
                script.coordX = i;
                script.coordY = j;
                gridScript[i, j] = script;
                tileRenderer[i, j] = tile.GetComponent<SpriteRenderer>();
            }
        }
    }

    private void Update()
    {
        //Update map if the timer is done && the map has been generated
        if (mapGenerated && update && co == null)
            co = StartCoroutine(UpdateGrid());
    }

    //Coroutine to wait a given time between each update
    IEnumerator UpdateGrid()
    {
        update = false;
        yield return new WaitForSeconds(speed);
        for (int i = 0; i < mapWidth - 1; i++)
        {
            for (int j = 0; j < mapHeight - 1; j++)
            {
                CheckTilesAround(i, j);
            }
        }

        ChangeGrid();
        update = true;
        co = null;
    }

    //Update the value and color of the tiles
    void ChangeGrid()
    {
        for (int i = 0; i < mapWidth - 1; i++)
        {
            for (int j = 0; j < mapHeight - 1; j++)
            {
                if (newGridValue[i, j] == 0)
                    tileRenderer[i, j].color = Color.black;
                else
                    tileRenderer[i, j].color = Color.white;
                gridValue[i, j] = newGridValue[i, j];
            }
        }
    }


    //Rules of life and death
    void CheckTilesAround(int coordX, int coordY)
    {
        var neighbour = 0;
        if (coordX > 0 && gridValue[coordX - 1, coordY] == 1)
        {
            neighbour++;
        }
        if (coordX > 0 && coordY > 0 && gridValue[coordX - 1, coordY - 1] == 1)
        {
            neighbour++;
        }
        if (coordY > 0 && gridValue[coordX, coordY - 1] == 1)
        {
            neighbour++;
        }
        if (coordX < mapWidth && coordY > 0 && gridValue[coordX + 1, coordY - 1] == 1)
        {
            neighbour++;
        }
        if (coordX < mapWidth && gridValue[coordX + 1, coordY] == 1)
        {
            neighbour++;
        }
        if (coordX < mapWidth && coordY < mapHeight && gridValue[coordX + 1, coordY + 1] == 1)
        {
            neighbour++;
        }
        if (coordY < mapHeight && gridValue[coordX, coordY + 1] == 1)
        {
            neighbour++;
        }
        if (coordX > 0 && coordY < mapHeight && gridValue[coordX - 1, coordY + 1] == 1)
        {
            neighbour++;
        }
        if (gridValue[coordX, coordY] == 1 && (neighbour <= 1 || neighbour >= 4))
        {
            newGridValue[coordX, coordY] = 0;
        }
        if (neighbour == 3)
        {
            newGridValue[coordX, coordY] = 1;
        }
        if (gridValue[coordX, coordY] == 1 && neighbour == 2)
        {
            newGridValue[coordX, coordY] = 1;
        }
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
        StopCoroutine(UpdateGrid());
        if (update && co == null)
            co = StartCoroutine(UpdateGrid());
    }

    public void RandomizeMap()
    {
        for (int i = 0; i < mapWidth - 1; i++)
        {
            for (int j = 0; j < mapHeight - 1; j++)
            {
                var rand = (int)Mathf.Floor(Random.Range(0f, 1.5f));
                gridValue[i, j] = rand;
                newGridValue[i, j] = rand;
                if (rand != 0)
                {
                    tileRenderer[i, j].color = Color.white;
                }
            }
        }
    }

    public void Stop()
    {
        if (co != null)
            StopCoroutine(co);
        co = null;
    }

    public void StartUpdate()
    {
        update = true;
    }

    public void ResetMap()
    {
        for (int i = 0; i < mapWidth - 1; i++)
        {
            for (int j = 0; j < mapHeight - 1; j++)
            {
                gridValue[i, j] = 0;
                newGridValue[i, j] = 0;
                tileRenderer[i, j].color = Color.black;
            }
        }
    }
}
