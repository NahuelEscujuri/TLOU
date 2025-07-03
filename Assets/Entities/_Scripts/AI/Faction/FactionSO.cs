using UnityEngine;
using System;

namespace Automata
{
    [CreateAssetMenu(fileName = "Faction", menuName = "Data/FactionSO")]
    public class FactionSO: ScriptableObject
    {
        public FactionSymphaty[] factionsSymphaties;

        [Serializable]
        public struct FactionSymphaty
        {
            public FactionSO faction;
            public float sympathy;
        }
    }
}