using System;
using System.Linq;

namespace GaussAlgorithm;

public class Solver
{
    public double[] Solve(double[][] matrix, double[] freeMembers)
    {
        const double eps = 1e-9;

        int rows = matrix.Length;
        int cols = matrix[0].Length;
        var a = matrix
            .Select((row, i) => row.Concat(new[] { freeMembers[i] }).ToArray())
            .ToArray();

        var where = Enumerable.Repeat(-1, cols).ToArray();
        int row = 0;

        for (int col = 0; col < cols && row < rows; col++)
        {
            int sel = Enumerable.Range(row, rows - row)
                .FirstOrDefault(r => Math.Abs(a[r][col]) > eps);

            if (Math.Abs(a[sel][col]) < eps)
                continue;

            (a[row], a[sel]) = (a[sel], a[row]);
            where[col] = row;

            double div = a[row][col];
            for (int j = col; j <= cols; j++)
                a[row][j] /= div;

            for (int i = 0; i < rows; i++)
            {
                if (i == row) continue;
                double factor = a[i][col];
                for (int j = col; j <= cols; j++)
                    a[i][j] -= factor * a[row][j];
            }

            row++;
        }

        for (int i = 0; i < rows; i++)
        {
            bool allZero = Enumerable.Range(0, cols).All(j => Math.Abs(a[i][j]) < eps);
            if (allZero && Math.Abs(a[i][cols]) > eps)
                throw new NoSolutionException("1213");
        }

        var ans = new double[cols];
        for (int i = 0; i < cols; i++)
        {
            if (where[i] != -1)
                ans[i] = a[where[i]][cols];
            else
                ans[i] = 0;
        }

        return ans;
    }
}
