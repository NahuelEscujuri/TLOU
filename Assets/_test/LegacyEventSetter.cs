using UnityEngine;
using UnityEditor;  // SOLO si vas a ejecutar esto como Editor Script
using System.Collections.Generic;

public class LegacyEventSetter : MonoBehaviour
{
    // Asigna este script a un GameObject que tenga un Animation component
    // Con este botón de Editor crearás un evento en un clip legacy.
    [ContextMenu("Add Legacy Event")]
    void AddEvent()
    {
        var anim = GetComponent<Animation>();
        if (anim == null) { Debug.LogError("Necesitas un Animation component."); return; }

        // Asegúrate de que el clip está marcado como Legacy en el Import Settings
        AnimationClip clip = anim.clip;
        if (clip == null) { Debug.LogError("No hay clip asignado."); return; }

        // Limpia eventos previos (opcional)
        clip.events = new AnimationEvent[0];

        // Crea y configura el evento
        AnimationEvent evt = new AnimationEvent();
        evt.functionName = "OnLegacyKeyframe";  // nombre del método a invocar
        evt.time = 0.5f;                        // en segundos dentro de la duración del clip

        // Lo añade al clip
        var events = new List<AnimationEvent>(clip.events);
        events.Add(evt);
        clip.events = events.ToArray();

        Debug.Log($"Evento añadido a {clip.name} en t={evt.time}");
    }

    // Este método será llamado desde el AnimationEvent
    void OnLegacyKeyframe()
    {
        Debug.Log("¡Legacy Event disparado!");
    }
}
