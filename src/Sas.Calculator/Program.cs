// See https://aka.ms/new-console-template for more information
using Sas.Calculator;

Console.WriteLine("Hello, World!");

Vector v1 = new Vector(11, 13, 17);
Vector v2 = new Vector(3, 7, 11);

Matrix matrix1 = new Matrix(1, 3, 4, 0, 3, 0, 2 ,-2, -3);

Matrix matrix2 = new Matrix(1, 2, 3, 0, 1, 5, 5, 6, 0);
Console.WriteLine(matrix2);
matrix2.Invert();
Console.WriteLine(matrix2);

Console.WriteLine();