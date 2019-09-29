using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LaunchArm : MonoBehaviour
{
    public GameObject arm;
    public float launchSpeed = 40f;


    public void launchArm()
    {
        arm.transform.position = arm.transform.position + arm.transform.forward * 2;
        Rigidbody rb = arm.GetComponent<Rigidbody>();
        rb.velocity = arm.transform.forward * launchSpeed;
    }
}
