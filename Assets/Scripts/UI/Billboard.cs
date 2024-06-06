using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    GameObject main;

    private void Start()
    {
        main = GameObject.Find("Cam_FPS");
    }

    void Update()
    {
        transform.LookAt(transform.position + main.transform.rotation * Vector3.forward, main.transform.rotation * Vector3.up);
    }
}
