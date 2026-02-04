using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Projectile : MonoBehaviour 
{ 
    [SerializeField][Range(5000f, 25000f)] 
    float _launchForce = 10000f; 
    [SerializeField][Range(10, 1000)] 
    int _damage = 100; 
    [SerializeField][Range(2f, 10f)] 
    float _range = 5f; 
    Rigidbody _rigidbody; 
    float _duration; 
    bool OutofFuel 
    { 
        get 
        {
            _duration -= Time.deltaTime; 
            return _duration <= 0f; 
        } 
    } 
    void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>(); 
    } 
    void OnEnable() 
    { 
        _rigidbody.AddForce(transform.forward * _launchForce); _duration = _range;
    } 
    void Update()
    { 
        if (OutofFuel) Destroy(gameObject); 
    } 
    void OnCollisionEnter(Collision collision) 
    { 
        Debug.Log(message: $"Projectile collided with {collision.collider.name}"); 
    } 
}