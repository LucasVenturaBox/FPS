using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float _bounceStrenght = 1;
    private float _time;
    private Vector3 _initialPos;

    private void Start() {
        _initialPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        _time+= Time.deltaTime;
        float bounce = Mathf.Sin(_time);

        transform.position = _initialPos + Vector3.up * bounce * _bounceStrenght;
    }
}
