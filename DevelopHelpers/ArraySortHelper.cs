using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevelopHelpers
{
    public static class ArraySortHelper
    {
        /// <summary>
        /// 冒泡排序法（从大到小）
        /// </summary>
        /// <param name="data"></param>
        public static void BubbleSort(this double[] data)
        {
            if (data != null && data.Length > 0)
            {
                for (int i = 0; i < data.Length - 1; i++)
                {
                    for (int j = data.Length - 1; j > i; j--)
                    {
                        if (data[j] > data[j - 1])
                        {
                            data[j] = data[j] + data[j - 1];
                            data[j - 1] = data[j] - data[j - 1];
                            data[j] = data[j] - data[j - 1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 插入排序法(从小到大)
        /// </summary>
        /// <param name="data"></param>
        public static void InsertSort(this IList<int> data)
        {
            if (data != null && data.Count > 0)
            {
                int temp;
                for (int i = 1; i < data.Count; i++)
                {
                    temp = data[i];
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (data[j] > temp)
                        {
                            data[j + 1] = data[j];
                            if (j == 0)
                            {
                                data[0] = temp;
                                break;
                            }
                        }
                        else
                        {
                            data[j + 1] = temp;
                            break;
                        }
                    }
                }
            }
        }
    }
}
