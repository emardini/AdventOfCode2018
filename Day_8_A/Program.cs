using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_8_A
{
    class Program
    {
        static void Main(string[] args)
        {
            var entries = DataProvider.Input_8().Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x)).ToList();

            var root = GetTree(entries);

            var totalMetadata = root.Traverse().SelectMany(x => x.Metadata).Sum();

            Console.WriteLine(totalMetadata);
            Console.WriteLine(root.Value);

            Console.ReadKey();
        }

        private static SimpleNode GetTree(IEnumerable<int> entries)
        {
            var nbChildren = entries.FirstOrDefault();
            var nbMetadata = entries.Skip(1).FirstOrDefault();
            var metadata = entries.Skip(2).Take(nbMetadata);

            var root = GetTree(nbChildren, nbMetadata, entries.Skip(2).ToList());

            return root;
        }

        private static SimpleNode GetTree(int nbRootChildren, int nbRootMetadata, List<int> entries)
        {
            if (nbRootChildren == 0)
            {
                return new SimpleNode(nbRootChildren, nbRootMetadata, null, entries);
            }

            var root = new SimpleNode(nbRootChildren, nbRootMetadata, null, null);
            for(int child = 0; child < nbRootChildren; child++)
            {
                var nbChildren = entries.FirstOrDefault();
                var nbMetadata = entries.Skip(1).FirstOrDefault();
                if(nbChildren == 0)
                {
                    root.Children.Add(new SimpleNode(nbChildren, nbMetadata, null, entries.Skip(2).Take(nbMetadata)));
                    entries.RemoveRange(0, 2 + nbMetadata);
                    continue;
                }

                entries.RemoveRange(0, 2);
                var childrenNode = GetTree(nbChildren, nbMetadata, entries);
                root.Children.Add(childrenNode);
            }
            root.Metadata.AddRange(entries.Take(nbRootMetadata));
            entries.RemoveRange(0, nbRootMetadata);
            return root;
        }
    }

    public class SimpleNode
    {

        public SimpleNode(int nbChildren, int nbMetadata, IEnumerable<SimpleNode> children = null, IEnumerable<int> metadata = null)
        {
            this.NbMetadata = nbMetadata;
            this.NbChilren = nbChildren;
            this.metadata = metadata == null ? new List<int>() : metadata.ToList();
            this.children = children == null ? new List<SimpleNode>() : children.ToList();
        }
        
        public int NbChilren { get; }

        public int NbMetadata { get;  }

        private readonly List<SimpleNode> children = new List<SimpleNode>();
        public List<SimpleNode> Children { get { return children; } }


        private readonly List<int> metadata =  new List<int>();
        public List<int> Metadata { get { return metadata; } }

        public IEnumerable<SimpleNode> Traverse()
        {
            yield return this;

            foreach(var children in this.children.Select(x=> x.Traverse()))
            {
                foreach(var child in children)
                {
                    yield return child;
                }
            }
        }

        public int Value
        {
            get
            {
                if(!children.Any())
                {
                    return metadata.Sum();
                }

                var indexedChildren = children.Select((c, i) => new { child = c, index = i + 1}).ToList();
                return ( from c in indexedChildren
                         join m in metadata on c.index equals m
                         select c.child.Value).Sum();
            }
        }
    }
}
