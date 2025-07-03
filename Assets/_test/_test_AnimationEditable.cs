using UnityEngine;

public class _test_AnimationEditable: MonoBehaviour
{
    [SerializeField] Animation anim;
    private void Update()
    {
        foreach (AnimationState state in anim)
        {
            if (anim.IsPlaying(state.name))
            {
                Debug.Log("Animación actual: " + state.name + "Tiempo actual:" + anim[state.name].time);
            }
        }
    }

    public void LaunchDebug() => Debug.Log("Messi");
}
