using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BST
{
    public partial class MainPage : UserControl
    {
        private WebBST bTree;
        public MainPage()
        {
            InitializeComponent();
            // create empty Binary Search Tree
            bTree = new WebBST(mainSpace);
            fadeWelcome(welcomeTextBlock); // fade out welcome message

            // for testing on artificially populated BST
            /*bTree.add(1);
            bTree.incrementTreeSize();
            bTree.add(6);
            bTree.incrementTreeSize();
            bTree.add(2);
            bTree.incrementTreeSize();
            bTree.add(0);
            bTree.incrementTreeSize();
            bTree.add(4);
            bTree.incrementTreeSize();
            bTree.add(5);
            bTree.incrementTreeSize();
            bTree.add(3);
            bTree.incrementTreeSize();
            */
        }

        // fades the welcome message out after 5 seconds
        private void fadeWelcome(TextBlock welcome)
        {
            DoubleAnimation fadeAnim = new DoubleAnimation();
            Duration duration = new Duration(TimeSpan.FromSeconds(5));
            fadeAnim.Duration = duration;
            fadeAnim.From = 1;
            fadeAnim.To = 0;
            Storyboard fadeStory = new Storyboard();
            fadeStory.Children.Add(fadeAnim);
            Storyboard.SetTarget(fadeAnim, welcomeTextBlock);
            Storyboard.SetTargetProperty(fadeAnim, new PropertyPath("(Opacity)"));
            fadeStory.Begin();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            welcomeTextBlock.Text = "";
            int result;
            traversalListBox.Items.Clear(); // clears output window
            if (string.IsNullOrWhiteSpace(addTextBox.Text))
            {
                traversalListBox.Items.Add("Please enter a value in the add box!");
            }
            else if (int.TryParse(addTextBox.Text, out result))
            {
                if (bTree.add(Int32.Parse(addTextBox.Text)))
                {
                    bTree.incrementTreeSize();
                }
                else
                {
                    traversalListBox.Items.Add("Node already in tree!");
                }
            }
            else
            {
                traversalListBox.Items.Add("Please enter an integer and then click add again!");
            }
            addTextBox.Text = ""; // clear add field
        } // end add click handler

        private void Button_Click_Remove(object sender, RoutedEventArgs e)
        {
            int result;
            traversalListBox.Items.Clear(); // clears output window
            if (string.IsNullOrWhiteSpace(removeTextBox.Text))
            {
                traversalListBox.Items.Add("Please enter a value in the remove box!");
            }
            else if (int.TryParse(addTextBox.Text, out result))
            {
                if (bTree.remove(Int32.Parse(removeTextBox.Text))) // in tree and removed
                {
                    traversalListBox.Items.Add("Node with value " + removeTextBox.Text + " deleted!");
                    bTree.decrementTreeSize();
                    if (bTree.getTreeSize() > 0)
                    {
                        bTree.redrawTree(bTree.getRoot(), traversalListBox); // redraw the updated b-tree to the canvas
                    }
                }
                else // not found
                {
                    traversalListBox.Items.Add("Node with value: " + removeTextBox.Text + " not found in BST!");
                }
            }
            else
            {
                traversalListBox.Items.Add("Please enter an integer then click remove again!");
            }
            removeTextBox.Text = ""; // clear remove field
        } // end remove button click

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            int result;
            traversalListBox.Items.Clear(); // clears output window
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                traversalListBox.Items.Add("Please enter a value in the search box!");
            }
            else if (int.TryParse(addTextBox.Text, out result))
            {
                WebNode temp = bTree.TreeSearch(bTree.getRoot(), Int32.Parse(searchTextBox.Text));
                if (temp != null)
                {
                    traversalListBox.Items.Add("Node at position " + temp.getPositionInTree().ToString() + "!");
                }
                else
                {
                    traversalListBox.Items.Add("Node not in tree!");
                }
            }
            else
            {
                traversalListBox.Items.Add("Please enter an integer and then click search again!");
            }
            searchTextBox.Text = ""; // clear search field
        } // end search button click handler

        private void Button_Click_InOrder(object sender, RoutedEventArgs e)
        {
            resultTextBlock.Text = "";
            traversalListBox.Items.Clear();      
            if (bTree.getTreeSize() > 0)
            {
                traversalListBox.Items.Add("InOrder Traversal Is:"); 
                bTree.inOrderTraverse(bTree.getRoot(), resultTextBlock);
                traversalListBox.Items.Add(resultTextBlock.Text);
                resultTextBlock.Text = "";
            }
            else
            {
                traversalListBox.Items.Add("Tree is empty! No in-order traversal possible.");
            }
        }

        private void Button_Click_preOrder(object sender, RoutedEventArgs e)
        {
            resultTextBlock.Text = "";
            traversalListBox.Items.Clear();
            if (bTree.getTreeSize() > 0)
            {
                traversalListBox.Items.Add("PreOrder Traversal Is:"); 
                bTree.preOrderTraverse(bTree.getRoot(), resultTextBlock);
                traversalListBox.Items.Add(resultTextBlock.Text);
                resultTextBlock.Text = "";
            }
            else
            {
                traversalListBox.Items.Add("Tree is empty! No pre-order traversal possible.");
            }
        }

        private void Button_Click_postOrder(object sender, RoutedEventArgs e)
        {
            resultTextBlock.Text = "";
            traversalListBox.Items.Clear(); 
            if (bTree.getTreeSize() > 0)
            {
                traversalListBox.Items.Add("PostOrder Traversal Is:");
                bTree.postOrderTraverse(bTree.getRoot(), resultTextBlock);
                traversalListBox.Items.Add(resultTextBlock.Text);
                resultTextBlock.Text = "";
            }
            else
            {
                traversalListBox.Items.Add("Tree is empty! No post-order traversal possible.");
            }
        }       
    }
}
                    /* for testing
                    resultTextBlock.Text += "\nLevel is: " + temp.getLevel();
                    int nodesOnLevel = (int)Math.Pow(Convert.ToDouble(2), Convert.ToDouble(temp.getLevel()));
                    resultTextBlock.Text += "\nNodes on Level is: " + nodesOnLevel.ToString();
                    int slot = temp.getPositionInTree() % nodesOnLevel;
                    resultTextBlock.Text += "\nSlot on Level is: " + slot.ToString();
                    // show parent
                    if (temp.getParent() != null)
                    {
                        resultTextBlock.Text += "\nNode Parent is: " + temp.getParent().getValue().ToString();
                    }
                    else
                    {
                        resultTextBlock.Text += "\nNode Parent is: Null" ;
                    }
                    */