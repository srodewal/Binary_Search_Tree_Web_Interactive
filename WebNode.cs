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
    public class WebNode
    {
        int data;
        WebNode leftChild = null;
        WebNode rightChild = null;
        WebNode parent = null; // keep track of parent for lines

        int vertHeight; //hardcoded
        int horizWidth; //hardcoded

        int positionInTree; //what place in the list is this?
        //bool isTopRow;

        //Rectangle upperBox, lowerBox;
        //TextBlock valueLabel, nullLabel;
        Rectangle nodeBox;
        TextBlock nodeBlock;
        //Line lineToNext, lineLeftChild, lineRightChild;
        Line lineLeftChild, lineRightChild;
        Canvas mainSpace;

        public WebNode(int value, int vh, int hw, Canvas mS, int positionInTree) // bool itr
        {
            data = value; // update String data for node
            leftChild = null;
            rightChild = null;

            vertHeight = vh; // set position of node
            horizWidth = hw;

            //isTopRow = itr; // ?

            mainSpace = mS; // set canvas?
            //positionInList = pil; // position

            this.positionInTree = positionInTree;

            drawNode(); // call to function
        }

        public TextBlock getNodeTextLabel()
        {
            return nodeBlock;
        }

        public Rectangle getNodeBox()
        {
            return nodeBox;
        }

        public void drawNode()
        {
            mainSpace.Children.Remove(nodeBox); // ??
            mainSpace.Children.Remove(nodeBlock);
            mainSpace.Children.Remove(lineLeftChild); // ??
            mainSpace.Children.Remove(lineRightChild); // ??


            nodeBox = new Rectangle()
            {
                Height = vertHeight,
                Width = horizWidth,
                Fill = new SolidColorBrush(Colors.White),
                Stroke = new SolidColorBrush(Colors.Black),
            };

            nodeBlock = new TextBlock()
            {
                FontSize = 12,
                TextAlignment = TextAlignment.Center,
                Height = vertHeight,
                Width = horizWidth,
            };

            nodeBlock.Text = data.ToString(); // cast to string

            //this.redrawLine(); // what

            /*if (isTopRow)
            {
                Canvas.SetTop(upperBox, 70);
                Canvas.SetTop(lowerBox, 100);
                Canvas.SetTop(valueLabel, 70);
                Canvas.SetTop(nullLabel, 100);
            }
            else
            {
                Canvas.SetTop(upperBox, 270);
                Canvas.SetTop(lowerBox, 300);
                Canvas.SetTop(valueLabel, 270);
                Canvas.SetTop(nullLabel, 300);
            }*/

            Canvas.SetTop(nodeBox, 25 + 75*getLevel()); // change value was 50
            Canvas.SetTop(nodeBlock, 25 + 75*getLevel()); // change value was 50

            //Canvas.SetLeft(nodeBox, (25 + 175 * (positionInList - 1))); // change calculations
            //Canvas.SetLeft(nodeBlock, (25 + 175 * (positionInList - 1))); // change calculations

            if (getLevel() == 0)
            {
                Canvas.SetLeft(nodeBox, mainSpace.Width / 2); // change calculations // 375?
                Canvas.SetLeft(nodeBlock, mainSpace.Width / 2); // change calculations // 375?
            }
            else
            {
                int nodesOnLevel = (int)Math.Pow(Convert.ToDouble(2),Convert.ToDouble(getLevel()));
                int slot = positionInTree % nodesOnLevel;
                //int slot = 2; stub
                Canvas.SetLeft(nodeBox, (slot+1)*(mainSpace.Width/(nodesOnLevel+1)));
                Canvas.SetLeft(nodeBlock, (slot+1)*(mainSpace.Width/(nodesOnLevel+1)));
            }
            //Canvas.SetLeft(nodeBox, mainSpace.Width/2); // change calculations // 375?
            //Canvas.SetLeft(nodeBlock, mainSpace.Width/2); // change calculations // 375?

            mainSpace.Children.Add(nodeBox);
            mainSpace.Children.Add(nodeBlock);

        } // end drawNode?

        public int getValue()
        {
            return data;
        }

        public WebNode getLeftChild()
        {
            return leftChild;
        }

        public WebNode getRightChild()
        {
            return rightChild;
        }

        public WebNode getParent()
        {
            return parent;
        }

        public void setParent(WebNode n)
        {
            parent = n;
        }

        public void setRightChild(WebNode n)
        {
            rightChild = n;
        }

        public void setLeftChild(WebNode n)
        {
            leftChild = n;
        }

        public int getPositionInTree()
        {
            return positionInTree;
        }

        public void setPositionInTree(int p)
        {
            positionInTree = p;
        }

        public int getLevel()
        {
            return (int)Math.Floor(Math.Log(Convert.ToDouble(positionInTree), Convert.ToDouble(2)));
        }

        /*public void setIsTopRow(bool i)
        {
            isTopRow = i;
        }*/

        public bool add(int data, int position)
        {
            if (data == this.data)
            {
                return false;
            }
            else if (data < this.data)
            {
                if (leftChild == null)
                {
                    position = position * 2;
                    leftChild = new WebNode(data, 25, 25, mainSpace, position); // 25's are height and width
                    leftChild.setPositionInTree(position); // set position // redundant
                    leftChild.setParent(this);
                    // draw line between parent and child
                    this.drawLines(leftChild, mainSpace);
                    return true;
                }
                else
                {
                    position = position * 2;
                    return leftChild.add(data, position);
                }
            }
            else if (data > this.data)
            {
                if (rightChild == null)
                {
                    position = position * 2 + 1;
                    rightChild = new WebNode(data, 25, 25, mainSpace, position); // 25's are height and width
                    rightChild.setPositionInTree(position); // redundant done in constructor
                    rightChild.setParent(this);
                    // draw line between parent and child
                    this.drawLines(rightChild, mainSpace);
                    return true;
                }
                else
                {
                    position = position * 2 + 1;
                    return rightChild.add(data, position);
                }
            }
            return false;
        }

        // from Node.cs
        public bool remove(int data, WebNode parent)
        {
            if (data < this.data)
            {
                if (leftChild != null)
                {
                    return leftChild.remove(data, this);
                }
                else
                {
                    return false;
                }
            }
            else if (data > this.data)
            {
                if (rightChild != null)
                {
                    return rightChild.remove(data, this);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (leftChild != null && rightChild != null)
                {
                    this.data = rightChild.minimum(); // get smallest node underneath rightChild
                    rightChild.remove(this.getValue(), this); // duh // forgot this somehow
                    mainSpace.Children.Clear(); // remove all nodes
                }
                else if (parent.leftChild == this)
                {
                    if (leftChild != null)
                    {
                        parent.leftChild = leftChild;
                        if (leftChild != null)
                        {
                            leftChild.setParent(parent); // set left child's parent to new parent
                        }
                        mainSpace.Children.Clear(); // remove all nodes
                    }
                    else
                    {
                        parent.leftChild = rightChild;
                        if (rightChild != null)
                        {
                            rightChild.setParent(parent); // set right child's parent to new parent
                        } 
                        mainSpace.Children.Clear(); // remove all nodes
                    }
                }
                else if (parent.rightChild == this)
                {
                    if (leftChild != null)
                    {
                        parent.rightChild = leftChild;
                        if (leftChild != null)
                        {
                            leftChild.setParent(parent); // set left child's parent to new parent
                        }
                        mainSpace.Children.Clear(); // remove all nodes
                    }
                    else
                    {
                        parent.rightChild = rightChild;
                        if(rightChild != null) {
                            rightChild.setParent(parent); // set right child's parent to new parent
                        }
                        mainSpace.Children.Clear(); // remove all nodes
                    }
                }
                return true;
            }
        }
        /*
        public bool remove(int data, WebNode parent)
        {
            if (data < this.data)
            {
                if (leftChild != null)
                {
                    return leftChild.remove(data, this);
                }
                else
                {
                    return false;
                }
            }
            else if (data > this.data)
            {
                if (rightChild != null)
                {
                    return rightChild.remove(data, this);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (leftChild != null && rightChild != null)
                {
                    this.data = rightChild.minimum(); // get smallest node in right subtree
                    mainSpace.Children.Clear(); // remove all nodes
                    // redraw
                   
                    // mainSpace.Children.Remove(rightChild.minimumNode().getNodeBox());
                    //mainSpace.Children.Remove(rightChild.minimumNode().getNodeTextLabel());
                    //if(rightChild.minimumNode().getValue() < rightChild.minimumNode().getParent().getValue()) { // if min node is left child
                    //    mainSpace.Children.Remove(rightChild.minimumNode().getParent().lineLeftChild);
                    //}
                    //else
                    //{
                     //   mainSpace.Children.Remove(rightChild.minimumNode().getParent().lineRightChild);
                    //}
                    //this.drawNode();
                    //traverseSubTree(this, this.getPositionInTree()); // should call this on this
                    
                }
                else if (parent.leftChild == this)
                {
                    if (leftChild != null)
                    { 
                        mainSpace.Children.Remove(parent.leftChild.getNodeBox());
                        mainSpace.Children.Remove(parent.leftChild.getNodeTextLabel());
                        leftChild.setPositionInTree(this.getPositionInTree());
                        leftChild.drawNode();
                        // 
                        mainSpace.Children.Remove(this.lineLeftChild); // testing
                       
                        //traverseSubTree(this, this.getPositionInTree());

                        parent.leftChild = leftChild;
                        mainSpace.Children.Clear(); // remove all nodes
                        // redraw
                    }
                    else
                    {
                        
                        mainSpace.Children.Remove(parent.leftChild.getNodeBox());
                        mainSpace.Children.Remove(parent.leftChild.getNodeTextLabel());
                        // 
                        if (rightChild != null)
                        {
                            rightChild.setPositionInTree(this.getPositionInTree());
                            rightChild.drawNode();
                        }
                        mainSpace.Children.Remove(parent.lineLeftChild);
                        mainSpace.Children.Remove(this.lineRightChild); // testing
                         //traverseSubTree(this, this.getPositionInTree());
        
                        parent.leftChild = rightChild;
                        mainSpace.Children.Clear(); // remove all nodes
                        // redraw
                    }
                }
                else if (parent.rightChild == this)
                {
                    if (leftChild != null)
                    {
                        
                        //
                        mainSpace.Children.Remove(parent.rightChild.getNodeBox());
                        mainSpace.Children.Remove(parent.rightChild.getNodeTextLabel());
                        leftChild.setPositionInTree(this.getPositionInTree());
                        leftChild.drawNode();
                        // 
                        mainSpace.Children.Remove(this.lineLeftChild); // testing
                        //traverseSubTree(this, this.getPositionInTree());
                         
                        parent.rightChild = leftChild;
                        mainSpace.Children.Clear(); // remove all nodes
                        // redraw
                    }
                    else
                    {
                       
                        mainSpace.Children.Remove(parent.rightChild.getNodeBox());
                        mainSpace.Children.Remove(parent.rightChild.getNodeTextLabel());
                        // 
                        if (rightChild != null)
                        {
                            rightChild.setPositionInTree(this.getPositionInTree());
                            rightChild.drawNode();
                        }
                        mainSpace.Children.Remove(parent.lineRightChild);
                        mainSpace.Children.Remove(this.lineRightChild); // testing
                         //traverseSubTree(this, this.getPositionInTree());
                         

                        parent.rightChild = rightChild;
                        mainSpace.Children.Clear(); // remove all nodes
                        // redraw
                    }
                }
                return true;
            }
        }
        */

        public void traverseSubTree(WebNode curr, int pos)
        {
            if (curr.getLeftChild() == null && curr.getRightChild() == null) // stopping condition
            {
                return; // return value
            }
            else
            {
                if (curr.getLeftChild() != null)
                {
                    curr.getLeftChild().setPositionInTree(this.getPositionInTree() * 2);
                    curr.getLeftChild().drawNode();
                    if (curr.getLeftChild().getLeftChild() != null)
                    {
                        curr.getLeftChild().drawLines(curr.getLeftChild().getLeftChild(), mainSpace);
                    }
                    if (curr.getLeftChild().getRightChild() != null)
                    {
                        curr.getLeftChild().drawLines(curr.getLeftChild().getRightChild(), mainSpace);
                    }
                    traverseSubTree(curr.getLeftChild(), curr.getLeftChild().getPositionInTree());
                }
                if (curr.getRightChild() != null)
                {
                    curr.getRightChild().setPositionInTree(this.getPositionInTree() * 2 + 1);
                    curr.getRightChild().drawNode();
                    if (curr.getRightChild().getLeftChild() != null)
                    {
                        curr.getRightChild().drawLines(curr.getRightChild().getLeftChild(), mainSpace);
                    }
                    if (curr.getRightChild().getRightChild() != null)
                    {
                        curr.getRightChild().drawLines(curr.getRightChild().getRightChild(), mainSpace);
                    }
                    traverseSubTree(curr.getRightChild(), curr.getRightChild().getPositionInTree());
                }
            }
        } 

        public int minimum()
        {
            if (leftChild == null)
            {
                return data;
            }
            else
            {
                return leftChild.minimum();
            }
        }

        // added to get minimum node in subtree // may be redundant
        public WebNode minimumNode()
        {
            if (leftChild == null)
            {
                return this;
            }
            else
            {
                return leftChild.minimumNode();
            }
        }

        public void drawLines(WebNode aChild, Canvas ms)
        {
            /*Line tempLine = new Line();
            tempLine.X1 = 25;
            tempLine.X2 = 50;
            tempLine.Y1 = 75;
            tempLine.Y2 = 100;
            tempLine.Stroke = new SolidColorBrush(Colors.Brown);
            mainSpace.Children.Add(tempLine);
            //tempLine.Visibility = Visibility.Visible;
            */

            mainSpace = ms;

            // draw left/right child lines
            //mainSpace.Children.Remove(lineLeftChild);
            //mainSpace.Children.Remove(lineRightChild);
            lineLeftChild = new Line();
            lineRightChild = new Line();

            //Canvas.SetTop(nodeBox, 25 + 50 * getLevel()); // change value was 50
            //Canvas.SetTop(nodeBlock, 25 + 50 * getLevel()); // change value was 50
            //Canvas.SetLeft(nodeBox, mainSpace.Width / 2); // change calculations // 375?
            //Canvas.SetLeft(nodeBlock, mainSpace.Width / 2); // change calculations // 375?
            //int nodesOnLevel = (int)Math.Pow(Convert.ToDouble(2), Convert.ToDouble(getLevel()));
            //int slot = positionInTree % nodesOnLevel;
            //Canvas.SetLeft(nodeBox, (slot + 1) * (mainSpace.Width / (nodesOnLevel + 1)));
            //Canvas.SetLeft(nodeBlock, (slot + 1) * (mainSpace.Width / (nodesOnLevel + 1)));

            if (aChild.getValue() < aChild.getParent().getValue())
            {
                /*Line tempLine2 = new Line();
                tempLine2.X1 = 50;
                tempLine2.X2 = 75;
                tempLine2.Y1 = 100;
                tempLine2.Y2 = 125;
                tempLine2.Stroke = new SolidColorBrush(Colors.Brown);
                mainSpace.Children.Add(tempLine2);

                //tempLine2.Visibility = Visibility.Visible;
                */
                
                int nodesOnLevel;
                int slot;
                if (aChild.getParent().getLevel() == 0)
                {
                    lineLeftChild.X1 = 12.5+mainSpace.Width / 2;
                }
                else {
                    nodesOnLevel = (int)Math.Pow(Convert.ToDouble(2), Convert.ToDouble(aChild.getParent().getLevel()));  
                    slot = aChild.getParent().getPositionInTree() % nodesOnLevel;
                    lineLeftChild.X1 = 12.5+(slot + 1) * (mainSpace.Width / (nodesOnLevel + 1));
                }
                nodesOnLevel = (int)Math.Pow(Convert.ToDouble(2), Convert.ToDouble(aChild.getLevel()));  
                slot = aChild.getPositionInTree() % nodesOnLevel;
                lineLeftChild.X2 = 12.5+(slot + 1) * (mainSpace.Width / (nodesOnLevel + 1));

                lineLeftChild.Y1 = 25+25 + 75 * aChild.getParent().getLevel();
                lineLeftChild.Y2 = 25 + 75 * aChild.getLevel();

                lineLeftChild.Stroke = new SolidColorBrush(Colors.Brown);
                //lineLeftChild.StrokeThickness = 2;
                ///lineLeftChild.Visibility = Visibility.Visible;

                mainSpace.Children.Add(lineLeftChild);
            }

            //if (parentNode.getRightChild() != null)
            if (aChild.getValue() > aChild.getParent().getValue())
            {
                /*
                Line tempLine3 = new Line();
                tempLine3.X1 = 250;
                tempLine3.X2 = 275;
                tempLine3.Y1 = 200;
                tempLine3.Y2 = 225;
                tempLine3.Stroke = new SolidColorBrush(Colors.Brown);
                mainSpace.Children.Add(tempLine3);
                */

                int nodesOnLevel;
                int slot;
                if (aChild.getParent().getLevel() == 0)
                {
                    lineRightChild.X1 = 12.5+mainSpace.Width / 2;
                }
                else
                {
                    nodesOnLevel = (int)Math.Pow(Convert.ToDouble(2), Convert.ToDouble(aChild.getParent().getLevel()));
                    slot = aChild.getParent().getPositionInTree() % nodesOnLevel;
                    lineRightChild.X1 = 12.5+((slot + 1) * (mainSpace.Width / (nodesOnLevel + 1)));
                }
                nodesOnLevel = (int)Math.Pow(Convert.ToDouble(2), Convert.ToDouble(aChild.getLevel()));
                slot = aChild.getPositionInTree() % nodesOnLevel;
                lineRightChild.X2 = 12.5+(slot + 1) * (mainSpace.Width / (nodesOnLevel + 1));

                lineRightChild.Y1 = 25+25 + 75 * aChild.getParent().getLevel();

                lineRightChild.Y2 = 25 + 75 * aChild.getLevel();
                //lineRightChild.Y2 = 75;

                lineRightChild.Stroke = new SolidColorBrush(Colors.Brown);
                //lineRightChild.StrokeThickness = 2;
                //lineRightChild.Visibility = Visibility.Visible;

                mainSpace.Children.Add(lineRightChild);
            }
        }

    }


} // end namespace
