using System;
using System.Linq;

namespace statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] data = {
                {"StdNum", "Name", "Math", "Science", "English"},
                {"1001", "Alice", "85", "90", "78"},
                {"1002", "Bob", "92", "88", "84"},
                {"1003", "Charlie", "79", "85", "88"},
                {"1004", "David", "94", "76", "92"},
                {"1005", "Eve", "72", "95", "89"}
            };
            // You can convert string to double by
            // double.Parse(str)

            int stdCount = data.GetLength(0) - 1;
            
            int rangeOfstd = data.GetLength(0) - 1;
            int rangeOfsbj = data.GetLength(1) - 1;
            
            Console.WriteLine("Average Scores:");
            for (int i = 2; i <= rangeOfsbj; i++) {
                double avg = 0;
                for (int j = 1; j <= rangeOfstd; j++) {
                    avg += double.Parse(data[j, i]);
                }  
                Console.WriteLine($"{data[0, i]} : {(avg/stdCount):F2}");
            }
            Console.WriteLine("");
            
            
            Console.WriteLine("Max and min Scores:");
            for (int i = 2; i <= rangeOfsbj; i++) {
                double max = double.Parse(data[1, i]);
                double min = double.Parse(data[1, i]);
                for (int j = 1; j <= rangeOfstd; j++) {
                    if (double.Parse(data[j, i]) > max) {
                        max = double.Parse(data[j, i]);
                    }
                    if (double.Parse(data[j, i]) < min) {
                        min = double.Parse(data[j, i]);
                    }
                }
                Console.WriteLine($"{data[0, i]}: ({max}, {min})");
            }
            Console.WriteLine("");
            
            
            Console.WriteLine("Students rank by total scores:");
            double[] stdTotal = new double [stdCount];
            string[] stdName = new string [stdCount];
            for (int i = 1; i <= rangeOfstd; i++) {
                double total = 0;
                for (int j = 2; j<= rangeOfsbj; j++) {
                    total += double.Parse(data[i, j]);
                }
                stdTotal[i - 1] = total;
                stdName[i - 1] = data[i, 1];
            }

            for (int i = 0; i < stdCount; i++) {
                int cnt = 0;
                for (int j = 0; j < stdCount; j++) {
                    if (stdTotal[j] >= stdTotal[i]) {
                        cnt ++;
                    }
                }
                if (cnt == 1) {
                    Console.WriteLine($"{stdName[i]}: {cnt}st");
                }
                else if (cnt == 2) {
                    Console.WriteLine($"{stdName[i]}: {cnt}nd");
                }
                else {
                    Console.WriteLine($"{stdName[i]}: {cnt}th");
                }
            }
        }
    }
}

/* example output

Average Scores: 
Math: 84.40
Science: 86.80
English: 86.20

Max and min Scores: 
Math: (94, 72)
Science: (95, 76)
English: (92, 78)

Students rank by total scores:
Alice: 4th
Bob: 1st
Charlie: 5th
David: 2nd
Eve: 3rd

*/
