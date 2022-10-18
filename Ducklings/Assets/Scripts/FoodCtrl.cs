using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCtrl : MonoBehaviour
{

    CircleCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        pos.x = Random.RandomRange(0.0f, 1.0f);
        pos.y = Random.RandomRange(0.0f, 1.0f);

        Debug.Log("trigger!");

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

    //    pos.x = Random.RandomRange(0.0f, 1.0f);
    //    pos.y = Random.RandomRange(0.0f, 1.0f);

    //    Debug.Log("trigger!");

    //    transform.position = Camera.main.ViewportToWorldPoint(pos);
    //}
    
}
