using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private GunInfoSO _gunInfoSO;
    private event Action onCooldownFinished;
    private event Action onReloadFinished;
    private WeaponState _state;
    private Coroutine _cooldown, _reload;

    private int _bulletsOnClip;

    public GunInfoSO GetGunInfo => _gunInfoSO;

    private void OnEnable() {
        onCooldownFinished += WeaponReady;

        onReloadFinished += WeaponReady;
        onReloadFinished += FullClip;
    }

    private void OnDisable() {
        onCooldownFinished -= WeaponReady;

        onReloadFinished -= WeaponReady;
        onReloadFinished -= FullClip;

    }

    public void Start()
    {
        Debug.Log("Gun Started");
        StopAllCoroutines();

        _state = WeaponState.Ready;
        _bulletsOnClip = _gunInfoSO.ClipSize;
    }

    public void Shoot(GameObject bullet, Transform origin, Vector3 destination)
    {
        Debug.Log("Gun tried to shoot");
        Debug.Log("Weapon state is -" + _state + "-, and _bullets are -" + _bulletsOnClip + "-");
        if(_state == WeaponState.Ready && _bulletsOnClip > 0)
        {
            Debug.Log("Gun can shoot");

            if (bullet != null)
            {
                GameObject newBullet = Instantiate(bullet, origin.position, origin.rotation);

                Vector3 finalDestination;
                finalDestination.x = UnityEngine.Random.Range(destination.x - _gunInfoSO.Accuracy, destination.x + _gunInfoSO.Accuracy);
                finalDestination.y = UnityEngine.Random.Range(destination.y - _gunInfoSO.Accuracy, destination.y + _gunInfoSO.Accuracy);
                finalDestination.z = UnityEngine.Random.Range(destination.z - _gunInfoSO.Accuracy, destination.z + _gunInfoSO.Accuracy);

                newBullet.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(finalDestination) * _gunInfoSO.MaxRange, ForceMode.Impulse);

                _bulletsOnClip--;

                _state = WeaponState.OnCooldown;

                _cooldown = StartCoroutine(Utils.DelaySeconds(_gunInfoSO.FireRate, onCooldownFinished));
                Debug.Log("Gun has just shot");

            }
        }
        else if(_state == WeaponState.Ready && _bulletsOnClip <= 0) 
        {
            Reload();
        }

    }

    public void Reload()
    {
        if(_bulletsOnClip < _gunInfoSO.ClipSize)
        {
            _state = WeaponState.Reloading;

            if(_cooldown != null) StopCoroutine(_cooldown);

            _reload = StartCoroutine(Utils.DelaySeconds(_gunInfoSO.ReloadTime, onReloadFinished));
        }
    }

    private void WeaponReady()
    {
        _state = WeaponState.Ready;
    }

    private void FullClip()
    {
        _bulletsOnClip = _gunInfoSO.ClipSize;
    }
}

public enum WeaponState
{
    OnCooldown,
    Ready,
    Reloading
}

