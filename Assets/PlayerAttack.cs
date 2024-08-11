using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private Transform _gunFirePosition;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletForce = 5f;
    private RaycastHit _ray;
    private GameObject _gunInstance;

    private float _velocity;
    private Vector3 _velocityVector;
    private bool _hasWeapon = false;

    private void Start() 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward * 1000f, out _ray);
        Debug.DrawRay(Camera.main.transform.position,Camera.main.transform.forward * 1000f, Color.red);

        if(!_hasWeapon) return;

        _gunInstance.transform.position = _gunHolder.position;

        _gunInstance.transform.rotation = Quaternion.Euler(_gunHolder.eulerAngles.z, _gunHolder.eulerAngles.y -90, -_gunHolder.eulerAngles.x);

        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ReloadWeapon();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Pistol>() && _hasWeapon == false)
        {
            _hasWeapon = true;
            _gunInstance = Instantiate(other.gameObject, _gunHolder);
            _gunInstance.GetComponent<Rigidbody>().isKinematic = true;
            _gunInstance.GetComponent<BoxCollider>().enabled = false;
            _gunInstance.GetComponent<SphereCollider>().enabled = false;
            Destroy(other.gameObject);
        }
    }

    private void ReloadWeapon()
    {
        _gunInstance.GetComponent<Pistol>().Reload();
    }

    private void Fire()
    {
        Debug.Log("Is firing");
        if(_ray.collider == null)
        {
            _gunInstance.GetComponent<Pistol>().Shoot(_bullet,_gunFirePosition,_gunFirePosition.forward * 100);
        }
        else
        {
            Vector3 newBulletDirection = _ray.point - _gunFirePosition.position;
            _gunInstance.GetComponent<Pistol>().Shoot(_bullet,_gunFirePosition, newBulletDirection);;
        }
    }
}
