using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float mouseSensity = 100f;
    float xRotation = 0f;
    public Transform player;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensity * Time.deltaTime;

       


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);

        // transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

    }
    public void LateUpdate()
    {
        //    float xAxisValue = Input.GetAxis("Horizontal")*0.7f;
        //    float zAxisValue = Input.GetAxis("Vertical") * 0.7f;

        //    transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y, transform.position.z + zAxisValue);
        //}
    }
}
