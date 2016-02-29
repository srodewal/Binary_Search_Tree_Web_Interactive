using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace BST
{
    public class WebBST
    {
        private WebNode root;
        Canvas mainSpace;
        int treeSize = 0;

        // constructor for canvas?
        public WebBST(Canvas mS)
        {
            mainSpace = mS;
            root = null;
            treeSize = 0;
        }

        public WebNode getRoot()
        {
            return root;
        }

        public int getTreeSize()
        {
            return treeSize;
        }
        public void incrementTreeSize()
        {
            treeSize += 1;
        }
        public void decrementTreeSize()
        {
            treeSize -= 1;
        }

      
        public bool add(int key)
        {
            if (root == null)
            {
                root = new WebNode(key, 25, 25, mainSpace, 1);
                root.setPositionInTree(1); // root = position 1
                return true;
            }
            else
            {
                return root.add(key, 1); // start at position 1 and work way down
            }
        }

        public bool remove(int key)
        {
            if (root == null)
            {
                return false;
            }
            else
            {
                if (root.getValue() == key) // if root to be deleted
                {
                    WebNode tempRoot = new WebNode(0, 25, 25, mainSpace, 1); // create dummy root
                    tempRoot.setLeftChild(root);
                    bool result = root.remove(key, tempRoot);
                    root = tempRoot.getLeftChild();
                    return result;
                }
                else
                {
                    return root.remove(key, null);
                }
            }
        }

        public WebNode TreeSearch(WebNode x, int key)
        {
            if (x == null || key == x.getValue())
            {
                return x;
            }
            if (key < x.getValue())
            {
                return TreeSearch(x.getLeftChild(), key);
            }
            else
            {
                return TreeSearch(x.getRightChild(), key);
            }
        }

        public void inOrderTraverse(WebNode x, TextBlock traversal)
        {
            if (x != null)
            {
                inOrderTraverse(x.getLeftChild(), traversal);
                traversal.Text += x.getValue() + " ";
                inOrderTraverse(x.getRightChild(), traversal);
            }
        }

        public void preOrderTraverse(WebNode x, TextBlock traversal)
        {
            if (x != null)
            {
                traversal.Text += x.getValue() + " "; ;
                preOrderTraverse(x.getLeftChild(), traversal);
                preOrderTraverse(x.getRightChild(), traversal);
            }
        }

        public void postOrderTraverse(WebNode x, TextBlock traversal)
        {
            if (x != null)
            {
                postOrderTraverse(x.getLeftChild(), traversal);
                postOrderTraverse(x.getRightChild(), traversal);
                traversal.Text += x.getValue() + " ";
            }
        }

        //  when a node is removed this method is called to update the position values in the tree and then redraw the nodes on the canvas
        public void redrawTree(WebNode x, ListBox traversal) // size-n problem
        {
            if (x != null)
            {
                traversal.Items.Add("\n Node with value: " + x.getValue() + " visited!");
            }
            if (x.getParent() != null)
            {
                traversal.Items.Add("\n Parent with value: " + x.getParent().getValue() + "!");
            }
            if(x.getParent() != null) // not root // construct solution to size m problem
            {
                if(x.getValue() < x.getParent().getValue()) // leftChild
                {
                    x.setPositionInTree(x.getParent().getPositionInTree() * 2);
                    x.drawNode();
                    x.getParent().drawLines(x, mainSpace);
                }
                else // rightChild
                {
                    x.setPositionInTree(x.getParent().getPositionInTree() * 2 + 1);
                    x.drawNode();
                    x.getParent().drawLines(x, mainSpace);
                }
            }
            if (x == root) // construct solution to size m problem
            {
                x.setPositionInTree(1);
                x.drawNode();
            }
            if (x.getLeftChild() == null && x.getRightChild() == null) // stopping condition
            {
                return;
            }
            if (x.getLeftChild() != null)
            {
                redrawTree(x.getLeftChild(), traversal); // recursively call method m-1 problem
            }
            if (x.getRightChild() != null)
            {
                redrawTree(x.getRightChild(), traversal); // recursively call method m-1 problem
            }
        } // end redraw tree
    }
}

