using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.Tilemaps;

public class changeuniverse : MonoBehaviour
{
    [SerializeField]SimplePlayerMovement player;

    private string tagtoCheck = "whiteobstacles";
    private string tagtoCheck2 = "blackobstacles";

    GameObject obstacles;
    bool foundTaggedchild = false;
    bool foundTaggedchild2 = false;
    bool turnoffcolliders2 = false;
    public bool blackuniverse = false;

    
    void Start()
    { 
    }
    private void Update()
    {
        if (player.worldisblack)
        {
            bool found = CheckEachChildren(transform);
            bool found2 = CheckEachChildren2(transform);
            if (!found)
            {
/*                Debug.Log("No with tag " + tagtoCheck);
*/            }
            if (!found2)
            {
/*                Debug.Log("No with tag " + tagtoCheck2);
*/            }
            blackworld();
        }
        if (player.worldiswhite)
        {
            bool found = CheckEachChildren(transform);
            bool found2 = CheckEachChildren2(transform);

            if (!found)
            {
/*                Debug.Log("No with tag " + tagtoCheck);
*/            }
            if (!found2)
            {
/*                Debug.Log("No with tag " + tagtoCheck2);
*/            }
            whiteworld();
        }


    }
    private void blackworld()
    {
            GameObject[] whiteObstaclesArray = GameObject.FindGameObjectsWithTag("whiteobstacles");
            foreach (GameObject obstacle in whiteObstaclesArray)
            {
                TilemapCollider2D whiteObstaclesCollider = obstacle.GetComponent<TilemapCollider2D>();
                TilemapRenderer tilemapRenderer = obstacle.GetComponent<TilemapRenderer>();

            if (whiteObstaclesCollider != null)
                {
                //WHITE TRANSP
                tilemapRenderer.material.color = new Color(255f/255f, 255f/255f,255f/255f,0.01f);
                whiteObstaclesCollider.enabled = false;
                }
            }
        GameObject[] blackObstaclesArray = GameObject.FindGameObjectsWithTag("blackobstacles");
        foreach (GameObject obstacle in blackObstaclesArray)
        {
            TilemapCollider2D blackObstaclesCollider = obstacle.GetComponent <TilemapCollider2D>();
            TilemapRenderer tilemapRenderer = obstacle.GetComponent<TilemapRenderer>();
            if (blackObstaclesCollider != null)
            {
                // NORMAL BLACK
                tilemapRenderer.material.color = new Color(0f/255f,0f/255f,0f/255f,1f);
                blackObstaclesCollider.enabled = true;
            }
        }
    }
    private void whiteworld()
    {
        GameObject[] whiteObstaclesArray = GameObject.FindGameObjectsWithTag("whiteobstacles");
        
        foreach (GameObject obstacle in whiteObstaclesArray)
        {
           
            TilemapCollider2D whiteObstaclesCollider = obstacle.GetComponent<TilemapCollider2D>();
            TilemapRenderer tilemapRenderer = obstacle.GetComponent<TilemapRenderer>();
            if (whiteObstaclesCollider != null)
            {
                //NORMAL WHITE
                tilemapRenderer.material.color = new Color(255f/255f,255f/255f,255F/255f,1f);
                whiteObstaclesCollider.enabled = true;
            }
        }
        GameObject[] blackObstaclesArray = GameObject.FindGameObjectsWithTag("blackobstacles");
        foreach (GameObject obstacle in blackObstaclesArray)
        {
            TilemapCollider2D blackObstaclesCollider = obstacle.GetComponent<TilemapCollider2D>();
            TilemapRenderer tilemapRenderer = obstacle.GetComponent<TilemapRenderer>();
            if (blackObstaclesCollider != null)
            {
                //BLACK TRANSP
                tilemapRenderer.material.color = new Color(0f/255f,0f/255f, 0f/255f,0.05f);
                blackObstaclesCollider.enabled = false;
            }
        }
    }
    bool CheckEachChildren(Transform parent) // its checking each transformer/is similar to GameObject; you can change the "parent" to any other name 
    {
        foreach (Transform child in parent)
        {
            if(child.CompareTag(tagtoCheck))
            {
/*                Debug.Log("Found Tagged desccendant: " + child.name);
*/                foundTaggedchild = true;
                // return true
            }
            if (CheckEachChildren(child))
            {
                foundTaggedchild = true;
                // return true 
            }
        }
        return foundTaggedchild;
    }
    bool CheckEachChildren2(Transform parent) // its checking each transformer/is similar to GameObject; you can change the "parent" to any other name 
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tagtoCheck2))
            {
/*                Debug.Log("Found Tagged desccendant: " + child.name);
*/                foundTaggedchild2 = true;
                // return true
            }
            if (CheckEachChildren(child))
            {
                foundTaggedchild2 = true;
                // return true 
            }
        }
        return foundTaggedchild;
    }
}
