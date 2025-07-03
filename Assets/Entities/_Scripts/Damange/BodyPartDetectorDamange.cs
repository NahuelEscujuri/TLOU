using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartDetectorDamange : MonoBehaviour
{
    public string tagDetector;
    public event Action<GameObject> DetectDamange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == tagDetector) DetectDamange?.Invoke(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == tagDetector) DetectDamange?.Invoke(gameObject);
    }
}
