using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FSITree<TNode, TData, TKey> where TNode : FSITreeNode<TKey> where TData : FSITreeData<TKey>
    {
        public List<TData> LstData { get; private set; }
        public List<FSITree<TNode, TData, TKey>> Childs { get; private set; }
        public FSITree<TNode, TData, TKey> Parent { get; private set; }
        public bool IsLeaf { get; private set; }
        public bool HasParent { get; private set; }
        public bool HasChild { get; private set; }
        public TKey Id { get;private set; }
        public int CurentLevel { get; private set; }
        public FSITree()
        {
            LstData = new List<TData>();
            Childs = new List<FSITree<TNode, TData, TKey>>();
            Parent = null;
            IsLeaf = true;
            CurentLevel = 0;
        }
        public FSITree(List<TNode> lstNode, List<TData> lstData, int lengthNode)
        {
            LstData = new List<TData>();
            Childs = new List<FSITree<TNode, TData, TKey>>();
            Parent = null;
            IsLeaf = true;
            CurentLevel = 0;
            lstNode = lstNode.OrderBy(a => a.TreePath).ToList();
            
            foreach (var item in lstNode)
            {

            }
        }

        public void AddNode(List<TNode> lstNode)
        {
            lstNode = lstNode.OrderBy(a => a.TreePath).ToList();

        }

        public void AddData(List<TData> lstData)
        {

        }

        public FSITree<TNode, TData, TKey> GetNode(ICTINTree<TKey> tree)
        {
            if (tree == null)
                return null;
            if (tree.TreeId.Equals(this.Id))
                return this;
            if (this.IsLeaf && this.HasParent)
            {
                return this.Parent.GetNode(tree);
            }
            else if(this.HasChild)
            {
                foreach (var child in Childs)
                {
                    var node = child.GetNode(tree);
                    if (node != null)
                        return node;
                }
            }
            return null;
        }


    }
}
