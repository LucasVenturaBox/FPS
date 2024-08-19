using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClip : MonoBehaviour, IInteractable
{
    [SerializeField] private int _amountOfBullets;

    public int AmountOfBullets { get{ return _amountOfBullets; }}

    public string GetObjectName()
    {
        return "Bullet Clip";
    }
}
