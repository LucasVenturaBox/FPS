using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _cameraSensivity = 0.1f;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _cameraHolder;

    public Transform CameraHolder => _cameraHolder;

     float mouseX;
     float mouseY;

    private void Update() {

        Vector3 direction = Vector3.zero;

        if(Input.GetKey(KeyCode.W))
        {
            direction += transform.forward;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            direction -= transform.forward;
        }

        if(Input.GetKey(KeyCode.A))
        {
            direction -= transform.right; 
        }
        else if(Input.GetKey(KeyCode.D))
        {
            direction += transform.right;
        }

       
        transform.position += direction * _speed * Time.deltaTime;


        mouseX += Input.GetAxisRaw("Mouse X") * _cameraSensivity;
        mouseY += Input.GetAxisRaw("Mouse Y") * _cameraSensivity;

        mouseY = Mathf.Clamp(mouseY, -90,90);
        _cameraHolder.rotation = Quaternion.Euler(-mouseY, mouseX, 0);

        _playerCamera.transform.rotation = _cameraHolder.rotation;
        _playerCamera.transform.position = _cameraHolder.position;
        
        transform.right = _cameraHolder.right;
    }
}
