using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField][Range(0.1f,5)] private float _cameraSensivity = 1f;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _cameraHolder;

    public Transform CameraHolder => _cameraHolder;
    
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

        _cameraHolder.rotation = Quaternion.Euler(-Input.mousePosition.y * _cameraSensivity, Input.mousePosition.x * _cameraSensivity,0);

        _playerCamera.transform.rotation = _cameraHolder.rotation;
        _playerCamera.transform.position = _cameraHolder.position;
        
        transform.right = _cameraHolder.right;
    }
}
