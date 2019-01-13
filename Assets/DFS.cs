using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    class Dfs
    {
        public static SearchResult Search(int[][] graph, int startNode, List<int> endNodes)
        {
            var marked = new bool[graph.Length];
            var from = new int[graph.Length];
            var stack = new Stack<int>();

            var path = new List<int>();
            var searched = new List<int>();

            from[startNode] = -1;
            marked[startNode] = true;
            stack.Push(startNode);

            while (stack.Count > 0)
            {
                var curNode = stack.Peek();
                searched.Add(curNode);

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
                
                var found = false;
                for (var nextNode = 0; nextNode < graph[curNode].Length; nextNode++)
                {
                    if (graph[curNode][nextNode] != 1 || marked[nextNode])
                    {
                        continue;
                    }

                    marked[nextNode] = true;
                    from[nextNode] = curNode;
                    stack.Push(nextNode);
                    found = true;
                    break;
                }

                if (found)
                {
                    continue;
                }

                stack.Pop();
            }

            return new SearchResult();
        }
    }
}
