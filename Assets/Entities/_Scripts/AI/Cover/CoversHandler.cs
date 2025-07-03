using System.Collections.Generic;
using UnityEngine;

namespace Automata
{
    public class CoversHandler: MonoBehaviour, ICoverHandler
    {
        [SerializeField] float gridSize = 5f; // Tamaño de cada celda de la cuadrícula
        Dictionary<Vector2Int, List<CoverPoint>> coverGrid = new();

        private void Awake()
        {
            CoverPoint.OnEnableCover += AddCover;
            CoverPoint.OnDisableCover += RemoveCover;
        }
        private void OnDestroy()
        {
            CoverPoint.OnEnableCover -= AddCover;
            CoverPoint.OnDisableCover -= RemoveCover;
        }
        void AddCover(CoverPoint point)
        {
            Vector2Int cell = GetCellFromPosition(point.transform.position);
            if (!coverGrid.ContainsKey(cell))
                coverGrid[cell] = new();
            coverGrid[cell].Add(point);
        }
        void RemoveCover(CoverPoint point)
        {
            Vector2Int cell = GetCellFromPosition(point.transform.position);
            if (coverGrid.ContainsKey(cell))
                coverGrid[cell].Remove(point);
        }
        Vector2Int GetCellFromPosition(Vector3 pos)
        {
            int x = Mathf.FloorToInt(pos.x / gridSize);
            int z = Mathf.FloorToInt(pos.z / gridSize);
            return new Vector2Int(x, z);
        }
        public List<CoverPoint> GetCoverPointsInArea(Vector3 position, float radius)
        {
            List<CoverPoint> results = new();
            float sqrRadius = radius * radius;

            Vector3 lowerBound = position - new Vector3(radius, 0, radius);
            Vector3 upperBound = position + new Vector3(radius, 0, radius);

            Vector2Int lowerCell = GetCellFromPosition(lowerBound);
            Vector2Int upperCell = GetCellFromPosition(upperBound);

            for (int x = lowerCell.x; x <= upperCell.x; x++)
            {
                for (int z = lowerCell.y; z <= upperCell.y; z++)
                {
                    Vector2Int cell = new(x, z);
                    if (coverGrid.TryGetValue(cell, out List<CoverPoint> cellPoints))
                    {
                        foreach (CoverPoint cp in cellPoints)
                        {
                            if ((cp.transform.position - position).sqrMagnitude <= sqrRadius)
                            {
                                results.Add(cp);
                            }
                        }
                    }
                }
            }

            return results;
        }
    }    
}