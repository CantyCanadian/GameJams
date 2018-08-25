using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float RotationSpeed = 4.0f;

	void Update ()
    {
        float horizontal = Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0.0f, horizontal + transform.eulerAngles.y, 0.0f);
    }
}
