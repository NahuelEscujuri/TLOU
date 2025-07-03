using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class ListenMode: MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] float range;
    [SerializeField] LayerMask layerMask;
    [Header("Debug")]
    [SerializeField] bool debug_Start;
    WaitForSeconds wait = new(.1f);
    List<ListenModeTarget> listenModeTargets = new();
    Coroutine detectionCor;
    Coroutine transition;

    private void Start()
    {
        if (debug_Start) Activate();
    }


    public void Activate()
    {

        if (detectionCor != null) StopCoroutine(detectionCor);

        detectionCor = StartCoroutine(Detection());
        StartTransition(1f);
    }

    public void Desactive()
    {
        if (detectionCor != null) StopCoroutine(detectionCor);
        listenModeTargets.ForEach(lmt => lmt.Hide());
        listenModeTargets.Clear();
        StartTransition(0f);
    }
    void StartTransition(float value)
    {
        if (transition != null) StopCoroutine(transition);
        transition = StartCoroutine(TransitionWeight(value));
    }
    IEnumerator TransitionWeight(float value)
    {
        while(volume.weight != value)
        {
            volume.weight = Mathf.MoveTowards(volume.weight, value, 2 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Detection()
    {
        while (true)
        {
            yield return wait;
            DetectEnemies();
        }
    }
    void DetectEnemies()
    {
        List<ListenModeTarget> previousTargets = new(listenModeTargets);
        listenModeTargets.Clear();

        Collider[] detectedTargetsColliders = Physics.OverlapSphere(transform.position, range, layerMask);

        foreach (Collider targetCollider in detectedTargetsColliders)
        {
            if (!targetCollider.TryGetComponent(out ListenModeTarget target)) continue;

            listenModeTargets.Add(target);

            if (previousTargets.Contains(target)) continue;

            target.Show();
        }

        foreach (var targetLost in previousTargets.Except(listenModeTargets))
        {
            targetLost.Hide();
        }
    }
}
