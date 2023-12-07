// See https://aka.ms/new-console-template for more information
using DSA;
using System.Diagnostics;

//Random data:
int[] testData1 = Enumerable.Range(1, 100).OrderBy(x => Guid.NewGuid()).ToArray();
//Sorted data:
int[] testData2 = Enumerable.Range(1, 1000000).ToArray();
//Reverse sorted data:
int[] testData3 = Enumerable.Range(1, 1000000).OrderByDescending(x => x).ToArray();
//All zeros:
int[] testData4 = new int[1000000];
//All ones:
int[] testData5 = Enumerable.Repeat(1, 1000000).ToArray();

//bool test data for crystal ball problem.
bool[] testData6 = GenerateBoolTestData(100);

Algorithms.RunTest(Algorithms.BubbleSort, testData1);
//Algorithms.RunTest(Algorithms.BinarySearch, testData2, 6);
//Algorithms.RunTest(Algorithms.BinarySearch, testData3, 566);
//Algorithms.RunTest(Algorithms.BinarySearch, testData4, 1);
//Algorithms.RunTest(Algorithms.BinarySearch, testData5, 0);

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
            for(; i < breaks.Length; i+= jumpAmount)
            {
                if (breaks[i])
                    break;
            }

            i -= jumpAmount;

            for(int j = 0; j < jumpAmount && i < breaks.Length; ++j, ++i)
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
            for(int i = 0; i < arr.Length -1; ++i)
            {
                for(int j = 0; j < arr.Length - 1 - i; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        var temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j+ 1] = temp;   
                    }
                }
            }

            return arr;
        }

        #endregion
    }
}






