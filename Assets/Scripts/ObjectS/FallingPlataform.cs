using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class FallingPlataform : MonoBehaviour
{
    public float fallwait = 2f;
    public float destroryWait = 1.0f;

    bool isFalling;
    Rigidbody2D rb; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall()); 
        }
    }

    private IEnumerator Fall()
    { 
        isFalling = true;
        yield return new WaitForSeconds(fallwait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject,destroryWait);
    
    }
}
