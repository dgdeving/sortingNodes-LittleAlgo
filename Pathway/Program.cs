using System;
using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;

namespace Pathway
{
    internal class Program
    {
        public static void PrintMatrix(double[,] matrix)
        {
            int size1 = matrix.GetLength(0);
            int size2 = matrix.GetLength(1);



            for (int i = 0; i < size1; i++)
            {
                for (int j = 0; j < size2; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static (double[,], double) ReduceMatrix(double[,] matrix)
        {
            int size = matrix.GetLength(0);

            // Declare a new matrix for the reduced matrix
            double[,] reducedMatrix = new double[size, size];

            // Before row and column reduction, declare a variable to hold the total reduction
            double totalReduction = 0;

            // 1. Row reduction: For each row, find the smallest element and subtract it from each element in its row
            for (int i = 0; i < size; i++)
            {
                double minRowValue = double.PositiveInfinity;
                for (int j = 0; j < size; j++)
                {
                    minRowValue = Math.Min(minRowValue, matrix[i, j]);
                }

                totalReduction += minRowValue;  // Add the minimum value for this row to the total reduction

                for (int j = 0; j < size; j++)
                {
                    reducedMatrix[i, j] = Math.Round(matrix[i, j] - minRowValue, 2);
                }
            }

            // 2. Column reduction: For each column, find the smallest element and subtract it from each element in its column
            for (int j = 0; j < size; j++)
            {
                double minColValue = double.PositiveInfinity;
                for (int i = 0; i < size; i++)
                {
                    minColValue = Math.Min(minColValue, reducedMatrix[i, j]);
                }

                totalReduction += minColValue;  // Add the minimum value for this column to the total reduction

                for (int i = 0; i < size; i++)
                {
                    reducedMatrix[i, j] = Math.Round(reducedMatrix[i, j] - minColValue, 2);

                }
            }
            Console.WriteLine("Reduced matrix by minimum row and column");
            return (reducedMatrix, totalReduction);
        }

        public static double[,] CalculatePenaltiesMatrix(double[,] matrix)
        {
            int size = matrix.GetLength(0);

            // Declare a new matrix to hold the penalties and initialize all cells to negative infinity
            double[,] penalties = new double[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    penalties[x, y] = double.NegativeInfinity;
                }
            }

            // For each cell in the matrix, calculate the penalty if cell value is zero
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j] == 0) // Process only zero cells
                    {
                        double rowMin = double.PositiveInfinity;
                        double colMin = double.PositiveInfinity;

                        // Find the minimum value in the row, excluding the cell itself
                        for (int k = 0; k < size; k++)
                        {
                            if (k != j) // Exclude the cell itself
                            {
                                rowMin = Math.Min(rowMin, matrix[i, k]);
                            }
                        }

                        // Find the minimum value in the column, excluding the cell itself
                        for (int k = 0; k < size; k++)
                        {
                            if (k != i) // Exclude the cell itself
                            {
                                colMin = Math.Min(colMin, matrix[k, j]);
                            }
                        }

                        // The penalty is the sum of the minimum values from the row and column
                        penalties[i, j] = rowMin + colMin;
                    }
                }
            }
            Console.WriteLine("Calculated penalties matrix at 0");

            return penalties;
        }

        public static (double, int, int) FindMaxPenalty(double[,] penalties)
        {
            double maxPenalty = double.MinValue;
            int maxPenaltyRow = -1;
            int maxPenaltyCol = -1;

            for (int i = 0; i < penalties.GetLength(0); i++)
            {
                for (int j = 0; j < penalties.GetLength(1); j++)
                {
                    if (penalties[i, j] > maxPenalty)
                    {
                        maxPenalty = penalties[i, j];
                        maxPenaltyRow = i;
                        maxPenaltyCol = j;
                    }
                }
            }

            Console.WriteLine("Calculated max penalty in matrix");

            return (maxPenalty, maxPenaltyRow, maxPenaltyCol);
        }

        public static double[,] RemoveRowAndColumnMatrix(double[,] matrix, int maxPenaltyRow, int maxPenaltyCol)
        {
            int size = matrix.GetLength(0);

            // Create a new matrix with reduced size (one less row and one less column)
            double[,] reducedMatrixByPenalty = new double[size - 1, size - 1];

            // Set the value at [maxPenaltyRow, maxPenaltyCol] to positive infinity in the original matrix
            matrix[maxPenaltyCol, maxPenaltyRow] = double.PositiveInfinity;

            int rowIndex = 0;
            for (int i = 0; i < size; i++)
            {
                if (i != maxPenaltyRow) // Exclude the maxPenaltyRow
                {
                    int colIndex = 0;
                    for (int j = 0; j < size; j++)
                    {
                        if (j != maxPenaltyCol) // Exclude the maxPenaltyCol
                        {
                            reducedMatrixByPenalty[rowIndex, colIndex] = matrix[i, j];
                            colIndex++;
                        }
                    }
                    rowIndex++;
                }
            }

            int newColIndex = 0;
            for (int j = 0; j < size; j++)
            {
                if (j != maxPenaltyCol) // Exclude the maxPenaltyCol
                {
                    newColIndex++;
                }
            }

            Console.WriteLine($"\nRemoved row and column ");
            return reducedMatrixByPenalty;
        }

        public static (Dictionary<int, int>, Dictionary<int, int>) BuildMappings(int maxPenaltyRow, int maxPenaltyCol, int matrixSize)
        {
            Dictionary<int, int> rowMapping = new Dictionary<int, int>();
            Dictionary<int, int> colMapping = new Dictionary<int, int>();

            int[] rowValues = Enumerable.Range(0, matrixSize).ToArray();
            int[] colValues = Enumerable.Range(0, matrixSize).ToArray();

            // Remove maxPenaltyRow value from rowValues
            rowValues = rowValues.Where((value, index) => index != maxPenaltyRow).ToArray();

            // Remove maxPenaltyCol value from colValues
            colValues = colValues.Where((value, index) => index != maxPenaltyCol).ToArray();

            for (int i = 0; i < matrixSize - 1; i++)
            {
                rowMapping[i] = rowValues[i];
                colMapping[i] = colValues[i];
            }

            return (rowMapping, colMapping);
        }

        public static (Dictionary<int, int>, Dictionary<int, int>) UpdateMappings(Dictionary<int, int> rowMapping, Dictionary<int, int> colMapping, int maxPenaltyRow, int maxPenaltyCol)
        {
            Dictionary<int, int> updatedRowMapping = new Dictionary<int, int>();
            Dictionary<int, int> updatedColMapping = new Dictionary<int, int>();

            int rowCounter = 0;
            int colCounter = 0;

            foreach (var entry in rowMapping)
            {
                if (entry.Value != maxPenaltyRow)
                {
                    updatedRowMapping[rowCounter] = entry.Value;
                    rowCounter++;
                }
            }

            foreach (var entry in colMapping)
            {
                if (entry.Value != maxPenaltyCol)
                {
                    updatedColMapping[colCounter] = entry.Value;
                    colCounter++;
                }
            }

            return (updatedRowMapping, updatedColMapping);
        }

        public static double[,] LoadMatrix(string path, int size)
        {
            FileInfo file = new FileInfo(path);
            double[,] matrix = new double[size, size];

            // Check if file exists
            if (!File.Exists(path))
            {
                Console.WriteLine("The file does not exist.");
                return null;
            }

            // Check if file is an .xlsx file
            if (Path.GetExtension(path) != ".xlsx")
            {
                Console.WriteLine("The file is not an .xlsx file.");
                return null;
            }

            // Initialize matrix with positive infinity for all elements
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = double.PositiveInfinity;
                }
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Add this line

            using (ExcelPackage package = new ExcelPackage(file))
            {
                if (package.Workbook.Worksheets.Count == 0)
                {
                    Console.WriteLine("The Excel file does not contain any worksheets.");
                    return null;
                }
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (i != j) // Skip when row number equals column number
                        {
                            matrix[i, j] = workSheet.Cells[i + 1, j + 1].GetValue<double>();
                        }
                        else
                        {
                            matrix[i, j] = double.PositiveInfinity; // or any default value
                        }
                    }
                }
            }

            return matrix;
        }




        static void Main(string[] args)
        {
            #region LoadingXLSX
            string path = @"Path";
            int size = 3; // change this to your required matrix size
            double[,] matrix = LoadMatrix(path, size);

            Console.WriteLine("Starting Matrix");

            PrintMatrix(matrix);

            #endregion LoadingXLSX

            #region FirstStep

            // Define rowMapping and colMapping dictionaries
            Dictionary<int, int> rowMapping = new Dictionary<int, int>();
            Dictionary<int, int> colMapping = new Dictionary<int, int>();

            // Reduce the matrix
            double totalReduction;
            (matrix, totalReduction) = ReduceMatrix(matrix);

            // Print the reduced matrix and total reduction
            PrintMatrix(matrix);
            Console.WriteLine($"Total Reduction: {totalReduction}\n");

            // Calculate the Penalties matrix
            double[,] penalitiesMatrix;
            penalitiesMatrix = CalculatePenaltiesMatrix(matrix);
            Console.WriteLine("Penalties Matrix: ");
            PrintMatrix(penalitiesMatrix);

            // Find the max penalty and its location by row and column 
            (double maxPenalty, int maxPenaltyRow, int maxPenaltyCol) = FindMaxPenalty(penalitiesMatrix);
            Console.WriteLine($"Maximum Penalty is at [{maxPenaltyRow},{maxPenaltyCol}] = {maxPenalty}");

            (rowMapping, colMapping) = BuildMappings(maxPenaltyRow, maxPenaltyCol, matrix.GetLength(0));

            // Reducing the matrix by removing the max penalty row and column
            matrix = RemoveRowAndColumnMatrix(matrix, maxPenaltyRow, maxPenaltyCol);
            PrintMatrix(matrix);

            // Print the reduced resulting matrix and total reduction
            double totalReduction2;
            (matrix, totalReduction2) = ReduceMatrix(matrix);

            // Print the reduced matrix and total reduction
            PrintMatrix(matrix);
            Console.WriteLine($"Total Reduction 2: {totalReduction2}\n");

            // Costul nefolosirii rutei definita de max penalty row and column  
            double costNotUsing = Math.Round((totalReduction + maxPenalty), 2);
            Console.WriteLine($"Cost not using arc [{maxPenaltyRow},{maxPenaltyCol}] = {costNotUsing}");

            // Costul folosirii rutei definita de max penalty row and column
            double costUsing = Math.Round((totalReduction + totalReduction2), 2);
            Console.WriteLine($"Cost using arc [{maxPenaltyRow},{maxPenaltyCol}] = {costUsing}");

            #endregion

            #region SecondStep

            double[,] resultingMatrix = matrix;
            double cost;
            bool shouldContinue = true;
            while (shouldContinue)
            {
                // Check if costFolosire <= costNefolosire
                if (costUsing <= costNotUsing)
                {
                    cost = costUsing;
                    Console.WriteLine("------------------------------------\nResulting Matrix");
                    PrintMatrix(resultingMatrix);

                    penalitiesMatrix = CalculatePenaltiesMatrix(resultingMatrix);
                    PrintMatrix(penalitiesMatrix);

                    (maxPenalty, maxPenaltyRow, maxPenaltyCol) = FindMaxPenalty(penalitiesMatrix);
                    Console.WriteLine($"Maximum Penalty is at [{maxPenaltyRow},{maxPenaltyCol}] = {maxPenalty} local");

                    costNotUsing = Math.Round(cost + maxPenalty, 2);

                    // Get the original row and column indices using the rowMapping and colMapping dictionaries
                    int originalMaxPenaltyRow = rowMapping[maxPenaltyRow];
                    int originalMaxPenaltyCol = colMapping[maxPenaltyCol];

                    (rowMapping, colMapping) = UpdateMappings(rowMapping, colMapping, originalMaxPenaltyRow, originalMaxPenaltyCol);

                    resultingMatrix = RemoveRowAndColumnMatrix(resultingMatrix, maxPenaltyRow, maxPenaltyCol);
                    PrintMatrix(resultingMatrix);

                    (resultingMatrix, totalReduction) = ReduceMatrix(resultingMatrix);
                    PrintMatrix(resultingMatrix);
                    Console.WriteLine($"Total Reduction: {totalReduction}\n");

                    costUsing = Math.Round(cost + totalReduction, 2);

                    Console.WriteLine($"Cost folosire [{originalMaxPenaltyRow},{originalMaxPenaltyCol}] = {costUsing} global");
                    Console.WriteLine($"Cost nefolosire [{originalMaxPenaltyRow},{originalMaxPenaltyCol}] = {costNotUsing} global");



                }
                else
                {
                    int opposedMaxPenaltyRow = maxPenaltyCol;
                    int opposedMaxPenaltyCol = maxPenaltyRow;

                    // Assign +Infinity to the opposed (maxPenaltyRow, maxPenaltyCol) value
                    resultingMatrix[opposedMaxPenaltyRow, opposedMaxPenaltyCol] = double.PositiveInfinity;

                    penalitiesMatrix = CalculatePenaltiesMatrix(matrix);
                    Console.WriteLine("Penalties Matrix: ");
                    PrintMatrix(penalitiesMatrix);

                    // Find the max penalty and its location by row and column 
                    (maxPenalty, maxPenaltyRow, maxPenaltyCol) = FindMaxPenalty(penalitiesMatrix);
                    Console.WriteLine($"Maximum Penalty is at [{maxPenaltyRow},{maxPenaltyCol}] = {maxPenalty}");

                    (rowMapping, colMapping) = BuildMappings(maxPenaltyRow, maxPenaltyCol, matrix.GetLength(0));

                    // Reducing the matrix by removing the max penalty row and column
                    matrix = RemoveRowAndColumnMatrix(matrix, maxPenaltyRow, maxPenaltyCol);
                    PrintMatrix(matrix);

                    // Print the reduced resulting matrix and total reduction
                    (matrix, totalReduction2) = ReduceMatrix(matrix);

                    // Print the reduced matrix and total reduction
                    PrintMatrix(matrix);
                    Console.WriteLine($"Total Reduction 2: {totalReduction2}\n");

                    // Costul nefolosirii rutei definita de max penalty row and column  
                    costNotUsing = Math.Round((totalReduction + maxPenalty), 2);
                    Console.WriteLine($"Cost not using arc [{maxPenaltyRow},{maxPenaltyCol}] = {costNotUsing}");

                    // Costul folosirii rutei definita de max penalty row and column
                    costUsing = Math.Round((totalReduction + totalReduction2), 2);
                    Console.WriteLine($"Cost using arc [{maxPenaltyRow},{maxPenaltyCol}] = {costUsing}");

                }

                if (resultingMatrix.GetLength(0) < 2)
                {
                    shouldContinue = false;
                }

            }

            #endregion

        }
    }
}
