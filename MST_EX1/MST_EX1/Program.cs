﻿// לירן חסן 315306761
// אסף יוסף 209018332
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MST_EX1
{
    class Integer
    {
        public int Value { get; set; }


        public Integer() { }
        public Integer(int value) { Value = value; }


        // Custom cast from "int":
        public static implicit operator Integer(Int32 x) { return new Integer(x); }

        // Custom cast to "int":
        public static implicit operator Int32(Integer x) { return x.Value; }


        public override string ToString()
        {
            return string.Format("{0}", Value);
        }
    }

    class MinHeap<T> where T : IComparable<T>
    {
        const int MIN_CAPACITY = 7;
        T[] data;

        public int Count { get; private set; }

        public int Capacity { get { return data.Length; } }

        public MinHeap()
        {
            data = new T[MIN_CAPACITY];
            Count = 0;
        }

        public MinHeap(T[] nodes)
        {
            int length = nodes.Length, count = 0;
            while (length > 0)
            {
                count++;
                length /= 2;
            }
            data = new T[Math.Max(MIN_CAPACITY, ((int)Math.Pow(2, count + 1) - 1))];
            Count = nodes.Length;
            for (int i = 0; i < nodes.Length; i++)
            {
                data[i] = nodes[i];
            }
            Heapify();
        }

        private void extand()
        {
            if (Count == Capacity)
            {
                T[] newData = new T[((1 + Capacity) * 2) - 1];
                for (int i = 0; i < data.Length; i++)
                {
                    newData[i] = data[i];
                }
                data = newData;
            }
        }
        private void collapse()
        {
            if (Capacity > MIN_CAPACITY && Count < (((1 + Capacity) / 2) - 1))
            {
                T[] newData = new T[((1 + Capacity) / 2) - 1];
                for (int i = 0; i < newData.Length; i++)
                {
                    newData[i] = data[i];
                }
                data = newData;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">index in one-base</param>
        private void shiftDown(int index)
        {
            if (index > Count / 2)
                return;
            int left = index * 2, right = (index * 2) + 1;
            if (data[index - 1].CompareTo(data[left - 1]) > 0 || (right < Count && data[index - 1].CompareTo(data[right - 1]) > 0))
            {
                int idx;
                if (right >= Count || data[left - 1].CompareTo(data[right - 1]) < 0)
                    idx = left;
                else
                    idx = right;

                T temp = data[idx - 1];
                data[idx - 1] = data[index - 1];
                data[index - 1] = temp;
                shiftDown(idx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">index in one-base</param>
        private void shiftUp(int index)
        {
            if (index <= 0)
                return;
            int father = index / 2;
            if (data[index - 1].CompareTo(data[father - 1]) > 0)
            {
                T temp = data[father - 1];
                data[father - 1] = data[index - 1];
                data[index - 1] = temp;
                shiftUp(father);
            }
        }

        public void Insert(T newNode)
        {
            data[Count] = newNode;
            shiftUp(Count);
            Count++;
            extand();
        }

        public T getMin()
        {
            T min = data[0];

            data[0] = data[Count - 1];
            data[Count - 1] = default(T);
            Count--;
            shiftDown(1);

            return min;
        }

        public T peek()
        {
            return data[0];
        }

        public void Heapify()
        {
            if (Count > 1)
                for (int i = Count / 2; i >= 1; i--)
                {
                    shiftDown(i);
                }
        }

        public bool Contains(T node)
        {
            foreach (T t in data)
            {
                if (t != null && t.Equals(node))
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            string str = "Heap: |";
            for (int i = 0; i < Count; i++)
            {
                str += string.Format("{0}|", data[i]);
            }

            return str;
        }
    }

    class Node : IComparable<Node>, ICloneable
    {
        /// <summary>
        /// The avilable colors of the node
        /// </summary>
        public enum COLOR
        {
            WHITE = 1, GRAY = 2, BLACK = 3
        }

        public int key;
        public int d;

        public LinkedList<Node> adjacents;
        public Node pi;

        public int dfsD;
        public int dfsF;
        public Node dfsPi;
        public COLOR color;

        /// <summary>
        /// Create a node
        /// </summary>
        /// <param name="key">the key of the node</param>
        public Node(int key)
        {
            this.key = key;
            this.adjacents = new LinkedList<Node>();
            dfs_init();
        }

        public int CompareTo(Node other)
        {
            return d.CompareTo(other.d);
        }

        /// <summary>
        /// Initialize the node for DFS
        /// dfsD = dfsF = 0, dfsPi = null, color = WHITE
        /// </summary>
        public void dfs_init()
        {
            dfsD = dfsF = 0;
            dfsPi = null;
            color = COLOR.WHITE;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node)
            {
                Node other = obj as Node;
                return (key == other.key);
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("{0,2}:({1,2},{2,2})", key + 1, d == int.MaxValue ? "" : d.ToString(), (pi != null ? pi.key.ToString() : ""));

            string output = (key + 1) + "";
            if (adjacents.Count != 0)
            {
                output += ": ";
                LinkedListNode<Node> curr = adjacents.First;
                while (curr != null)
                {
                    output += (curr.Value.key + 1);
                    if (curr.Next != null)
                        output += ", ";
                    curr = curr.Next;
                }
            }
            return output;
        }

        public object Clone()
        {
            Node newNode = new Node(this.key);
            newNode.d = d;
            newNode.pi = pi;
            newNode.dfsD = dfsD;
            newNode.dfsF = dfsF;
            newNode.dfsPi = dfsPi;
            newNode.color = color;
            return newNode;
        }
    }

    class Edge
    {
        public Node from;
        public Node to;
        public int weight;

        /// <summary>
        /// Create a new edge
        /// </summary>
        /// <param name="from">The start of the edge</param>
        /// <param name="to">The end of the edge</param>
        /// <param name="weight">The weight of the edge</param>
        public Edge(Node from, Node to, int weight)
        {
            this.from = from;
            this.to = to;
            this.weight = weight;
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge)
            {
                Edge other = obj as Edge;
                return (from.Equals(other.from) && to.Equals(other.to));
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("[{0,2},{1,2}]: weight {2}", (from.key + 1), (to.key + 1), weight);
        }
    }

    class Graph
    {
        public ArrayList nodes;
        public Integer[,] edges;
        private bool oriented;

        /// <summary>
        /// Set a new Graph
        /// </summary>
        /// <param name="oriented">true - the graph is oriented, 
        /// false (default) - the graph is not oriented</param>
        public Graph(bool oriented = false)
        {
            nodes = new ArrayList();
            this.oriented = oriented;
        }

        /// <summary>
        /// Shallow copy of the graph
        /// </summary>
        /// <param name="other">The copied graph</param>
        public Graph(Graph other)
        {
            this.nodes = other.nodes;
            this.edges = other.edges;
        }

        /// <summary>
        /// Adding a node to the graph
        /// </summary>
        /// <param name="key">The node key</param>
        public void addNode(int key)
        {
            nodes.Add(new Node(key));

            Integer[,] oldEdges = edges;
            edges = new Integer[nodes.Count, nodes.Count];
            int i, j;
            for (i = 0; i < edges.GetLength(0); i++)
            {
                for (j = 0; j < edges.GetLength(1); j++)
                {
                    if (oldEdges == null || i >= oldEdges.GetLength(0) || j >= oldEdges.GetLength(1))
                        edges[i, j] = null;
                    else
                        edges[i, j] = oldEdges[i, j];
                }
            }
        }

        /// <summary>
        /// Adding an edge to the graph
        /// </summary>
        /// <param name="from">The start node of the edge</param>
        /// <param name="to">The end node of the edge</param>
        /// <param name="weight">The weight of the edge</param>
        /// <returns>The added edge</returns>
        public Edge addEdge(int from, int to, int weight)
        {
            if (edges == null || from < 0 || from > nodes.Count || to < 0 || to > nodes.Count) return null;

            Node _from = (Node)nodes[from];
            Node _to = (Node)nodes[to];

            _from.adjacents.AddLast(_to);
            edges[from, to] = weight;

            if (!this.oriented)
            {
                _to.adjacents.AddLast(_from);
                edges[to, from] = weight;
            }

            return new Edge(_from, _to, weight);
        }

        /// <summary>
        /// Remove an edge for the graph
        /// </summary>
        /// <param name="edge">The edge to remove</param>
        public virtual void removeEdge(Edge edge)
        {
            edge.from.adjacents.Remove(edge.to);
            edge.to.adjacents.Remove(edge.from);
            edges[edge.from.key, edge.to.key] = edges[edge.to.key, edge.from.key] = null;
        }

        /// <summary>
        /// Check if an edge is exist
        /// </summary>
        /// <param name="from">The start of the edge</param>
        /// <param name="to">The end of the edge</param>
        /// <returns>true - the edge exist in the graph
        /// false - the edge doesn't exist in the graph</returns>
        public bool edgeExist(int from, int to)
        {
            return edges[from, to] != null;
        }

        /// <summary>
        /// Get the edge weight (Nullable)
        /// </summary>
        /// <param name="from">The start of the edge</param>
        /// <param name="to">The end of the edge</param>
        /// <returns>The edge weight</returns>
        public Integer edgeWeight(int from, int to)
        {
            return edges[from, to];
        }

        /// <summary>
        /// Get the edge weight (Nullable)
        /// </summary>
        /// <param name="from">The start node of the edge</param>
        /// <param name="to">The end node of the edge</param>
        /// <returns>The edge weight</returns>
        public Integer edgeWeight(Node from, Node to)
        {
            return edgeWeight(from.key, to.key);
        }

        /// <summary>
        /// Print the graph in adjacent matrix view
        /// </summary>
        /// <param name="graph"></param>
        public virtual void print(bool graph = false)
        {
            Console.Write("    |");
            for (int i = 0; i < nodes.Count; i++)
            {
                Console.Write("{0,4}|", (i + 1));
            }
            Console.WriteLine();
            for (int i = 0; i < nodes.Count; i++)
            {
                Console.Write("{0,4}|", (i + 1));
                for (int j = 0; j < 20; j++)
                {
                    if (edgeExist(i, j))
                        Console.Write("{0,4}|", edgeWeight(i, j));
                    else if (edgeExist(j, i))
                        Console.Write("{0,4}|", edgeWeight(i, j));
                    else
                        Console.Write("{0,4}|", " ");
                }
                Console.WriteLine();

            }
        }
    }

    class MST : Graph
    {
        public ArrayList mstEdges = new ArrayList();

        /// <summary>
        /// Building a MST based on Graph 
        /// The MST is in the mstEdges list
        /// </summary>
        /// <param name="G">A graph with d and pi values</param>
        public MST(Graph G) : base(G)
        {
            foreach (Node node in nodes)
            {
                if (node.pi != null)
                    mstEdges.Add(new Edge(node.pi, node, G.edgeWeight(node.pi, node)));
            }
        }

        /// <summary>
        /// Copy a MST the graph full edges, only with MST edges and nodes
        /// </summary>
        /// <param name="tree">The copied MST</param>
        public MST(MST tree) : base()
        {
            foreach (Node node in tree.nodes)
            {
                this.nodes.Add(node.Clone());
            }
            edges = new Integer[nodes.Count, nodes.Count];
            foreach (Edge edge in tree.mstEdges)
            {
                Edge e = addEdge(edge.from.key, edge.to.key, edge.weight);
                this.mstEdges.Add(e);
            }
        }

        /// <summary>
        /// Remove an edge from the MST
        /// </summary>
        /// <param name="edge">The edge in the MST to be removed</param>
        public override void removeEdge(Edge edge)
        {
            base.removeEdge(edge);
            foreach (Edge edg in mstEdges)
            {
                if (edg.Equals(edge))
                {
                    mstEdges.Remove(edg);
                    break;
                }
            }
        }

        /// <summary>
        /// Print the MST / Graph
        /// </summary>
        /// <param name="graph">If true, an adjacent matrix is being printed
        /// otherwise, the MST edges is printed</param>
        public override void print(bool graph = false)
        {
            if (graph)
                base.print(graph);
            else
            {
                foreach (Edge edge in mstEdges)
                {
                    Console.WriteLine(edge.ToString());
                }
            }
        }
    }

    class Program
    {

        static int dfsTime;

        static MST Prim(Graph G, Node s)
        {
            // Initialize all of the nodes
            foreach (Node node in G.nodes)
            {
                node.d = int.MaxValue;
                node.pi = null;
            }

            // Insert nodes to priority queue (based on d values)
            MinHeap<Node> newNodes = new MinHeap<Node>((Node[])G.nodes.ToArray(typeof(Node)));

            // Set the default first node as the start 
            s.d = 0;
            newNodes.Heapify();
            //newNodes.peek().d = 0;

            // For all nodes in queue
            while (newNodes.Count > 0)
            {
                // Getting the minimum d nodes
                Node u = newNodes.getMin();

                // For all adjacents nodes
                foreach (Node v in u.adjacents)
                {
                    // Getting the weight of the edge
                    int w = G.edgeWeight(u, v);

                    // If the node is in the queue, and its value greater then the weight , set the new d and pi
                    if (newNodes.Contains(v) && w < v.d)
                    {
                        v.pi = u;
                        v.d = w;
                    }
                }

                // Re-arange the queue
                newNodes.Heapify();
            }

            // Build the return MST (not part of the PRIM itself)
            // Making the edge list based on pi and d values
            MST tree = new MST(G);
            return tree;
        }

        static void DFS_VISIT(Node u)
        {
            // DFS-visit untile we find the circle made from the new edge

            u.color = Node.COLOR.GRAY;
            u.dfsD = ++dfsTime;
            foreach (Node v in u.adjacents)
            {
                // Our addition - if the start node had been found, we completed the circle
                //   (Not including re-referance from the start of the circle (when v is the start of circle, but also the finder of u)
                if (v.color == Node.COLOR.GRAY && v.dfsD == 1 && u.dfsPi != v)
                {
                    // The circle is complete, no need for more iterations
                    v.dfsPi = u;
                    return;
                }
                if (v.color == Node.COLOR.WHITE)
                {
                    v.dfsPi = u;
                    DFS_VISIT(v);
                }
            }
            u.color = Node.COLOR.BLACK;
            u.dfsF = ++dfsTime;
        }

        static MST reevaluate(MST tree, Edge newEdge)
        {
            dfsTime = 0;
            foreach (Node node in tree.nodes)
                node.dfs_init();

            // Calculate the circle in the MST with the new edge
            // the circle will be in the dfsPi member of the tree nodes
            DFS_VISIT(newEdge.from);

            // Get the edge with the max weight
            Node currNode = newEdge.from;
            Node prevNode = newEdge.from.dfsPi;
            int maxWeight = int.MinValue;
            Edge maxEdge = null;
            do
            {
                int currWeight = tree.edgeWeight(currNode, prevNode);
                if (currWeight > maxWeight)
                {
                    maxWeight = currWeight;
                    maxEdge = new Edge(currNode, prevNode, maxWeight);
                }

                currNode = currNode.dfsPi;
                prevNode = prevNode.dfsPi;
            }
            while (prevNode != newEdge.from);

            // Check if the new edgee is the max edge
            if (newEdge.Equals(maxEdge))
            {
                // Remove the new Edge
                tree.removeEdge(newEdge);
                return tree;
            }

            // Remove the old edge, and set the new edge
            tree.mstEdges.Add(newEdge);
            tree.removeEdge(maxEdge);

            return tree;
        }

        static void Main(string[] args)
        {
            const int numOfNodes = 20;
            Graph g = new Graph();

            //Create Graph
            // Add 20 nodes
            for (int i = 0; i < numOfNodes; i++)
            {
                g.addNode(i);
            }
            // Add 50 edges
            // from, to, weight
            g.addEdge(0, 1, 1);
            g.addEdge(0, 2, 4);
            g.addEdge(0, 3, 48);
            g.addEdge(0, 4, 6);
            g.addEdge(1, 2, 2);
            g.addEdge(1, 5, 38);
            g.addEdge(1, 6, 20);
            g.addEdge(2, 3, 3);
            g.addEdge(2, 4, 50);
            g.addEdge(2, 6, 21);// 10
            g.addEdge(2, 7, 22);
            g.addEdge(2, 8, 19);
            g.addEdge(3, 4, 5);
            g.addEdge(3, 8, 18);
            g.addEdge(3, 9, 8);
            g.addEdge(3, 10, 13);
            g.addEdge(4, 10, 7);
            g.addEdge(5, 6, 39);
            g.addEdge(5, 11, 41);
            g.addEdge(6, 7, 30); // 20
            g.addEdge(6, 8, 31);
            g.addEdge(6, 11, 40);
            g.addEdge(6, 12, 36);
            g.addEdge(7, 8, 28);
            g.addEdge(7, 11, 35);
            g.addEdge(7, 12, 37);
            g.addEdge(7, 13, 29);
            g.addEdge(8, 9, 17);
            g.addEdge(8, 13, 24);
            g.addEdge(8, 14, 23); // 30
            g.addEdge(9, 10, 11);
            g.addEdge(9, 14, 16);
            g.addEdge(9, 15, 12);
            g.addEdge(9, 18, 14);
            g.addEdge(10, 15, 9);
            g.addEdge(11, 12, 42);
            g.addEdge(11, 16, 43);
            g.addEdge(12, 13, 32);
            g.addEdge(12, 16, 44);
            g.addEdge(12, 17, 34); // 40
            g.addEdge(13, 14, 26);
            g.addEdge(13, 17, 33);
            g.addEdge(13, 19, 27);
            g.addEdge(14, 15, 49);
            g.addEdge(14, 18, 15);
            g.addEdge(14, 19, 25);
            g.addEdge(15, 18, 10);
            g.addEdge(16, 17, 45);
            g.addEdge(17, 19, 46);
            g.addEdge(18, 19, 47);//50

            // Print the Graph
            Console.WriteLine("\nGraph:");
            g.print();

            // Calculate MST with Prim algorithem
            MST tree = Prim(g, (Node)g.nodes[0]);

            // Print the tree
            Console.WriteLine("\nTree after Prim:");
            tree.print();

            // Clone the tree for new edge
            tree = new MST(tree);

            // Insert new edge
            Edge e = tree.addEdge(2, 9, 30);

            // Print the new Edge
            Console.WriteLine("\nnew edge: " + e + " (non-changer)");

            //Calculate the new MST
            tree = reevaluate(tree, e);

            //Print the tree
            Console.WriteLine("\nTree after new edge (non-changer):");
            tree.print();

            // Insert new edge
            e = tree.addEdge(8, 19, 1);

            // Print the new edge
            Console.WriteLine("\nnew edge: " + e + " (changer)");

            //Calculate the new MST
            tree = reevaluate(tree, e);

            //Print the tree
            Console.WriteLine("\nTree after new edge (changer):");
            tree.print();

        }
    }
}