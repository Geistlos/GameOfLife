using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator Instance { get; private set; }

    //CONTROLS
    bool mapGenerated;
    bool update;

    //LINKS IN INSPECTOR
    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    bool randomMap;
    [SerializeField]
    float speed = 0.2f;

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

                if (randomMap == true)
                {
                    var rand = (int)Mathf.Floor(Random.Range(0f, 1.5f));
                    gridValue[i, j] = rand;
                    newGridValue[i, j] = rand;
                    if (rand == 0)
                    {
                        tileRenderer[i, j].color = Color.black;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (mapGenerated && update)
            StartCoroutine(UpdateGrid());

        if (Input.GetKeyDown(KeyCode.Space))
            update = true;
    }

    IEnumerator UpdateGrid()
    {
        update = false;
        for (int i = 0; i < mapWidth - 1; i++)
        {
            for (int j = 0; j < mapHeight - 1; j++)
            {
                CheckTilesAround(i, j);
            }
        }
        yield return new WaitForSeconds(speed);
        ChangeGrid();
        update = true;
    }

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
}
