using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour,IInteractable
{
    //Data
    [SerializeField] private WeaponInfoSO _weaponInfoSO;

    //Cached References
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    //Weapon Events and States
    private event Action onCooldownFinished;
    private event Action onReloadEmptyClipFinished;
    private event Action onReloadClipFinished;
    private WeaponState _state;
    private Coroutine _cooldown, _reload;

    //Bullet Management Info
    private int _bulletsOnClip;
    private int _reserveAmmo;


    //Accessors
    public int BulletsOnClip => _bulletsOnClip;
    public int ReserveAmmo => _reserveAmmo;

    #region Unity
    private void OnEnable() {
        onCooldownFinished += WeaponReady;

        onReloadEmptyClipFinished += WeaponReady;
        onReloadEmptyClipFinished += LoadEmptyClip;

        onReloadClipFinished += WeaponReady;
        onReloadClipFinished += LoadClip;
    }

    private void OnDisable() {
        onCooldownFinished -= WeaponReady;

        onReloadEmptyClipFinished -= WeaponReady;
        onReloadEmptyClipFinished -= LoadEmptyClip;

        onReloadClipFinished -= WeaponReady;
        onReloadClipFinished -= LoadClip;

    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        if(_meshFilter.mesh == null || _meshFilter.mesh != _weaponInfoSO.WeaponMesh)
        {
            _meshFilter.mesh = _weaponInfoSO.WeaponMesh;
        }

        if(_meshRenderer.materials == null || _meshRenderer.materials != _weaponInfoSO.WeaponMaterials)
        {
            _meshRenderer.materials = _weaponInfoSO.WeaponMaterials;
        }
        
    }

    private void Start()
    {
        StopAllCoroutines();

        _state = WeaponState.Ready;
        _bulletsOnClip = _weaponInfoSO.ClipSize;
    }
    #endregion

    public void Shoot(GameObject bullet, Transform origin, Vector3 destination)
    {
        if(_state == WeaponState.Ready && _bulletsOnClip > 0)
        {

            if (bullet != null)
            {
                GameObject newBullet = Instantiate(bullet, origin.position, origin.rotation);

                Vector3 finalDestination;
                finalDestination.x = UnityEngine.Random.Range(destination.x - _weaponInfoSO.Accuracy, destination.x + _weaponInfoSO.Accuracy);
                finalDestination.y = UnityEngine.Random.Range(destination.y - _weaponInfoSO.Accuracy, destination.y + _weaponInfoSO.Accuracy);
                finalDestination.z = UnityEngine.Random.Range(destination.z - _weaponInfoSO.Accuracy, destination.z + _weaponInfoSO.Accuracy);

                newBullet.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(finalDestination) * _weaponInfoSO.MaxRange, ForceMode.Impulse);

                _bulletsOnClip--;

                _state = WeaponState.OnCooldown;

                _cooldown = StartCoroutine(Utils.DelaySeconds(_weaponInfoSO.FireRate, onCooldownFinished));
            }
        }
        else if(_state == WeaponState.Ready && _bulletsOnClip <= 0) 
        {
            Reload();
        }

    }

    public void Reload()
    {
        bool hasAmmo = _reserveAmmo > 0;

        if(hasAmmo && _state != WeaponState.Reloading)
        {
            if (_bulletsOnClip < _weaponInfoSO.ClipSize && _bulletsOnClip > 0)
            {
                _state = WeaponState.Reloading;

                if (_cooldown != null) StopCoroutine(_cooldown);

                _reload = StartCoroutine(Utils.DelaySeconds(_weaponInfoSO.ReloadTime, onReloadClipFinished));
            }
            else if(_bulletsOnClip < _weaponInfoSO.ClipSize && _bulletsOnClip == 0)
            {
                _state = WeaponState.Reloading;

                if (_cooldown != null) StopCoroutine(_cooldown);

                _reload = StartCoroutine(Utils.DelaySeconds(_weaponInfoSO.ReloadTime, onReloadEmptyClipFinished));
            }
        }
    }

    private void WeaponReady()
    {
        _state = WeaponState.Ready;
    }

    private void LoadEmptyClip()
    {
        if(_reserveAmmo >= _weaponInfoSO.ClipSize)
        {
            _reserveAmmo -= _weaponInfoSO.ClipSize;
            _bulletsOnClip = _weaponInfoSO.ClipSize;
        }
        else
        {
            _bulletsOnClip = _reserveAmmo;
            _reserveAmmo = 0;
        }
    }

    private void LoadClip()
    {
        int bulletsMissing = _weaponInfoSO.ClipSize - _bulletsOnClip;
        if (bulletsMissing > 0 && _reserveAmmo >= bulletsMissing)
        {
            _reserveAmmo -= bulletsMissing;
            _bulletsOnClip += bulletsMissing;
        }
        else if (bulletsMissing > 0 && _reserveAmmo < bulletsMissing)
        {
            _bulletsOnClip += _reserveAmmo;
            _reserveAmmo = 0;
        }
    }

    public void MoreAmmo(int newBullets)
    {
        _reserveAmmo += newBullets;
    }

    public void PickupWeapon(bool wasPickedUp)
    {
        GetComponent<Rigidbody>().isKinematic = wasPickedUp;
        GetComponent<BoxCollider>().enabled = !wasPickedUp;
        GetComponent<SphereCollider>().enabled = !wasPickedUp;

        
    }

    public string GetObjectName()
    {
        return _weaponInfoSO.WeaponName;
    }
}

public enum WeaponState
{
    OnCooldown,
    Ready,
    Reloading
}

