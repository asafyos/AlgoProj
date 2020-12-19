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
            string output = key + "";
            if (adjacents.Count != 0)
            {
                output += ": ";
                LinkedListNode<Node> curr = adjacents.First;
                while (curr != null)
                {
                    output += curr.Value.key;
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
            weight = rndNum.Next(int.MaxValue / 4);
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
            return from.key + " -> " + to.key;
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
            Edge edge = new Edge(_from, _to);
            edges.Add(edge, edge.weight);
        }

        public bool edgeExist(int from, int to)
        {
            Node _from = (Node)nodes[from];
            Node _to = (Node)nodes[to];

            return edges.ContainsKey(new Edge(_from, _to));
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
            foreach(object node in nodes)
            {
                Console.WriteLine(node.ToString());
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

            for (int i = 0; i < numOfEdges; i++)
            {
                int from = -1, to = -1;
                do
                {
                    from = (new Random()).Next(numOfEdges);
                    to = (new Random()).Next(numOfEdges);
                } while (from != to && g.edgeExist(from, to));
                g.addEdge(from, to);
            }

            g.print();

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
