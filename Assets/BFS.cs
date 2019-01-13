using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class Bfs
    {
        public static SearchResult Search(int[][] graph, int startNode,List<int> endNodes)
        {
            var path = new List<int>();
            var searched = new List<int>();

            var marked = new bool[graph.Length];
            var from = new int[graph.Length];

            var queue = new LinkedList<int>();

            marked[startNode] = true;
            from[startNode] = -1;

            queue.AddLast(startNode);

            while ( queue.Count > 0 )
            {
                var curNode = queue.First.Value;
                searched.Add(curNode);

                queue.RemoveFirst();

                if (endNodes.Any(endNode => endNode == curNode))
                {
                    while (true)
                    {
                        curNode = from[curNode];
                        if (curNode != -1)
                        {
                            path.Add(curNode);
                        }
                        else
                        {
                            break;
                        }
                    }

                    return new SearchResult()
                    {
                        Path = path.ToArray(),
                        Searched = searched.ToArray()
                    };
                }

                for (var nextNode = 0; nextNode < graph[curNode].Length; nextNode++)
                {
                    if (graph[curNode][nextNode] == 1 && !marked[nextNode])
                    {
                        marked[nextNode] = true;
                        from[nextNode] = curNode;
                        queue.AddLast(nextNode);
                    }
                }
            }

            return new SearchResult();
        }
    }
}
