using System.Collections.Generic;
using UnityEngine;

namespace Automata
{
    public interface ICoverHandler
    {
        List<CoverPoint> GetCoverPointsInArea(Vector3 position, float radius);
    }
}