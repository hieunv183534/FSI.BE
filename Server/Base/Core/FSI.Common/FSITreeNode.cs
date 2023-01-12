using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ICTINTree<TKey>
    {
        TKey TreeId { get; set; }
        string TreePath { get; set; }
        int CurentLevel { get; set; }
    }
    public interface FSITreeNode<TKey> : ICTINTree<TKey>
    {
        string TreeName { get; set; }
    }
    public interface FSITreeData<TKey> : ICTINTree<TKey>
    {
    }
}
