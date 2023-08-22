using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SnakePath
{
    [SerializeField]
    private float _maxDistanceBetweenNodes = 1f;
    
    [SerializeField]
    public int SnakeLength { get; set; }

    private List<Vector2> _headPositions = new List<Vector2>();
    private Dictionary<int,Vector2> _nodePositions = new Dictionary<int, Vector2>();

    public Vector2 this[int index]
    {
        get
        {
            if (_nodePositions.ContainsKey(index))
            {
                return _nodePositions[index];
            }
            
            return _headPositions[0];
        }
    }

    public void AddHeadPosition(Vector2 pos)
    {
        _headPositions.Insert(0,pos);
        UpdateNodePositions();
    }

    public void ClearOldPositions(int lastUsedIndex)
    {
        for (var i = _headPositions.Count - 1; i > lastUsedIndex; i--)
        {
            _headPositions.RemoveAt(i);
        }
        
        Debug.Log("head Positions count: " + _headPositions.Count); 
    }

    public void UpdateNodePositions()
    {
        if (_headPositions == null || _headPositions.Count == 0) return;
        int LastUpdatedNode = 0;
        int LastUsedHeadIndex = 0;
        float distanceAddUp = 0;
        Vector2 LatestNodePos = _headPositions[0];
        
        Vector2 VectorToLatestNode(Vector2 currentPos) =>  (currentPos - LatestNodePos);
        
        for (int i = 0; i < _headPositions.Count; i++)
        {
            if (i == 0)
            {
                _nodePositions[0] = _headPositions[0];
                continue;
            }

            LastUsedHeadIndex = i;
            if (LastUpdatedNode >= SnakeLength) break;

            distanceAddUp += (_headPositions[i]-_headPositions[i-1]).magnitude;
            if (!(distanceAddUp >= _maxDistanceBetweenNodes)) continue;
            
            var vectorToLatestNode = VectorToLatestNode(_headPositions[i]);
            var clamped = Vector2.ClampMagnitude(vectorToLatestNode,_maxDistanceBetweenNodes);
            var clampedPos = LatestNodePos + clamped;
            _nodePositions[++LastUpdatedNode] = clampedPos;
            LatestNodePos = clampedPos;
            distanceAddUp -= _maxDistanceBetweenNodes;
            //distanceAddUp = 0;

        }
        
        ClearOldPositions(LastUsedHeadIndex);
    }
}