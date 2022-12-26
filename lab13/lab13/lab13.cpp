#include <mpi.h>
#include <iostream>
#include <vector>
#include <algorithm>
#include <ctime>
#include <chrono>
#include <thread>
#include <string>
using namespace std;


void fill_row(std::vector<int>& row)
{
    generate(row.begin(), row.end(), []() { return rand() % 10; });
}

void fill_matrix(std::vector<vector<int>>& matrix)
{
    for_each(matrix.begin(), matrix.end(), fill_row);
}

void MultiplyMatrices(vector<vector<int>>& A, vector<vector<int>>& B, vector<vector<int>>& C, int size)
{
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            for (int k = 0; k < size; k++)
            {
                C[i][j] += A[i][k] * B[k][j];
            }
        }
    }
}

int main(int argc, char** argv) {

    int size;
    cout << "Input size:\n";
    cin >> size;

    int numberOfThreads;
    cout << "Input number of threads:\n";
    cin >> numberOfThreads;

    vector<vector<int>> A(size, vector<int>(size, 0));
    fill_matrix(A);

    vector<vector<int>> B(size, vector<int>(size, 0));
    fill_matrix(B);

    vector<vector<int>> C(size, vector<int>(size, 0));
    fill_matrix(C);

    auto startTime1 = std::chrono::high_resolution_clock::now();
    std::thread* myThreads = new thread[numberOfThreads];
    for (int i = 0; i < numberOfThreads; i++)
    {
        myThreads[i] = thread(MultiplyMatrices, A, B, C, size);
        myThreads[i].join();
    }
    auto endTime1 = chrono::high_resolution_clock::now();
    int elapsed_time_ms1 = chrono::duration<double, std::milli>(endTime1 - startTime1).count();
    cout << "\nElapsed time for " + to_string(numberOfThreads) + " threads: " << elapsed_time_ms1 << " ms";

    // Initialize the MPI environment
    MPI_Init(NULL, NULL);

    // Get the number of processes
    int world_size;
    MPI_Comm_size(MPI_COMM_WORLD, &world_size);

    // Get the rank of the process
    int world_rank;
    MPI_Comm_rank(MPI_COMM_WORLD, &world_rank);

    // Get the name of the processor
    char processor_name[MPI_MAX_PROCESSOR_NAME];
    int name_len;
    MPI_Get_processor_name(processor_name, &name_len);

    // Print off a hello world message

    MultiplyMatrices(A, B, C, size);

    printf("Method MultiplyMatrices executed from processor %s, rank %d out of %d processors\n",
        processor_name, world_rank, world_size);

    // Finalize the MPI environment.
    MPI_Finalize();
}