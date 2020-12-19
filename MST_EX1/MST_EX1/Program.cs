using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MST_EX1
{
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
            return (d - key).CompareTo(other.d - other.key);
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

        public Edge(Node from, Node to)
        {
            this.from = from;
            this.to = to;
            Random rndNum = new Random();
            weight = rndNum.Next(100);
        }

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
            return "from: " + (from.key + 1) + " to: " + (to.key + 1) + " weight: " + weight;
        }
    }

    class Graph
    {
        public ArrayList nodes;
        public Dictionary<Edge, int> edges = new Dictionary<Edge, int>();

        public Graph()
        {
            nodes = new ArrayList();
        }

        public Graph(Graph other)
        {
            this.nodes = other.nodes;
            this.edges = other.edges;
        }

        public void addNode(int key)
        {
            nodes.Add(new Node(key));
        }

        public void addEdge(int from, int to)
        {
            Node _from = (Node)nodes[from];
            Node _to = (Node)nodes[to];

            _from.adjacents.AddLast(_to);
            _to.adjacents.AddLast(_from);
            Edge edge = new Edge(_from, _to);
            edges.Add(edge, edge.weight);
            edge = new Edge(_to, _from);
            edges.Add(edge, edge.weight);
        }
        public void addEdge(int from, int to, int weight)
        {
            Node _from = (Node)nodes[from];
            Node _to = (Node)nodes[to];

            _from.adjacents.AddLast(_to);
            _to.adjacents.AddLast(_from);
            Edge edge = new Edge(_from, _to, weight);
            edges.Add(edge, edge.weight);
            edge = new Edge(_to, _from, weight);
            edges.Add(edge, edge.weight);
        }

        public bool edgeExist(int from, int to, out Edge edge)
        {
            Node _from = (Node)nodes[from];
            Node _to = (Node)nodes[to];

            foreach (Edge key in edges.Keys)
            {
                if (key.Equals(new Edge(_from, _to)))
                {
                    edge = key;
                    return true;
                }
            }
            edge = null;
            return false;
        }

        public int edgeWeight(int from, int to)
        {
            Node _from = (Node)nodes[from];
            Node _to = (Node)nodes[to];

            int weight;
            if (edges.TryGetValue(new Edge(_from, _to), out weight))
                return weight;
            return int.MinValue;

        }

        public int edgeWeight(Node from, Node to)
        {
            int weight;
            if (edges.TryGetValue(new Edge(from, to), out weight))
                return weight;
            return int.MinValue;

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
                    Edge edge;
                    if (edgeExist(i, j, out edge))
                        Console.Write("{0,4}|", edge.weight);
                    else if (edgeExist(j, i, out edge))
                        Console.Write("{0,4}|", edge.weight);
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
                    mstEdges.Add(new Edge(node.pi, node));
            }
        }

        public override void print()
        {
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
            SortedList<Node, Node> nodes = new SortedList<Node, Node>(); //  (G.nodes);

            foreach (Node node in G.nodes)
            {
                node.d = int.MaxValue;
                //node.pi = null;
                nodes.Add(node, node);
            }

            nodes.Values[0].d = 0;
            nodes.Values[0].pi = null;

            while (nodes.Count > 0)
            {
                Node u = nodes.Values[0];
                nodes.RemoveAt(0);
                foreach (Node v in u.adjacents)
                {
                    int w = G.edgeWeight(u, v);
                    if (nodes.ContainsKey(v) && w < v.d)
                    {
                        v.pi = u;
                        v.d = w;
                    }
                }
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
            const int numOfEdges = 50;
            Graph g = new Graph();

            //Create Graph
            for (int i = 0; i < numOfNodes; i++)
            {
                g.addNode(i);
            }
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
            g.addEdge(4, 10, 8);
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
            //for (int i = 0; i < numOfNodes; i++)
            //{
            //    for (int j = 0; j < 2; j++)
            //    {
            //        int to = -1;
            //        do
            //        {
            //            to = (new Random()).Next(numOfNodes);
            //        } while (i == to || g.edgeExist(i, to));
            //        g.addEdge(i, to);
            //    }
            //}
            //for (int i = 40; i < numOfEdges; i++)
            //{
            //    int from = -1, to = -1;
            //    do
            //    {
            //        from = (new Random()).Next(numOfNodes);
            //        to = (new Random()).Next(numOfNodes);
            //    } while (from == to || g.edgeExist(from, to));
            //    g.addEdge(from, to);
            //}

            Console.WriteLine("Graph:");
            g.print();

            Console.WriteLine("\nTree:");
            MST tree = Prim(g);
            tree.print();

            //// Insert new edge
            //Edge e = new Edge();

            //g.print();

            //reevaluate(tree, e);
            //tree.print();

            //// Insert new edge
            //e = new Edge();

            //g.print();

            //reevaluate(tree, e);
            //tree.print();


        }
    }
}
