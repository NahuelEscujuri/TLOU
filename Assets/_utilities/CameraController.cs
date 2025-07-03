using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] CinemachineFreeLook defautCam;
    [SerializeField] CinemachineTargetGroup targetGroup;
    [SerializeField] float extraScale = 1f;

    public void CameraShake(Vector3 velocity, float duration)
    {
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = duration;
        impulseSource.m_DefaultVelocity = velocity;
        impulseSource.GenerateImpulse();
    }

    public void ChangeTargets(Transform targets)
    {
        targetGroup.m_Targets[0].target = targets;
    }

    #region Change Camera Methods
    public bool IsDefaultCamEnable()=> defautCam.Priority == 10;

    public void EnableCameraFight()
    {
        defautCam.Priority = 10;
    }

    public void DisableCameraFight()
    {
        // 1) Obtener el yaw actual de la cámara principal
        float currentYaw = transform.eulerAngles.y;
        // 2) Asignarlo al eje X del FreeLook
        defautCam.m_XAxis.Value = currentYaw;
        // 3) Marcar el estado previo como inválido para forzar que recalcule
        //defautCam.m_PreviousStateIsValid = false;

        defautCam.Priority = 20;
    }
    #endregion
}
