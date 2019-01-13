using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CustomEditor(typeof(MapSolver))]
    public class MapInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var mapSolver = (MapSolver)target;

            if (GUILayout.Button("Load"))
            {
                mapSolver.Load();
            }

            if (GUILayout.Button("Solve"))
            {
                mapSolver.Solve();

                var output = mapSolver.SearchResult.Path.Reverse().Aggregate("Path: ", (current, path) => current + (mapSolver.Map.GetCoordinates(path) + " "));
                output += "\n Searched: " + mapSolver.SearchResult.Searched.Length;
                Debug.Log(output);
            }

            if (GUILayout.Button("Solve Slow"))
            {
                mapSolver.SolveSlow();

                var output = mapSolver.SearchResult.Path.Reverse().Aggregate("Path: ", (current, path) => current + (mapSolver.Map.GetCoordinates(path) + " "));
                output += "\n Searched: " + mapSolver.SearchResult.Searched.Length;
                Debug.Log(output);
            }

            if (GUILayout.Button("Measure Time"))
            {
                mapSolver.MeasureTime();
            }
        }
    }
}
