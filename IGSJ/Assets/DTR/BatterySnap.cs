using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySnap : MonoBehaviour
{
    public GameObject battery;
    private Vector3 Scale;

    // Start is called before the first frame update
    void Start()
    {
        if (battery != null)
        {
            Scale = battery.transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (battery != null && !battery.GetComponent<FixedJoint>())
        {
            battery.transform.parent = this.gameObject.transform;
            battery.transform.localPosition = Vector3.zero;
            battery.transform.localEulerAngles = Vector3.zero;
            battery.transform.localScale = new Vector3(1, 1, 1);
            battery.GetComponent<Rigidbody>().velocity = Vector3.zero;
            battery.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (battery == null && other.gameObject.tag == "Battery")
        {
            Scale = other.transform.localScale;
            battery = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == battery && other.gameObject.tag == "Battery")
        {
            battery = null;
            other.transform.parent = null;
            other.transform.localScale = Scale;
        }
    }
}
