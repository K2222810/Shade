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
    GameObject obstacles;
    [SerializeField] private string tagtoCheck2 = " ";
    bool foundTaggedchild = false;

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
            if (!found)
            {
                Debug.Log("No with tag " + tagtoCheck);
            }
        }
        worldtransp();
    }
    private void worldtransp()
    {
    
            Debug.Log("hello sir");
            GameObject whiteObstacles = GameObject.FindGameObjectWithTag("whiteobstacles");
            TilemapCollider2D whiteObstaclesCollider = whiteObstacles.GetComponent<TilemapCollider2D>();
            whiteObstaclesCollider.enabled = false;
        
        /*if (player.worldisblack)
        {
            if (whiteObstacles)
            {
                Debug.Log("HEllo there");
            }
            if (whiteObstacles)
            {
                whiteObstacles.SetActive(true);
                TilemapCollider2D whiteObstaclesCollider = whiteObstacles.GetComponent<TilemapCollider2D>();
            }
        }*/
    }
    bool CheckEachChildren(Transform parent) // its checking each transformer/is similar to GameObject; you can change the "parent" to any other name 
    {
        foreach (Transform child in parent)
        {
            if(child.CompareTag(tagtoCheck))
            {
                Debug.Log("Found Tagged desccendant: " + child.name);
                foundTaggedchild = true;
            }
            if (CheckEachChildren(child))
            {
                foundTaggedchild = true;
            }
        }
        return foundTaggedchild;
    }
}
