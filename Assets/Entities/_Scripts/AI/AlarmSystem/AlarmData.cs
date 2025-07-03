using UnityEngine;

namespace Automata
{
    [Tooltip("Dato a enviar a receptores de alarma, si la transform es distinta de null se encontró objetivo, si position es distinto de null solo se consiguio esa posicion")]
    public struct AlarmData
    {
        public enum State { Searching, Lost, Found }
        public State state;
        public Vector3? targetPosition;
        public Transform targetTransform;
    }
}