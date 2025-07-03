using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Automata
{
    public class SimpleFaction : MonoBehaviour, IFaction
    {
        public FactionSO FactionSO => factionSO;
        [SerializeField] FactionSO factionSO;
        Dictionary<FactionSO, float> factionSOToSympathy;
        private void Awake() => factionSOToSympathy = factionSO.factionsSymphaties.ToDictionary(fso => fso.faction, fso => fso.sympathy);
        public bool IsSameFaction(IFaction otherFaction) => otherFaction is SimpleFaction simpleFaction && simpleFaction.FactionSO == FactionSO;
        public float ObtainSympathy(IFaction otherFaction)
            => (otherFaction is SimpleFaction simpleFaction && factionSOToSympathy.TryGetValue(simpleFaction.FactionSO, out var symphaty)) ? symphaty : 0;
    }    
}