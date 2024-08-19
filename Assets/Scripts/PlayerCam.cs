using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private Vector2 _sensibility = new Vector2();
    private Vector3 _cameraRotation = new Vector3();
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensibility.x;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensibility.y;

        _cameraRotation.x -= mouseY;
        _cameraRotation.y += mouseX;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -90f, 90f);
    
    }
}
