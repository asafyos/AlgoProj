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

    class Node : IComparable<Node>
    {
        public int key;
        public int d;

        public LinkedList<Node> adjacents;
        public Node pi;

        public Node(int key)
        {
            this.key = key;
            this.adjacents = new LinkedList<Node>();
        }

        public int CompareTo(Node other)
        {
            return d.CompareTo(other.d);
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
    }

    class Edge
    {
        public Node from;
        public Node to;
        public int weight;

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
            return string.Format("from: {0,2} to: {1,2}, weight {2}", (from.key + 1), (to.key + 1), weight);
        }
    }

    class Graph
    {
        public ArrayList nodes;
        //public Dictionary<Edge, int> edges = new Dictionary<Edge, int>();
        public Integer[,] newEdges = new Integer[20, 20];

        public Graph()
        {
            nodes = new ArrayList();
            for (int i = 0; i < newEdges.GetLength(0); i++)
            {
                for (int j = 0; j < newEdges.GetLength(1); j++)
                {
                    newEdges[i, j] = null;
                }
            }
        }

        public Graph(Graph other)
        {
            this.nodes = other.nodes;
            this.newEdges = other.newEdges;
        }

        public void addNode(int key)
        {
            nodes.Add(new Node(key));
        }

        public Edge addEdge(int from, int to, int weight)
        {
            Node _from = (Node)nodes[from];
            Node _to = (Node)nodes[to];

            _from.adjacents.AddLast(_to);
            _to.adjacents.AddLast(_from);

            newEdges[from, to] = weight;
            newEdges[to, from] = weight;

            return new Edge(_from, _to, weight);
        }

        public bool edgeExist(int from, int to)
        {
            return newEdges[from, to] != null;
        }

        public Integer edgeWeight(int from, int to)
        {
            return newEdges[from, to];
        }

        public Integer edgeWeight(Node from, Node to)
        {
            return edgeWeight(from.key, to.key);
        }

        public virtual void print()
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
        ArrayList mstEdges = new ArrayList();

        public MST(Graph G) : base(G)
        {
            foreach (Node node in nodes)
            {
                if (node.pi != null)
                    mstEdges.Add(new Edge(node.pi, node, G.edgeWeight(node.pi, node)));
            }
        }

        public override void print()
        {
            if (false)
            {
                Console.WriteLine("Nodes:");
                foreach (Node node in nodes)
                {
                    //Console.WriteLine("node: " + node.key + " d: " + node.d + " pi: " + (node.pi != null ? node.pi.key : -1));
                    Console.WriteLine("node: {0}", node);
                }
                Console.WriteLine("\nEdges:");
            }
            foreach (Edge edge in mstEdges)
            {
                Console.WriteLine(edge.ToString());
            }
        }
    }

    class Program
    {

        static MST Prim(Graph G)
        {
            foreach (Node node in G.nodes)
            {
                node.d = int.MaxValue;
                node.pi = null;
            }

            MinHeap<Node> newNodes = new MinHeap<Node>((Node[])G.nodes.ToArray(typeof(Node)));

            newNodes.peek().d = 0;

            while (newNodes.Count > 0)
            {
                Node u = newNodes.getMin();
                foreach (Node v in u.adjacents)
                {
                    int w = G.edgeWeight(u, v);
                    if (newNodes.Contains(v) && w < v.d)
                    {
                        v.pi = u;
                        v.d = w;
                    }
                }
                newNodes.Heapify();
            }

            MST tree = new MST(G);
            return tree;
        }

        static void reevaluate(MST tree, Edge newEdge)
        {

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
            MST tree = Prim(g);

            // Print the tree
            Console.WriteLine("\nTree:");
            tree.print();

            // Insert new edge
            Edge e = g.addEdge(2, 9, 30);

            //Print the Graph
            Console.WriteLine("\nGraph after new edge (non-changer):");
            g.print();

            //Calculate the new MST
            reevaluate(tree, e);

            //Print the tree
            Console.WriteLine("\nTree after new edge (non-changer):");
            tree.print();

            // Insert new edge
            e = g.addEdge(8, 19, 1);

            //Print the Graph
            Console.WriteLine("\nGraph after new edge (changer):");
            g.print();

            //Calculate the new MST
            reevaluate(tree, e);

            //Print the tree
            Console.WriteLine("\nTree after new edge (changer):");
            tree.print();

        }
    }
}
