// See https://aka.ms/new-console-template for more information
using DSA;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using static DSA.Algorithms;

//Random data:
int[] linData1 = Enumerable.Range(1, 1000000).OrderBy(x => Guid.NewGuid()).ToArray();
int[] linData2 = Enumerable.Range(1, 100).OrderBy(x => Guid.NewGuid()).ToArray();
int[] linData3 = Enumerable.Range(1, 1000).OrderBy(x => Guid.NewGuid()).ToArray();
int[] linData4 = Enumerable.Range(1, 10000).OrderBy(x => Guid.NewGuid()).ToArray();
int[] linData5 = Enumerable.Range(1, 100000).OrderBy(x => Guid.NewGuid()).ToArray();
int[] testData1 = Enumerable.Range(1, 10000).OrderBy(x => Guid.NewGuid()).ToArray();

int treeDepth = 3;
BinaryNode<int> rootNode = GenerateBinaryTree(treeDepth);

//Sorted data:
int[] testData2 = Enumerable.Range(1, 1000000).ToArray();
//Reverse sorted data:
int[] testData3 = Enumerable.Range(1, 1000000).OrderByDescending(x => x).ToArray();
//All zeros:
int[] testData4 = new int[1000000];
//All ones:
int[] testData5 = Enumerable.Repeat(1, 1000000).ToArray();

string[] maze = new string[]
{
    "############ #",
    "#          # #",
    "#          # #",
    "############ #",
    "#            #",
    "# ############",
};

//bool test data for crystal ball problem.
bool[] testData6 = GenerateBoolTestData(100);

Algorithms.RunTest(Algorithms.BreadthFirstSearch, rootNode, 2);
//Algorithms.RunTest(Algorithms.Quicksort, linData2, linData2[0], linData2[linData2.Length - 1]);

//maze solver
//var path = Algorithms.SolveMaze(maze, '#', new Algorithms.PointM() { x = 12, y = 0 },
//    new Algorithms.PointM() { x = 1, y = 5 });

//foreach (var tile in path)
//{
//    Console.WriteLine($"[{tile.x}, {tile.y}]");
//}


//For crystal ball problem.
static bool[] GenerateBoolTestData(int size)
{
    // Initialize the array with all false values
    bool[] data = new bool[size];

    // Generate a random index to set as true
    Random random = new Random();
    int trueIndex = random.Next(0, size);

    // Set the value at the random index and all subsequent indices to true
    for (int i = trueIndex; i < size; i++)
    {
        data[i] = true;
    }

    return data;
}

namespace DSA
{
    public static class Algorithms
    {
        #region Testing Functions

        public static void RunTest(Func<int[], int, bool> algorithm, int[] testData, int target)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool result = algorithm(testData, target);

            stopwatch.Stop();

            Console.WriteLine($"{algorithm.Method.Name} Test - Target: {target}, Result: {result}, Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void RunTest(Func<int[], int[]> algorithm, int[] testData)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int[] result = algorithm(testData);

            stopwatch.Stop();

            Console.Write("[");
            for (int i = 0; i < result.Length - 1; i++)
            {
                Console.Write($"{result[i]},");
            }
            Console.Write("]");
            Console.WriteLine();

            Console.WriteLine($"{algorithm.Method.Name} Test - Result: {result}, Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void RunTest(Func<BinaryNode<int>, int, bool> algorithm, BinaryNode<int> head, int searchNum)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool result = algorithm(head, searchNum);

            stopwatch.Stop();

            // Print the binary tree in a pretty format
            PrintBinaryTree(head, 2);

            Console.WriteLine($"{algorithm.Method.Name} Test - Result: {result}, Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void RunTest(Func<BinaryNode<int>, List<int>> algorithm, BinaryNode<int> treeHead)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<int> result = algorithm(treeHead);

            stopwatch.Stop();

            // Print the binary tree in a pretty format
            PrintBinaryTree(treeHead, 4);

            Console.WriteLine($"{algorithm.Method.Name} - Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void RunTest(Action<int[], int, int> algorithm, int[] testData, int high, int low)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            algorithm(testData, 0, testData.Length - 1);

            stopwatch.Stop();

            Console.Write("[");
            for (int i = 0; i < testData.Length - 1; i++)
            {
                Console.Write($"{testData[i]},");
            }
            Console.Write("]");
            Console.WriteLine();

            Console.WriteLine($"{algorithm.Method.Name} Test - Result: {testData}, Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void RunTest(Func<bool[], int> algorithm, bool[] testData)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int result = algorithm(testData);

            stopwatch.Stop();

            Console.WriteLine($"{algorithm.Method.Name} - Result: {result}, Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        #endregion

        #region Search Algorithms

        public static bool LinearSearch(int[] haystack, int needle)
        {
            for (int i = 0; i < haystack.Length; i++)
            {
                if (haystack[i] == needle)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool BinarySearch(int[] arr, int val)
        {
            Array.Sort(arr);
            int low = 0;
            var high = arr.Length - 1;

            while (low <= high)
            {
                int mid = (int)Math.Floor((decimal)(low + (high - low) / 2));
                var v = arr[mid];

                if (v == val)
                {
                    return true;
                }
                else if (val > v)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return false;
        }

        public static int TwoCrystalBall(bool[] breaks)
        {
            int jumpAmount = (int)Math.Floor(Math.Sqrt(breaks.Length));
            var i = (int)jumpAmount;
            for (; i < breaks.Length; i += jumpAmount)
            {
                if (breaks[i])
                    break;
            }

            i -= jumpAmount;

            for (int j = 0; j < jumpAmount && i < breaks.Length; ++j, ++i)
            {
                if (breaks[i])
                    return i;
            }

            return -1;
        }

        #endregion

        #region Sorting Algorithms

        public static int[] BubbleSort(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; ++i)
            {
                for (int j = 0; j < arr.Length - 1 - i; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        var temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }

            return arr;
        }

        public static void Quicksort(int[] array, int low, int high)
        {
            if (low < high)
            {
                // Partition the array and get the pivot index
                int pivotIndex = Partition(array, low, high);

                // Recursively sort the sub-arrays on either side of the pivot
                Quicksort(array, low, pivotIndex - 1);
                Quicksort(array, pivotIndex + 1, high);
            }
        }

        private static int Partition(int[] array, int low, int high)
        {
            // Choose the rightmost element as the pivot
            int pivot = array[high];

            // Index of the smaller element
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                // If the current element is smaller than or equal to the pivot
                if (array[j] <= pivot)
                {
                    // Swap array[i] and array[j]
                    i++;
                    Swap(array, i, j);
                }
            }

            // Swap array[i+1] and array[high] (put the pivot in its correct position)
            Swap(array, i + 1, high);

            // Return the index of the pivot element
            return i + 1;
        }


        private static void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        #endregion

        #region Recursion

        private readonly static int[][] dir = new int[][]
        {
            new int[] {-1, 0},
            new int[] {1, 0},
            new int[] {0, -1 },
            new int[] {0 , 1}
        };

        public class PointM
        {
            public int x;
            public int y;
        }

        private static bool Walk(string[] maze, char wall, PointM curr, PointM end, bool[][] seen, Stack<PointM> path)
        {
            if (curr.x < 0 || curr.x >= maze[0].Length || curr.y < 0 || curr.y >= maze.Length)
            {
                return false;
            }

            if (maze[curr.y][curr.x] == wall)
            {
                return false;
            }

            if (curr.x == end.x && curr.y == end.y)
            {
                path.Push(end);
                return true;
            }

            if (seen[curr.y][curr.x])
            {
                return false;
            }

            //pre
            seen[curr.y][curr.x] = true;
            path.Push(curr);
            //recurse
            for (int i = 0; i < dir.Length; ++i)
            {
                int[] newCur = dir[i];
                if (Walk(maze, wall, new PointM() { x = curr.x + newCur[0], y = curr.y + newCur[1] },
                    end, seen, path))
                    return true;
            }
            //post
            path.Pop();

            return false;
        }

        public static Stack<PointM> SolveMaze(string[] maze, char wall, PointM start, PointM end)
        {
            var seen = Enumerable.Range(0, maze.Length)
            .Select(_ => Enumerable.Repeat(false, maze[0].Length).ToArray())
            .ToArray();
            var path = new Stack<PointM>();

            Walk(maze, wall, start, end, seen, path);

            return path;
        }

        #endregion

        #region Binary Tree

        private static int GetRandomNumber()
        {
            // You can customize the range of random numbers as needed
            Random random = new Random();
            return random.Next(1, 100);
        }

        public class BinaryNode<T>
        {
            public BinaryNode()
            {
                Value = default(T);
                Left = null;
                Right = null;
            }

            public T? Value { get; set; }
            public BinaryNode<T>? Left { get; set; }
            public BinaryNode<T>? Right { get; set; }
        }

        private static void PreWalk(BinaryNode<int> curr, List<int> path)
        {
            if (curr == null)
                return;

            //pre
            path.Add(curr.Value);
            //recurse
            PreWalk(curr.Left, path);
            PreWalk(curr.Right, path);
            //post
            return;
        }

        private static void InOrderWalk(BinaryNode<int> curr, List<int> path)
        {
            if (curr == null)
                return;

            //pre

            //recurse
            InOrderWalk(curr.Left, path);
            path.Add(curr.Value);
            InOrderWalk(curr.Right, path);
            //post
            return;
        }

        private static void PostOrderWalk(BinaryNode<int> curr, List<int> path)
        {
            if (curr == null)
                return;

            //pre

            //recurse
            PostOrderWalk(curr.Left, path);
            PostOrderWalk(curr.Right, path);
            path.Add(curr.Value);
            //post
            return;
        }

        public static List<int> PreOrderTreeTraversal(BinaryNode<int> head)
        {
            var path = new List<int>();
            PreWalk(head, path);
            return path;
        }

        public static List<int> InOrderTreeTraversal(BinaryNode<int> head)
        {
            var path = new List<int>();
            InOrderWalk(head, path);
            return path;
        }

        public static List<int> PostOrderTreeTraversal(BinaryNode<int> head)
        {
            var path = new List<int>();
            PostOrderWalk(head, path);
            return path;
        }

        public  static BinaryNode<int> GenerateBinaryTree(int depth)
        {
            if (depth < 0)
            {
                return null;
            }

            var node = new BinaryNode<int> { Value = GetRandomNumber() };

            // Recursively generate left and right subtrees
            node.Left = GenerateBinaryTree(depth - 1);
            node.Right = GenerateBinaryTree(depth - 1);

            return node;
        }

        static void PrintBinaryTree<T>(BinaryNode<T>? root, int indent)
        {
            if (root == null)
            {
                return;
            }

            // Increase the indentation for each level
            indent += 4;

            // Print the right subtree
            PrintBinaryTree(root.Right, indent);

            // Print the current node with indentation
            Console.WriteLine(new string(' ', indent) + root.Value);

            // Print the left subtree
            PrintBinaryTree(root.Left, indent);
        }

        public static bool BreadthFirstSearch(BinaryNode<int> head, int num)
        {
            var que = new Queue<BinaryNode<int>>();
            que.Enqueue(head);

            while(que.Count > 0)
            {
                var curr = que.Dequeue();

                //search
                if(curr.Value == num)
                {
                    return true;
                }

                if(curr.Left != null)
                    que.Enqueue(curr.Left);

                if (curr.Right != null)
                    que.Enqueue(curr.Right);
            }

            return false;
        }

        #endregion
    }

    #region Data Structures

    #region Queue

    public class QNode<T>
    {
        public QNode()
        {
            Next = null;
        }

        public T? Value { get; set; }
        public QNode<T>? Next { get; set; }
    }

    public class Que<T>
    {
        private QNode<T>? _head;
        private QNode<T>? _tail;
        public Que()
        {
            _head = _tail = null;
            Length = 0;
        }

        public int Length { get; set; }

        public void Enqueue(T item)
        {
            var node = new QNode<T> { Value = item };
            Length++;
            if (Length == 0 || _tail == null) _head = _tail = node;

            _tail.Next = node;
            _tail = node;
        }

        public T? Dequeue()
        {
            if (_head == null) return default(T);

            this.Length--;

            var head = _head;
            _head = _head.Next;
            head.Next = null;

            if (Length == 0)
                _tail = null;
            return head.Value;

        }

        public T? Peek()
        {
            return _head != null ? _head.Value : default(T);
        }

    }

    #endregion

    #region Stack

    public class Node<T>
    {
        public T? Value { get; set; }

        public Node<T>? Previous { get; set; } 
    }

    public class Stak<T>
    {
        private Node<T>? _head;
        public Stak()
        {
            _head = null;
            Length = 0;
        }

        public int Length { get; set; }

        public void Push(T item) 
        {
            Length++;
            Node<T> node = new Node<T>() { Value = item };
            if (_head == null) _head = node;

            node.Previous = _head;
            _head = node;
        }

        public T? Pop()
        {
            Length = Math.Max(0, Length - 1);
            if(Length == 0)
            {
                var head = _head;
                _head = null;
                return head != null ? head.Value : default(T);
            }

            var head2 = _head;
            _head = head2?.Previous;
            return _head != null ? _head.Value : default(T);

        }

        public T? Peek()
        {
            return _head != null ? _head.Value : default(T);
        }
    }

    #endregion

    #endregion
}






