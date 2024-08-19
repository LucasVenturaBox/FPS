using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private float _smoothness = 3;
    [SerializeField] private float _lerpSmoothness = 5f;
    [SerializeField] private Transform _gunFirePosition;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletForce = 5f;
    private RaycastHit _ray;
    private GameObject _gunInstance;

    public static event Action<string> onInteractionTextPrompt;
    public static event Action onHideInteractionTextPrompt;
    public static event Action<int,int> onWeaponBulletCounterChange;

    private float _velocity;
    private Vector3 _velocityVector;
    private bool _hasWeapon = false;
    private Weapon _weapon;

    [SerializeField] private Vector3 refVelocityVector = new Vector3(1,1,1);
    private float refVelocity = 0;

    private void Start() 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(!_hasWeapon) return;

         if(Input.GetMouseButton(0))
        {
            Fire();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ReloadWeapon();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(_hasWeapon)
            {
                _hasWeapon = false;
                _gunInstance.transform.parent = null;
                _gunInstance.GetComponent<Weapon>().PickupWeapon(false);
                onWeaponBulletCounterChange?.Invoke(0,0);


            }
        }

    }   
    private void LateUpdate()
    {
        Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward * 1000f, out _ray);
        Debug.DrawRay(Camera.main.transform.position,Camera.main.transform.forward * 1000f, Color.red);

        if(!_hasWeapon) return;

        _gunInstance.transform.position = Vector3.MoveTowards(_gunInstance.transform.position, _gunHolder.position, Time.deltaTime * _lerpSmoothness);

        float rotationX = Mathf.SmoothDampAngle(_gunInstance.transform.eulerAngles.x, _gunHolder.eulerAngles.z, ref refVelocity, Time.deltaTime * _smoothness);
        float rotationY = Mathf.SmoothDampAngle(_gunInstance.transform.eulerAngles.y , _gunHolder.eulerAngles.y -90, ref refVelocity, Time.deltaTime * _smoothness);
        float rotationZ = Mathf.SmoothDampAngle(_gunInstance.transform.eulerAngles.z, -_gunHolder.eulerAngles.x, ref refVelocity, Time.deltaTime * _smoothness);
        
        _gunInstance.transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

       
    }

    private void OnTriggerEnter(Collider other)
    {
        string interactibleName = other?.GetComponent<IInteractable>().GetObjectName();
        onInteractionTextPrompt?.Invoke(interactibleName);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            onHideInteractionTextPrompt?.Invoke();
        }

    }

    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.GetComponent<Weapon>() && _hasWeapon == false)
            {
                _hasWeapon = true;
                _gunInstance = other.gameObject;
                _weapon = _gunInstance.GetComponent<Weapon>();
                _weapon.PickupWeapon(true);
                onHideInteractionTextPrompt?.Invoke();
                onWeaponBulletCounterChange?.Invoke(_weapon.BulletsOnClip, _weapon.ReserveAmmo);
            }
            else if (other.GetComponent<BulletClip>() && _hasWeapon)
            {
                int newBullets = other.GetComponent<BulletClip>().AmountOfBullets;
                _gunInstance?.GetComponent<Weapon>().MoreAmmo(newBullets);
                Destroy(other.gameObject);
                onHideInteractionTextPrompt?.Invoke();
                onWeaponBulletCounterChange?.Invoke(_weapon.BulletsOnClip, _weapon.ReserveAmmo);

            }
        }
    }

    private void ReloadWeapon()
    {
        _gunInstance.GetComponent<Weapon>().Reload();
    }

    private void Fire()
    {
        if(_ray.collider == null)
        {
            _gunInstance.GetComponent<Weapon>().Shoot(_bullet,_gunFirePosition,_gunFirePosition.forward * 100);
        }
        else
        {
            Vector3 newBulletDirection = _ray.point - _gunFirePosition.position;
            _gunInstance.GetComponent<Weapon>().Shoot(_bullet,_gunFirePosition, newBulletDirection);;
        }

        onWeaponBulletCounterChange?.Invoke(_weapon.BulletsOnClip, _weapon.ReserveAmmo);

    }
}
