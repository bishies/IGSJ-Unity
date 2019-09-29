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
        Scale = new Vector3(0.02f, 0.04f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        if (battery != null && !battery.GetComponent<InteractableObject>().isBeingHeld)
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
        if (battery == null && /*other.gameObject.tag == "Battery" )*/(other.gameObject.tag == "RedBattery" || other.gameObject.tag == "BlueBattery" || other.gameObject.tag == "GreenBattery"))
        {
            battery = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == battery && /*other.gameObject.tag == "Battery" )*/(other.gameObject.tag == "RedBattery" || other.gameObject.tag == "BlueBattery" || other.gameObject.tag == "GreenBattery"))
        {
            battery = null;
            other.transform.parent = null;
            other.transform.localScale = Scale;
        }
    }
}
