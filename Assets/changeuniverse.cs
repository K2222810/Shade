using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class changeuniverse : MonoBehaviour
{
    [SerializeField]SimplePlayerMovement player;

    public bool blackuniverse = false;
    private void Update()
    {
        worldtransp();
    }
    private void worldtransp()
    {
     if (player.worldisblack)
        {
            GameObject whiteObstacles = GameObject.FindGameObjectWithTag("whiteobstacles");
            if (whiteObstacles)
            {
                whiteObstacles.SetActive(true);
                TilemapCollider2D whiteObstaclesCollider = whiteObstacles.GetComponent<TilemapCollider2D>();
                whiteObstaclesCollider.enabled = true;
            }
        }
    }
    
}
