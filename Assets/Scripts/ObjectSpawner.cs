using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawner : MonoBehaviour
{
    public enum ObjectType { SmallGem, BigGem, Enemy }

    public Tilemap tilemap;
    public GameObject[] objectPrefabs; //0=SmallGem, 1=BigGem, 2=Enemy
    public float bigGemProbability = 0.2f; //20% chance of spawning big gem
    public float enemyProbability = 0.1f;
    public int maxObjects = 5;
    public float gemLifeTime = 10f; //Only for gems
    public float spawnInterval = 0.5f;

    private List<Vector3> validSpawnPositions = new List<Vector3>();
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        GatherValidPositions();
        StartCoroutine(SpawnObjectsIfNeeded());
    }

    // Update is called once per frame
    void Update()
    {
        if (!tilemap.gameObject.activeInHierarchy)
        {
            LevelChange();
        }
        if (!isSpawning && ActiveObjectCount() < maxObjects)
        { 
            StartCoroutine(SpawnObjectsIfNeeded()); 
        }
    }

    private void LevelChange()
    { 
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        GatherValidPositions();
        DestroyAllSpawnedObjects();
    }


    private int ActiveObjectCount()
    {
        spawnedObjects.RemoveAll(item => item == null);
        return spawnedObjects.Count;        
    }

    private IEnumerator SpawnObjectsIfNeeded()
    { 
        isSpawning = true;
        while (ActiveObjectCount() < maxObjects)
        {
            SpawnObject(); 
            yield return new WaitForSeconds(spawnInterval);
        
        }
        isSpawning = false;    
    }

    private bool positionhasObject(Vector3 positionToCheck)
    {
 
        return spawnedObjects.Any(checkObj => checkObj && Vector3.Distance(checkObj.transform.position, positionToCheck) < 1.0f );
    }

    private ObjectType RandomObjectType()
    {
        float randomChoice = Random.value;

        if (randomChoice <= enemyProbability)
        {
            return ObjectType.Enemy;
        }
        else if (randomChoice <= (enemyProbability + bigGemProbability))
        {
            return ObjectType.BigGem;
        }
        else
        {
            return ObjectType.SmallGem;
        }
    }



    private void SpawnObject()
    {
        if (validSpawnPositions.Count == 0) return;

        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        while (!validPositionFound && validSpawnPositions.Count > 0)
        { 
            int Randomindex = Random.Range(0, validSpawnPositions.Count);
            Vector3 potentialPosition = validSpawnPositions[Randomindex];   
            Vector3 leftPosition = potentialPosition + Vector3.left;
            Vector3 RightPosition = potentialPosition + Vector3.right;

            if (!positionhasObject(leftPosition) && !positionhasObject(RightPosition))
            { 
                spawnPosition = leftPosition;
                validPositionFound = true;
            }
            
            validSpawnPositions.RemoveAt(Randomindex);
        }
        if (validPositionFound)
        {
            ObjectType objectType = RandomObjectType();
            GameObject gameObject = Instantiate(objectPrefabs[(int)objectType], spawnPosition, Quaternion.identity);
            spawnedObjects.Add(gameObject);

            if (objectType != ObjectType.Enemy)
            {
                StartCoroutine(DestroyObjectAfterTime(gameObject, gemLifeTime));
            }
        }
    }

    private IEnumerator DestroyObjectAfterTime(GameObject gameObject,float time)
    {
        yield return new WaitForSeconds(time);

        if (gameObject)
        { 
            spawnedObjects.Remove(gameObject);  
            validSpawnPositions.Add(gameObject.transform.position);
            Destroy(gameObject); 
        }
    }

    private void DestroyAllSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {

                Destroy(obj);
            }
            spawnedObjects.Clear();

        }
    }
    private void GatherValidPositions()
    {
        validSpawnPositions.Clear();
        BoundsInt boundsInt = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt);
        Vector3 start = tilemap.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));

        for (int x = 0; x < boundsInt.size.x; x++)
        {
            for (int y = 0; y < boundsInt.size.y; y++)
            { 
            TileBase tile = allTiles[x + y * boundsInt.size.x];
                if (tile != null)
                {
                    Vector3 place = start + new Vector3();
                    validSpawnPositions.Add(place); 
                
                }
                
            }
        
        }

    }
}
