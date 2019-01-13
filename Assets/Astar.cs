using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    public static class ExtensionMethods
    {
        public static LinkedListNode<T> RemoveAt<T>(this LinkedList<T> list, int index)
        {
            var currentNode = list.First;
            for (var i = 0; i <= index && currentNode != null; i++)
            {
                if (i != index)
                {
                    currentNode = currentNode.Next;
                    continue;
                }

                list.Remove(currentNode);
                return currentNode;
            }

            throw new IndexOutOfRangeException();
        }
    }

    public class Astar
    {
        public static SearchResult Search(int[][] graph, int startNode, List<int> endNodes, int[] hCost)
        {
            var open = new LinkedList<int>();
            var closed = new bool[graph.Length];
            var from = new int[graph.Length];

            var path = new List<int>();
            var searched = new List<int>();

            var gScore = new int[graph.Length];
            var fScore = new int[graph.Length];

            for (var i = 0; i < gScore.Length; i++)
            {
                gScore[i] = int.MaxValue;
                fScore[i] = int.MaxValue;
            }

            gScore[startNode] = 0;
            fScore[startNode] = hCost[startNode];
            from[startNode] = -1;

            open.AddFirst(startNode);

            while (open.Count > 0)
            {
                var minVal = int.MaxValue;
                var minPos = 0;
                var curNode = 0;

                for (var i = 0; i < open.Count; i++)
                {
                    var node = open.ElementAt(i);

                    if (fScore[node] < minVal)
                    {
                        minVal = fScore[node];
                        minPos = i;
                        curNode = node;
                        searched.Add(curNode);
                    }
                }

                open.RemoveAt(minPos);
                closed[curNode] = true;

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
                    if (graph[curNode][nextNode] > 0 && !closed[nextNode])
                    {
                        open.AddLast(nextNode);
                        var dist = gScore[curNode] + graph[curNode][nextNode];

                        if (dist < gScore[nextNode])
                        {
                            from[nextNode] = curNode;
                            gScore[nextNode] = dist;
                            fScore[nextNode] = gScore[nextNode] + hCost[nextNode];
                        }
                    }
                }
            }

            return new SearchResult();
        }
    }
}
