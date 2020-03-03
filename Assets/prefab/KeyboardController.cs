using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller;
    public float speed = 1.0f;
    // Update is called once per frame
    void Update()
    {
        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");

        //transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y, transform.position.z + zAxisValue);
        Vector3 move = transform.right * xAxisValue + transform.forward * zAxisValue;
        controller.Move(move * 12f* Time.deltaTime);
    }
}
