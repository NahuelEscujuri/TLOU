using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyPlayer : MonoBehaviour
{
    Animation anim;
    [SerializeField] string locomotionName;
    [SerializeField] float timeCrossfade=.3f;
    [SerializeField] bool isCrouch;
    [SerializeField] AnimationClip clip;

    public AnimationClip upperBodyClip;   // tu animación de torso (Legacy)
    public Transform upperBodyRoot;

    void Awake()
    {
        anim = GetComponent<Animation>();

        if (clip != null)
        {
            // Le damos un nombre único (puede ser clip.name)
            string clipName = clip.name;

            // Registramos el clip en el componente Animation
            anim.AddClip(clip, clipName);

            // Opcional: lo dejamos como clip por defecto
            anim.clip = clip;
        }
    }

    public void PlayClip()
    {
        if (isCrouch) GetComponent<Animation>().CrossFade(locomotionName, timeCrossfade);
        else GetComponent<Animation>().CrossFade("locomotion", timeCrossfade);

        isCrouch = !isCrouch;

        //anim.CrossFade("T-Pose (1)", 0.1f);
        //anim.CrossFade("ellie_explore_idel_waiting", 0.3f);

        return;

        // 1) Asegúrate de que tu clip esté importado como "Legacy" en el Inspector.

        // 2) Añade la animación al componente Animation:
        anim.AddClip(upperBodyClip, "UpperBody");

        // 3) Enmascara para que sólo afecte al torso:
        var state = anim["UpperBody"];
        state.layer = 1;                      // capa superior a la de locomoción
        state.weight = 1f;                    // peso total
        state.AddMixingTransform(upperBodyRoot, true);

        // 4) Arranca la reproducción (puedes crossfade si quieres suavizar):
        anim.Play("UpperBody");

    }

    void Update()
    {
        if(Input.GetButtonDown("Jump")) PlayClip();
        
    }
}
