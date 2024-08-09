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

    private void Update()
    {
        Physics.Raycast(Camera.main.transform.position,transform.forward,out _ray);

        _gun.transform.position = _gunHolder.transform.position;

        _gun.transform.forward = -_gunHolder.right;

        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject newBullet = Instantiate(_bullet, _gunFirePosition.position, _gunFirePosition.rotation);
        if(_ray.collider == null)
        {
            newBullet.GetComponent<Rigidbody>().AddForce(_gunFirePosition.forward * _bulletForce, ForceMode.Impulse);
        }
        else
        {
            Vector3 newBulletDirection = _ray.transform.position - _gunFirePosition.position;
            newBullet.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(newBulletDirection) * _bulletForce, ForceMode.Impulse);
        }
    }
}
