using System;
using System.IO;

namespace Sorting
{
    class Program
    {

        static Boolean optimised = false;
        static void Main(string[] args)
        {
            string[] names = CreateArray();
            string[] temp = new string[names.Length];
            Array.Copy(names, temp, names.Length);
            var watch = new System.Diagnostics.Stopwatch();//storing elapsed time using the built in diagnostics

            while (true) //loop for ever OR until 6 chosen
            {
                switch (menu())
                {
                    case 1:
                        {
                            print_array(temp);
                            break;
                        }
                    case 2:
                        {
                            watch.Start();
                            insertionSort(temp);
                            watch.Stop();//stop timer
                            Array.Copy(names,temp,names.Length);//reload unsorted data
                            break;
                        }
                    case 3:
                        {
                            watch.Start();
                            bubbleSort(temp);
                            watch.Stop();//stop timer
                            Array.Copy(names, temp, names.Length);//reload unsorted data
                            break;
                        }
                    case 4:
                        {
                            watch.Start();
                            MergeSort(temp, 0, temp.Length - 1);
                            watch.Stop();
                            Array.Copy(names, temp, names.Length);
                            break;
                        }
                    case 5:
                        {
                            optimised = true;
                            watch.Start();
                            bubbleSort(temp);
                            watch.Stop();//stop timer
                            Array.Copy(names, temp, names.Length);//reload unsorted data
                            break;
                        }
                    case 6:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
                Console.WriteLine($"\n Execution Time: {watch.ElapsedMilliseconds} ms");
                watch.Reset();
            }


        }
        static int menu()
        {
            int choice = 0;
            while (choice < 1 || choice > 6)
            {
                Console.WriteLine("1. Print the array so you can see it's random");
                Console.WriteLine("2. Insertion Sort");
                Console.WriteLine("3. Bubble Sort");
                Console.WriteLine("4. Merge Sort");
                Console.WriteLine("5. Optimised Bubble Sort");
                Console.WriteLine("6. Exit");
                try
                { choice = Int32.Parse(Console.ReadLine()); }
                catch
                {
                    Console.WriteLine("Try again, this time please enter a number. Enter to continue.");
                    Console.ReadLine();
                }
                Console.Clear();
            }
            return choice;
        }
        static string[] CreateArray()
        {
            int qty = 0;
            Console.WriteLine("The program will generate an array of random names. You can then choose different sorting algoritms to apply ");
            Console.WriteLine("Very large numbers (greater than 50,000) may take a long time to sort");
            Console.WriteLine("How many names do you want (minimum 10, maximum 100,000)?");
            Console.WriteLine("If you choose a value outside this you will get 10 names");
            
            try { qty = Int32.Parse(Console.ReadLine()); }
            catch { Console.WriteLine("OK, you get 10 names \n"); }

            if (qty < 10 || qty > 100000)
            {
                Console.WriteLine("OK, you get 10 names \n");
                qty = 10; 
            }
            string[] names = new string[qty];
            Random randFirst = new Random();
            Random randLast = new Random();
            var firstLines = File.ReadAllLines("../../../Resources/firstNames.txt");
            var lastLines = File.ReadAllLines("../../../Resources/surNames.txt");
            for (int i = 0; i < qty; i++)
            {
                names[i] = firstLines[randFirst.Next(0, firstLines.Length)] + " " + lastLines[randLast.Next(0, lastLines.Length)];
            }

            return names;
        }
        static void print_array(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(i.ToString() + ". " + arr[i]);
            }
        }
        static Boolean compareStrings(string str1, string str2) //returns true if str1 < str2
        {
            int comp = string.Compare(str1, str2);
            if (comp == -1)
            { return true; }
            else
            { return false; }
        }
        // BUBBLE SORT...O(n^2) because of the nested loops
        static void bubbleSort(string[] arr)
        {

            string temp;
            for (int i = 0; i <= arr.Length - 2; i++)//iterate through the array

            {
                Boolean swapped = false; //only used in the optimised version
                for (int j = 0; j <= arr.Length - 2 - i; j++) //minor optimisation - reducing the step each time to avoid looking at bits which we already sorted
                {
                    if (compareStrings(arr[j + 1], arr[j]))
                    {
                        temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        swapped = true; //only used in the optimised version
                    }

                }
                if(!swapped && optimised) //check to see if the optimised version is being used
                    
                  {
                    optimised = false; //reset the boolean
                    return; //we're done - return
                }
            }
        }
        //INSERTION SORT...O(n^2) because of the nested loops
        static void insertionSort(string[] arr)
        {

            string temp; //used for our value that we are going to swap
            Boolean done; // a flag to indicate if we have put the item in the correct place. We can stop when done
            for (int i = 1; i < arr.Length; i++) //NOTE we start at item 1 because item 0 is considered to be in the correct place already
            {
                temp = arr[i];
                done = false;
                for (int j = i - 1; j >= 0 && !done; j--) //loop as long as we still have items to compare (j hasn't reached -1) and we are not done comparing
                {
                    if (compareStrings(temp, arr[j])) //is the current item smaller than the next item in the j loop
                    {
                        arr[j + 1] = arr[j]; //swap - note one less step here then bubble sort because we already put the current item in temp at line 41
                        arr[j] = temp;

                    }
                    else
                    {
                        done = true; //if the current item is bigger than the current item in the j loop it is in the right place and we are done
                    }
                }
            }
        }
        //MERGE SORT .....O(nlog n)
       static public string[] MergeSort(string[] array, int left, int right)//Merge sort recursive routine- needs MergeArray to rebuild.
        {
            if (left < right)
            {
                int middle = left + (right - left) / 2;

                MergeSort(array, left, middle);
                MergeSort(array, middle + 1, right);

                MergeArray(array, left, middle, right);
            }

            return array;
        }
       static public void MergeArray(string[] array, int left, int middle, int right)
        {
            var leftArrayLength = middle - left + 1;
            var rightArrayLength = right - middle;
            var leftTempArray = new string[leftArrayLength];
            var rightTempArray = new string[rightArrayLength];
            int i, j;

            for (i = 0; i < leftArrayLength; ++i)
                leftTempArray[i] = array[left + i];
            for (j = 0; j < rightArrayLength; ++j)
                rightTempArray[j] = array[middle + 1 + j];

            i = 0;
            j = 0;
            int k = left;

            while (i < leftArrayLength && j < rightArrayLength)
            {
                if (compareStrings(leftTempArray[i], rightTempArray[j]))
                {
                    array[k++] = leftTempArray[i++];
                }
                else
                {
                    array[k++] = rightTempArray[j++];
                }
            }

            while (i < leftArrayLength)
            {
                array[k++] = leftTempArray[i++];
            }

            while (j < rightArrayLength)
            {
                array[k++] = rightTempArray[j++];
            }
        }
    }
}