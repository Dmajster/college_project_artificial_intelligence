using System.Linq;
using UnityEngine;

namespace Assets
{
    public class Map
    {
        public int Width;
        public int Height;

        public int Size => Width * Height;
        public int StartNode => Data.ToList().FindIndex(node => node == -2);
        public int[] EndNodes => Data
            .Select((value, index) => value == -3 ? index : -1)
            .Where(index => index != -1).ToArray();



        public int[] Data;
        public GameObject[] Tiles;

        public static Map Load(string path)
        {
            var text = ((TextAsset)Resources.Load(path)).text;
            var rows = text.Split('\n');
            var height = rows.Length - 1;
            var width = rows[0].Split(',').Length;

            var data = new int[width * height];
            var tiles = new GameObject[width * height];
            for (var row = 0; row < rows.Length; row++)
            {
                var rowData = rows[row].Split(',');
                for (var column = 0; column < rowData.Length; column++)
                {
                    int parsedRowData;
                    if (int.TryParse(rowData[column], out parsedRowData))
                    {
                        data[row * width + column] = parsedRowData;
                    }
                }
            }

            return new Map()
            {
                Width = width,
                Height = height,
                Data = data,
                Tiles = tiles
            };
        }

        public int GetIndex(int x, int y)
        {
            return y * Width + x;
        }

        public Vector2Int GetCoordinates(int index)
        {
            return new Vector2Int(index % Width, index / Width);
        }

        public int Get(int index)
        {
            return Data[index];
        }

        public int Get(int x, int y)
        {
            return Data[y * Width + x];
        }

        private void Set(int x, int y, int neighborValue)
        {
            Data[y * Width + x] = neighborValue;
        }

        public static int[][] MakeAdjacencyMatrix(Map map)
        {
            var adjacencyMatrix = new int[map.Size][];

            for (var index = 0; index < adjacencyMatrix.Length; index++)
            {
                adjacencyMatrix[index] = new int[map.Size];

                if (map.Get(index) == -1)
                {
                    continue;
                }

                var coordinates = map.GetCoordinates(index);
                int neighborIndex;

                if (coordinates.x < map.Width - 1)
                {
                    neighborIndex = map.GetIndex(coordinates.x + 1, coordinates.y);

                    if (map.Get(neighborIndex) != -1)
                    {
                        adjacencyMatrix[index][neighborIndex] = 1;
                    }
                }


                if (coordinates.x > 1)
                {
                    neighborIndex = map.GetIndex(coordinates.x - 1, coordinates.y);
                    if (map.Get(neighborIndex) != -1)
                    {
                        adjacencyMatrix[index][neighborIndex] = 1;
                    }
                }

                if (coordinates.y < map.Height - 1)
                {
                    neighborIndex = map.GetIndex(coordinates.x, coordinates.y + 1);
                    if (map.Get(neighborIndex) != -1)
                    {
                        adjacencyMatrix[index][neighborIndex] = 1;
                    }
                }

                if (coordinates.y > 1)
                {
                    neighborIndex = map.GetIndex(coordinates.x, coordinates.y - 1);
                    if (map.Get(neighborIndex) != -1)
                    {
                        adjacencyMatrix[index][neighborIndex] = 1;
                    }
                }
            }

            return adjacencyMatrix;
        }

        public static int[][] MakeDistanceMatrix(Map map)
        {
            var distanceMatrix = new int[map.Size][];

            for (var index = 0; index < distanceMatrix.Length; index++)
            {
                distanceMatrix[index] = new int[map.Size];

                if (map.Get(index) == -1)
                {
                    continue;
                }

                var coordinates = map.GetCoordinates(index);
                int neighborIndex;

                if (coordinates.x < map.Width - 1)
                {
                    neighborIndex = map.GetIndex(coordinates.x + 1, coordinates.y);

                    if (map.Get(neighborIndex) != -1)
                    {
                        var gScore = map.Get(neighborIndex);
                        distanceMatrix[index][neighborIndex] = gScore < 0 ? 1 : gScore;
                    }
                }


                if (coordinates.x > 1)
                {
                    neighborIndex = map.GetIndex(coordinates.x - 1, coordinates.y);
                    if (map.Get(neighborIndex) != -1)
                    {
                        var gScore = map.Get(neighborIndex);
                        distanceMatrix[index][neighborIndex] = gScore < 0 ? 1 : gScore;
                    }
                }

                if (coordinates.y < map.Height - 1)
                {
                    neighborIndex = map.GetIndex(coordinates.x, coordinates.y + 1);
                    if (map.Get(neighborIndex) != -1)
                    {
                        var gScore = map.Get(neighborIndex);
                        distanceMatrix[index][neighborIndex] = gScore < 0 ? 1 : gScore;
                    }
                }

                if (coordinates.y > 1)
                {
                    neighborIndex = map.GetIndex(coordinates.x, coordinates.y - 1);
                    if (map.Get(neighborIndex) != -1)
                    {
                        var gScore = map.Get(neighborIndex);
                        distanceMatrix[index][neighborIndex] = gScore < 0 ? 1 : gScore;
                    }
                }
            }

            return distanceMatrix;
        }

        public int[] MakeHeuristicsArray()
        {
            var heuristicsArray = new int[Size];

            for (var index = 0; index < Data.Length; index++)
            {
                var coordinates = GetCoordinates(index);
                var distance = EndNodes.Min(endNodeIndex =>
                {
                    var endNodeCoordinates = GetCoordinates(endNodeIndex);

                    return Mathf.Abs(endNodeCoordinates.x - coordinates.x) +
                           Mathf.Abs(endNodeCoordinates.y - coordinates.y);
                });
                heuristicsArray[index] = distance;
            }

            return heuristicsArray;
        }

        public override string ToString()
        {
            var output = "";
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    output += Get(x, y);

                }
                output += "\n";
            }
            return output;
        }
    }
}
