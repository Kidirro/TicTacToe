using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using BinarySaveSystem = SaveSystem.BinarySaveSystem;

namespace Map
{
    public class MapGenerator : MonoBehaviour
    {
        private MapData _mapData;
        private BinarySaveSystem mapSaveSystem;

        private List<List<Node>> _nodePositionsIterations = new();

        private readonly Vector2 _startPosition = Vector2.zero;

        private const float MIN_DISTANCE_GENERATE = 5;
        private const float MAX_DISTANCE_GENERATE = 8;

        private const int MAX_NODE_ON_ITERATIONS = 5;

        private const float MAX_DISTANCE_ROAD = 10;

        private const float MIN_X_CLAMP = -15;
        private const float MAX_X_CLAMP = 15;

        private const float MAX_Y_CLAMP = 50;

        private GameObject _nodeParent;

        private void Awake()
        {
            mapSaveSystem = new BinarySaveSystem("MapData");
        }

        private void GenerateMap()
        {
            _nodePositionsIterations.Clear();
            _nodePositionsIterations.Add(new List<Node> {new(_startPosition)});

            _mapData = new MapData();

            float maxY = _nodePositionsIterations[0][0].Position.y;
            int ireration = 0;
            while (maxY + MAX_DISTANCE_GENERATE < MAX_Y_CLAMP)
            {
                ireration += 1;
                _nodePositionsIterations.Add(new List<Node>());
                for (int i = 0; i < Random.Range(1, MAX_NODE_ON_ITERATIONS); i++)
                {
                    if (_nodePositionsIterations[ireration - 1].Count == 0)
                        ireration -= 1;

                    int prevIndex = Mathf.Clamp(i, 0, _nodePositionsIterations[ireration - 1].Count - 1);
                    Node prevPosition = _nodePositionsIterations[ireration - 1][prevIndex];
                    if (TryGeneratePosition(prevPosition.Position, out var resultPosition))
                    {
                        Node newNode = new(resultPosition);

                        _nodePositionsIterations[ireration].Add(newNode);
                        _nodePositionsIterations = new CustomMap().nodePositionsIterations;
                        _mapData.Nodes.Add(newNode);
                        maxY = Mathf.Max(maxY, resultPosition.y);
                    }
                }
            }
        }


        private bool TryGeneratePosition(Vector2 prevPosition, out Vector2 resultPosition)
        {
            resultPosition = default;
            bool isFoundedPosition = false;

            for (int i = 0; i < 15; i++)
            {
                Vector2 randomSphere = Random.insideUnitSphere;
                randomSphere.y = Mathf.Abs(randomSphere.y);
                Vector2 dirtyRandomPosition = randomSphere * Random.Range(MIN_DISTANCE_GENERATE, MAX_DISTANCE_GENERATE);
                dirtyRandomPosition.x = Mathf.Clamp(dirtyRandomPosition.x, MIN_X_CLAMP, MAX_X_CLAMP);
                var resultDirtyPosition = dirtyRandomPosition + prevPosition;

                if (!_mapData.Nodes.Exists(x =>
                        (x.Position - resultDirtyPosition).magnitude < MIN_DISTANCE_GENERATE))
                {
                    isFoundedPosition = true;
                    resultPosition = resultDirtyPosition;
                    break;
                }
            }

            return isFoundedPosition;
        }

        private void GenerateNodeObjects()
        {
            if (_nodeParent != null)
                Destroy(_nodeParent);
            _nodeParent = new GameObject();
            foreach (var t in _nodePositionsIterations)
            {
                foreach (var t1 in t)
                {
                    GameObject node = new GameObject();
                    node.transform.SetParent(_nodeParent.transform);
                    node.transform.position = (Vector2)t1.Position;
                    node.name = "NODE";
                }
            }

            foreach (var r in _mapData.Roads)
            {
                GameObject road = new GameObject();
                road.transform.SetParent(_nodeParent.transform);
                var line = road.AddComponent<LineRenderer>();
                Vector3[] positions =  {(Vector2)r.From.Position, (Vector2)r.To.Position};
                line.SetPositions(positions);
                road.name = "ROAD";
            }
        }

        private void GenerateMapRoads()
        {
            _mapData.Roads = new List<Road>();
            for (int i = 0; i < _nodePositionsIterations.Count; i++)
            {
                for (int j = 0; j < _nodePositionsIterations[i].Count; j++)
                {
                    var currentPosition = _nodePositionsIterations[i][j];

                    //Проверка на некст итерацию
                    if (i != _nodePositionsIterations.Count - 1)
                    {
                        for (int k = 0; k < _nodePositionsIterations[i + 1].Count; k++)
                        {
                            if ((_nodePositionsIterations[i + 1][k].Position - (Vector2)currentPosition.Position).magnitude <=
                                MAX_DISTANCE_ROAD)
                            {
                                _mapData.Roads.Add(new Road(currentPosition, _nodePositionsIterations[i + 1][k]));
                            }
                        }
                    }

                    //Проверка на текущей итерации
                    if (j != _nodePositionsIterations[i].Count - 1)
                    {
                        for (int k = 0; k < _nodePositionsIterations[i].Count - j - 1; k++)
                        {
                            if (((Vector2)_nodePositionsIterations[i][k].Position - currentPosition.Position).magnitude <=
                                MAX_DISTANCE_ROAD)
                            {
                                _mapData.Roads.Add(new Road(currentPosition, _nodePositionsIterations[i][k]));
                            }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                GenerateMap();
                GenerateMapRoads();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadMapData();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveMapData();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                GenerateNodeObjects();
            }
        }

        private void LoadMapData()
        {
            _mapData = (MapData) mapSaveSystem.Load();
        }

        private void SaveMapData()
        {
            mapSaveSystem.Save(_mapData);
        }
    }


    [Serializable]
    public class Node
    {
        [SerializeField]
        public NodePosition Position;

        public Node(NodePosition position)
        {
            Position = position;
        }
    }

    [Serializable]
    public class NodePosition
    {
        public float x;
        public float y;

        public static implicit  operator NodePosition(Vector2 vector2)
        {
            return new NodePosition() {x = vector2.x, y = vector2.y};
        }  
        
        public static implicit  operator Vector2(NodePosition vector2)
        {
            return new Vector2() {x = vector2.x, y = vector2.y};
        }
    }

    [Serializable]
    public class Road
    {
        public Node From;
        public Node To;

        public Road(Node from, Node to)
        {
            From = from;
            To = to;
        }

        public Road(Vector2 from, Vector2 to)
        {
            From = new Node(from);
            To = new Node(to);
        }
    }
}