# sortingNodes-LittleAlgo

## Description

This repository contains a C# application for performing matrix reduction and building mappings based on specific algorithms. The application is designed to read a square matrix from an Excel file, perform row and column reductions, calculate penalties, find the maximum penalty, and update mappings accordingly.

## Methods

### 1. `PrintMatrix(double[,] matrix)`

This method prints a given 2D matrix to the console in a human-readable format.

### 2. `ReduceMatrix(double[,] matrix)`

This method reduces the input matrix by finding the minimum value in each row and column and subtracting it from all elements in that row and column. It returns the reduced matrix and the total reduction value.

### 3. `CalculatePenaltiesMatrix(double[,] matrix)`

This method calculates a penalties matrix based on the input matrix. It identifies zero cells in the matrix and computes penalties by finding the minimum values in their corresponding row and column. The resulting penalties matrix contains the sum of the row and column minimums for each zero cell.

### 4. `FindMaxPenalty(double[,] penalties)`

This method identifies the cell with the maximum penalty in the given penalties matrix and returns the value of the maximum penalty along with its row and column indices.

### 5. `RemoveRowAndColumnMatrix(double[,] matrix, int maxPenaltyRow, int maxPenaltyCol)`

This method removes a specified row and column from the given matrix. It is used to reduce the matrix size after identifying the maximum penalty.

### 6. `BuildMappings(int maxPenaltyRow, int maxPenaltyCol, int matrixSize)`

This method builds two mappings (dictionaries) that help keep track of the original indices of the rows and columns. It is used to map the reduced matrix elements back to their original positions.

### 7. `UpdateMappings(Dictionary<int, int> rowMapping, Dictionary<int, int> colMapping, int maxPenaltyRow, int maxPenaltyCol)`

This method updates the mappings (dictionaries) by excluding the indices of the removed row and column from the previous mappings. It is used after removing a row and column to keep the mappings consistent.

### 8. `LoadMatrix(string path, int size)`

This method reads a square matrix of the given size from an Excel (.xlsx) file. The input file path is provided, and the method returns the matrix as a 2D array of doubles.

## Usage

To use this application, you need to include the provided methods in your C# project. The primary functionalities are related to matrix reduction and mapping. You can call the methods in sequence to perform the following steps:

1. Load the input matrix from an Excel file using `LoadMatrix`.
2. Reduce the matrix by minimum row and column using `ReduceMatrix`.
3. Calculate penalties for zero cells using `CalculatePenaltiesMatrix`.
4. Find the maximum penalty using `FindMaxPenalty`.
5. Remove the row and column with the maximum penalty using `RemoveRowAndColumnMatrix`.
6. Build and update mappings using `BuildMappings` and `UpdateMappings`.

Please make sure to handle any exceptions and error conditions that may arise during the execution of the methods.

Feel free to explore and customize the application according to your specific use case.

## Note

- Ensure that the required libraries or dependencies are properly referenced in your project to use the provided methods.
- The application is designed for square matrices (size x size) with numeric values in an Excel file.
- Always provide valid input files and appropriate matrix sizes to avoid errors.
