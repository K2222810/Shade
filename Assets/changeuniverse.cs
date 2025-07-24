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
/*        float r = 0.5f;
        float g = 0.8f;
        float b = 0.2f;
        float a = 0.7f;

        Color customColor = new Color(r, g, b, a);
        Renderer renderer = GetComponent<Renderer>();
        if(renderer != null )
        {
            renderer.material.color = customColor;
        }*/
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
            worldtransp();
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
            worldtransp1();
        }


    }
    private void worldtransp()
    {
            GameObject[] whiteObstaclesArray = GameObject.FindGameObjectsWithTag("whiteobstacles");
            foreach (GameObject obstacle in whiteObstaclesArray)
            {
                TilemapCollider2D whiteObstaclesCollider = obstacle.GetComponent<TilemapCollider2D>();
                if (whiteObstaclesCollider != null)
                {
                    whiteObstaclesCollider.enabled = false;
                }
            }
        GameObject[] blackObstaclesArray = GameObject.FindGameObjectsWithTag("blackobstacles");
        foreach (GameObject obstacle in blackObstaclesArray)
        {
            TilemapCollider2D blackObstaclesCollider = obstacle.GetComponent <TilemapCollider2D>();
            if(blackObstaclesCollider != null)
            {
                blackObstaclesCollider.enabled = true;
            }
        }
    }
    private void worldtransp1()
    {
        GameObject[] whiteObstaclesArray = GameObject.FindGameObjectsWithTag("whiteobstacles");
        foreach (GameObject obstacle in whiteObstaclesArray)
        {
            TilemapCollider2D whiteObstaclesCollider = obstacle.GetComponent<TilemapCollider2D>();
            if (whiteObstaclesCollider != null)
            {
                whiteObstaclesCollider.enabled = true ;
            }
        }
        GameObject[] blackObstaclesArray = GameObject.FindGameObjectsWithTag("blackobstacles");
        foreach (GameObject obstacle in blackObstaclesArray)
        {
            TilemapCollider2D blackObstaclesCollider = obstacle.GetComponent<TilemapCollider2D>();
            if (blackObstaclesCollider != null)
            {
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
