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
        bool found = CheckEachChildren(transform);
        if (!found)
        {
            Debug.Log("No with tag " + tagtoCheck);
        }
        if(player.worldisblack)
        {
            worldtransp();

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
    }
    bool CheckEachChildren(Transform parent) // its checking each transformer/is similar to GameObject; you can change the "parent" to any other name 
    {
        foreach (Transform child in parent)
        {
            if(child.CompareTag(tagtoCheck))
            {
                Debug.Log("Found Tagged desccendant: " + child.name);
                foundTaggedchild = true;
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
}
