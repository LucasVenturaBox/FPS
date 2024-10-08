using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunInfoSO", menuName = "GunInfoSO", order = 0)]
public class WeaponInfoSO : ScriptableObject 
{
    [SerializeField] private Mesh _weaponMesh;
    [SerializeField] private Material[] _weaponMaterials;

    [SerializeField] private string _weaponName;

    [Header("Gun Stats")]
    [SerializeField] private float _fireRate;
    [SerializeField] private float _maxRange;
    [SerializeField] private float _accuracy;
    [SerializeField] private int _bulletsPerShot;
    [SerializeField] private int _clipSize;
    [SerializeField] private float _reloadTime;

    //Getters
    public Mesh WeaponMesh => _weaponMesh;
    public Material[] WeaponMaterials => _weaponMaterials;

    public string WeaponName => _weaponName;

    //Stats
    public float FireRate => _fireRate;
    public float MaxRange => _maxRange;
    public float Accuracy => _accuracy;
    public int BulletsPerShot => _bulletsPerShot;
    public int ClipSize => _clipSize;
    public float ReloadTime => _reloadTime;
}

