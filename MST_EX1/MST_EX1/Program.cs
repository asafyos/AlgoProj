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
        public Node pie;

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

        public Graph()
        {
            nodes = new ArrayList();
        }

        public Graph(Graph other)
        {
            this.nodes = other.nodes;
        }

        public void addNode(int key)
        {
            nodes.Add(new Node(key));
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
        ArrayList edges = new ArrayList();

        public MST(Graph G) : base(G)
        {
            foreach (Node node in nodes)
            {
                if (node.pie != null)
                    edges.Add(new Edge(node.pie, node));
            }
        }

        public override void print()
        {
            foreach (Edge edge in edges)
            {
                Console.WriteLine(edge.ToString());
            }
        }
    }

    class Program
    {
        static Dictionary<Edge, int> edges = new Dictionary<Edge, int>();

        static int weight(Node from, Node to)
        {
            Edge edge = new Edge(from, to);
            if (edges.ContainsKey(edge))
            {
                return edges[edge];
            }
            return int.MinValue;
        }

        static MST Prim(Graph G, Func<Node, Node, int> W)
        {
            SortedList<Node, Node> nodes = new SortedList<Node, Node>(); //  (G.nodes);

            foreach (Node node in G.nodes)
            {
                node.d = int.MaxValue;
                //node.pie = null;

            }


            G.nodes.First.Value.d = 0;
            G.nodes.First.Value.pie = null;

            while (nodes.Count > 0)
            {
                Node u = nodes.Pop();
                foreach (Node v in u.adjacents)
                {
                    int w = W(u, v);
                    if (nodes.Contains(v) && w < v.d)
                    {
                        v.pie = u;
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
            Graph g = new Graph();

            //Create Graph
            for (int i = 1; i <= 20; i++)
            {
                g.addNode(i);
            }

            g.print();

            MST tree = Prim(g, weight);
            tree.print();

            // Insert new edge
            Edge e = new Edge();

            g.print();

            reevaluate(tree, e);
            tree.print();

            // Insert new edge
            e = new Edge();

            g.print();

            reevaluate(tree, e);
            tree.print();


        }
    }
}
