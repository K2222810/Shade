using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.Tilemaps;

public class changeuniverse : MonoBehaviour
{
    [SerializeField]SimplePlayerMovement player;

    [SerializeField] private string tagtoCheck = "whiteobstacles";
    [SerializeField] private string tagtoCheck2 = " ";
    bool foundTaggedchild = false;
    bool turnoffcolliders = false;

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
                Debug.Log("NO with tag" + tagtoCheck);
            }
        }
    }
    private void nocolliders()
    {
        if(turnoffcolliders)
        {
            Debug.Log("YES I AM ON");
        }
    }
    private void worldtransp()
    {
        if (player.worldisblack)
        {
  /*          GameObject whiteObstacles = GameObject.FindGameObjectWithTag("whiteobstacles");
            if (whiteObstacles)
            {
                Debug.Log("HEllo there");
            }
            if (whiteObstacles)
            {
                whiteObstacles.SetActive(true);
                TilemapCollider2D whiteObstaclesCollider = whiteObstacles.GetComponent<TilemapCollider2D>();
                whiteObstaclesCollider.enabled = true;
            }*/
        }
    }
    /*private void checktag()
    {
        bool foundTaggedChild = false;
        foreach (Transform child in transform)
        {
            if (child.CompareTag(tagtoCheck))
            {
                foundTaggedChild = true;
                Debug.Log("Found a child with tag " + tagtoCheck + "-->" + child.name);
            }
        }
        if (!foundTaggedChild)
        {
            Debug.Log("No children with tag :" + tagtoCheck);
        }
    }*/
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
