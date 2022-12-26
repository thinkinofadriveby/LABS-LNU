#include <iostream>
#include <stdlib.h>
#include <chrono>
#include <thread>
#include <format>
#include <string>
#include <vector>

using namespace std;

static void FindSmallestAndLargestNumbersInRandomMatrix(int size)
{
    int* matrix = new int[size * size];
    
    srand(time(0));
    
    for(int i = 0; i < size; i++)
    {
        for(int j = 0; j < size; j++)
        {
            matrix[i * j] = 1 + rand() % 250;
        }
    }

    // output matrix
    
    // for(int i = 0; i < size; i++)
    // {
    //     for(int j = 0; j < size; j++)
    //     {
    //         cout << matrix[i * j] << "\t";
    //     }
    // }
    
    int Min, Max;
    Max = matrix[0 * 0];
    Min = matrix[0 * 0];
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            if (matrix[i * j] > Max)
            {
                Max = matrix[i * j];
            }
            if (matrix[i * j] < Min)
            {
                Min = matrix[i * j];
            }
        }
    }
    // cout << "Max value: " << Max << endl;
    // cout << "Min value: " << Min << endl;
}
int main()
{
    int size;
    cout << "Input size of matrix: ";
    cin >> size;

    int numberOfThreads;
    cout << "Input number of threads: ";
    cin >> numberOfThreads;
    
    auto startTime = std::chrono::high_resolution_clock::now();
    FindSmallestAndLargestNumbersInRandomMatrix(size);
    auto endTime = std::chrono::high_resolution_clock::now();
    
    int elapsed_time_ms = std::chrono::duration<double, std::milli>(endTime-startTime).count();
    cout << "==========================";
    cout << "\nElapsed time for main thread: " << elapsed_time_ms << " ms";

    auto startTime1 = std::chrono::high_resolution_clock::now();
    
    std::thread* myThreads = new thread[numberOfThreads];
    for(int i = 0; i < numberOfThreads; i++)
    {
        myThreads[i] = std::thread(FindSmallestAndLargestNumbersInRandomMatrix, i);
        myThreads[i].join();
    }
    auto endTime1 = std::chrono::high_resolution_clock::now();
    int elapsed_time_ms1 = std::chrono::duration<double, std::milli>(endTime1-startTime1).count();
    cout << "\nElapsed time for " + std::to_string(numberOfThreads) + " threads: " << elapsed_time_ms1 << " ms";
}
