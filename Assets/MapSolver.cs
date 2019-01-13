using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public enum PathfindingAlgorithm
    {
        Dfs,
        Bfs,
        Astar,
    }

    public class MapSolver : MonoBehaviour
    {
        public GameObject Tile;

        public string FileName;
        
        public PathfindingAlgorithm Algorithm;
        public Map Map;

        [HideInInspector]
        public SearchResult SearchResult;

        private GameObject _tileContainer;

        private void Start()
        {
            _tileContainer = new GameObject("TileContainer");
        }

        public void Load()
        {
            SearchResult = new SearchResult();

            if (Map != null)
            {
                foreach (var tileGameObject in Map.Tiles)
                {
                    DestroyImmediate(tileGameObject);
                }
            }

            Map = Map.Load(FileName);
            var heuristics = Map.MakeHeuristicsArray();

            for (var index = 0; index < Map.Size; index++)
            {
                var tileType = Map.Get(index);
                var tilePosition = (Vector2)Map.GetCoordinates(index);
                tilePosition.y *= -1;

                var heuristic = heuristics[index];
                var tile = Instantiate(Tile, tilePosition, Quaternion.identity, _tileContainer.transform);
                Map.Tiles[index] = tile;

                switch (tileType)
                {
                    case -1:
                        tile.GetComponent<MeshRenderer>().material.color = Color.black;
                        tile.transform.position += new Vector3(0,0, -1);
                        break;
                    case -2:
                        tile.GetComponent<MeshRenderer>().material.color = Color.green;
                        tile.GetComponentInChildren<TextMesh>().text = $"I: {index.ToString()}\nC: {tileType.ToString()}\nD: {heuristic.ToString()}";
                        break;
                    case -3:
                        tile.GetComponent<MeshRenderer>().material.color = Color.red;
                        tile.GetComponentInChildren<TextMesh>().text = $"I: {index.ToString()}\nC: {tileType.ToString()}\nD: {heuristic.ToString()}";
                        break;
                    default:
                        tile.GetComponentInChildren<TextMesh>().text = $"I: {index.ToString()}\nC: {tileType.ToString()}\nD: {heuristic.ToString()}";
                        break;
                }
            }
        }

        public void MeasureTime()
        {
            var n = 1000;

            var runTime = Time.realtimeSinceStartup;
            var adjacencyMap = Map.MakeAdjacencyMatrix(Map);
            var distanceMap = Map.MakeAdjacencyMatrix(Map);
            var heuristicsMap = Map.MakeHeuristicsArray();

            for (var i = 0; i < n; i++)
            {
                switch (Algorithm)
                {
                    case PathfindingAlgorithm.Dfs:
                        SearchResult = Dfs.Search(adjacencyMap, Map.StartNode, Map.EndNodes.ToList());
                        break;
                    case PathfindingAlgorithm.Bfs:
                        SearchResult = Bfs.Search(adjacencyMap, Map.StartNode, Map.EndNodes.ToList());
                        break;
                    case PathfindingAlgorithm.Astar:
                        SearchResult = Astar.Search(distanceMap, Map.StartNode, Map.EndNodes.ToList(), heuristicsMap);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            runTime = Time.realtimeSinceStartup - runTime;

            Debug.Log(runTime/n);
        }

        public void Solve()
        {
            foreach (var tileIndex in SearchResult.Searched)
            {
                if (Map.Get(tileIndex) > 0)
                {
                    Map.Tiles[tileIndex].GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }

            for (var index = 0; index < SearchResult.Path.Length -1; index++)
            {
                var tileIndex = SearchResult.Path[index];
                Map.Tiles[tileIndex].GetComponent<MeshRenderer>().material.color = Color.white;
            }

            var adjacencyMap = Map.MakeAdjacencyMatrix(Map);
            var distanceMap = Map.MakeDistanceMatrix(Map);
            var heuristicsMap = Map.MakeHeuristicsArray();

            switch (Algorithm)
            {
                case PathfindingAlgorithm.Dfs:
                    SearchResult = Dfs.Search(adjacencyMap, Map.StartNode, Map.EndNodes.ToList());
                    break;
                case PathfindingAlgorithm.Bfs:
                    SearchResult = Bfs.Search(adjacencyMap, Map.StartNode, Map.EndNodes.ToList());
                    break;
                case PathfindingAlgorithm.Astar:
                    SearchResult = Astar.Search(distanceMap, Map.StartNode, Map.EndNodes.ToList(), heuristicsMap);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var tileIndex in SearchResult.Searched)
            {
                if (Map.Get(tileIndex) > 0)
                {
                    Map.Tiles[tileIndex].GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }

            if (Algorithm == PathfindingAlgorithm.Astar)
            {
                var gScore = 0;

                for (var index = 0; index < SearchResult.Path.Length - 2; index++)
                {
                    var tileIndex = SearchResult.Path[index];
                    var nextTileIndex = SearchResult.Path[index + 1];

                    gScore += distanceMap[tileIndex][nextTileIndex];
                }

                Debug.Log(gScore);
            }

            for (var index = 0; index < SearchResult.Path.Length - 1; index++)
            {
                var tileIndex = SearchResult.Path[index];
                Map.Tiles[tileIndex].GetComponent<MeshRenderer>().material.color = Color.magenta;
            }
        }

        public IEnumerator SolveSlowCoroutine()
        {
            foreach (var tileIndex in SearchResult.Searched)
            {
                if (Map.Get(tileIndex) > 0)
                {
                    Map.Tiles[tileIndex].GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }

            for (var index = 0; index < SearchResult.Path.Length - 1; index++)
            {
                var tileIndex = SearchResult.Path[index];
                Map.Tiles[tileIndex].GetComponent<MeshRenderer>().material.color = Color.white;
            }

            var adjacencyMap = Map.MakeAdjacencyMatrix(Map);
            var distanceMap = Map.MakeDistanceMatrix(Map);
            var heuristicsMap = Map.MakeHeuristicsArray();

            switch (Algorithm)
            {
                case PathfindingAlgorithm.Dfs:
                    SearchResult = Dfs.Search(adjacencyMap, Map.StartNode, Map.EndNodes.ToList());
                    break;
                case PathfindingAlgorithm.Bfs:
                    SearchResult = Bfs.Search(adjacencyMap, Map.StartNode, Map.EndNodes.ToList());
                    break;
                case PathfindingAlgorithm.Astar:
                    SearchResult = Astar.Search(distanceMap, Map.StartNode, Map.EndNodes.ToList(), heuristicsMap);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var tileIndex in SearchResult.Searched)
            {
                if (Map.Get(tileIndex) > 0)
                {
                    Map.Tiles[tileIndex].GetComponent<MeshRenderer>().material.color = Color.blue;
                    yield return new WaitForSeconds(0.01f);
                }
            }

            if (Algorithm == PathfindingAlgorithm.Astar)
            {
                var gScore = 0;

                for (var index = 0; index < SearchResult.Path.Length - 2; index++)
                {
                    var tileIndex = SearchResult.Path[index];
                    var nextTileIndex = SearchResult.Path[index + 1];

                    gScore += distanceMap[tileIndex][nextTileIndex];
                }

                Debug.Log(gScore);
            }

            for (var index = 0; index < SearchResult.Path.Length - 1; index++)
            {
                var tileIndex = SearchResult.Path[index];
                Map.Tiles[tileIndex].GetComponent<MeshRenderer>().material.color = Color.magenta;
            }
        }


        public void SolveSlow()
        {
            StartCoroutine(SolveSlowCoroutine());
        }
    }
}
