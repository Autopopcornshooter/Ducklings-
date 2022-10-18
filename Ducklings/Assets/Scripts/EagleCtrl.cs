using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    public float angle;
    Rigidbody rgd;
    void Start()
    {
        rgd = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        angle = Mathf.Atan2(rgd.velocity.y, rgd.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
