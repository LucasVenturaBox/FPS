using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _pickUpPromptText;
    [SerializeField] private TMPro.TextMeshProUGUI _bulletCounter;
    private string _initialText;

    private void OnEnable() {
        PlayerAttack.onInteractionTextPrompt += ShowInteractionText;
        PlayerAttack.onHideInteractionTextPrompt += HideInteractionText;
        PlayerAttack.onWeaponBulletCounterChange += UpdateBulletCounter;
    }

    private void OnDisable()
    {
        PlayerAttack.onInteractionTextPrompt -= ShowInteractionText;
        PlayerAttack.onHideInteractionTextPrompt -= HideInteractionText;
        PlayerAttack.onWeaponBulletCounterChange += UpdateBulletCounter;



    }

    private void Start()
    {
        _initialText = _pickUpPromptText.text;
    }

    private void ShowInteractionText(string InteractableName)
    {
        _pickUpPromptText.text = _initialText + " ["+ InteractableName + "]"; 

        if(!_pickUpPromptText.gameObject.activeInHierarchy)
        {
            _pickUpPromptText.gameObject.SetActive(true);
        }
    }

    private void HideInteractionText()
    {
        if(_pickUpPromptText.gameObject.activeInHierarchy)
        {
            _pickUpPromptText.gameObject.SetActive(false);
        }
    }

    private void UpdateBulletCounter(int bulletsOnClip, int bulletsOnReserve)
    {
        _bulletCounter.text = bulletsOnClip.ToString() + "/" + bulletsOnReserve.ToString();
    }
}
