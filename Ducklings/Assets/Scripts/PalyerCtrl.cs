using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalyerCtrl : MonoBehaviour
{
    public int m_moveSpeed;
    private float angle;

    Rigidbody rgd;
    // Start is called before the first frame update
    void Start()
    {
        rgd = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
           
            rgd.AddForce(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
           
            rgd.AddForce(Vector3.left);
        }
        if (Input.GetKey(KeyCode.S))
        {
           
            rgd.AddForce(Vector3.down);
        }
        if (Input.GetKey(KeyCode.D))
        {
           
            rgd.AddForce(Vector3.right);
        }

        angle = Mathf.Atan2(rgd.velocity.y, rgd.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
