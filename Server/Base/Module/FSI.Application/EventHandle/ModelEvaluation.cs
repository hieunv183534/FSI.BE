using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.EventHandle
{
    public class ModelEvaluation
    {
        public static double CalculatePrecision(double[] actualRatings, double[] predictedRatings, int k, double threshold = 2.0)
        {
            // Kiểm tra số lượng phần tử của hai mảng dữ liệu
            if (actualRatings.Length != predictedRatings.Length)
            {
                throw new ArgumentException("Hai mảng dữ liệu không có cùng số lượng phần tử.");
            }

            // Tạo mảng chỉ số sắp xếp dự đoán giảm dần
            int[] sortedIndices = Enumerable.Range(0, predictedRatings.Length)
                                           .OrderByDescending(i => predictedRatings[i])
                                           .ToArray();

            // Lấy k giá trị dự đoán đầu tiên
            int[] topKIndices = sortedIndices.Take(k).ToArray();

            // Đếm số lượng dự đoán đúng trong top k
            int correctPredictions = 0;
            foreach (int index in topKIndices)
            {
                if (predictedRatings[index] >= threshold && actualRatings[index] >= threshold)
                {
                    correctPredictions++;
                }
            }

            // Tính precision
            double precision = (double)correctPredictions / k;
            return precision;
        }

        public static double CalculateRecall(double[] actualRatings, double[] predictedRatings, int k, double threshold = 2.0)
        {
            // Kiểm tra số lượng phần tử của hai mảng dữ liệu
            if (actualRatings.Length != predictedRatings.Length)
            {
                throw new ArgumentException("Hai mảng dữ liệu không có cùng số lượng phần tử.");
            }

            // Tạo mảng chỉ số sắp xếp dự đoán giảm dần
            int[] sortedIndices = Enumerable.Range(0, predictedRatings.Length)
                                           .OrderByDescending(i => predictedRatings[i])
                                           .ToArray();

            // Lấy k giá trị dự đoán đầu tiên
            int[] topKIndices = sortedIndices.Take(k).ToArray();

            // Đếm số lượng mục thực tế đúng trong top k
            int truePositives = 0;
            for (int i = 0; i < k; i++)
            {
                int index = topKIndices[i];
                if (predictedRatings[index] >= threshold && actualRatings[index] >= threshold)
                {
                    truePositives++;
                }
            }

            // Tính recall
            int totalPositive = actualRatings.Count(rating => rating >= threshold);
            double recall = (double)truePositives / totalPositive;
            return recall;
        }

        public static double CalculateRMSE(double[] actualRatings, double[] predictedRatings)
        {
            double sumSquaredError = 0.0;

            for (int i = 0; i < actualRatings.Length; i++)
            {
                double error = predictedRatings[i] - actualRatings[i];
                sumSquaredError += Math.Pow(error, 2);
            }

            double rmse = Math.Sqrt(sumSquaredError / actualRatings.Length);
            return rmse;
        }

        public static double CalculateMAE(double[] actualRatings, double[] predictedRatings)
        {
            double sumAbsoluteError = 0.0;

            for (int i = 0; i < actualRatings.Length; i++)
            {
                double error = predictedRatings[i] - actualRatings[i];
                sumAbsoluteError += Math.Abs(error);
            }

            double mae = sumAbsoluteError / actualRatings.Length;
            return mae;
        }
    }
}
