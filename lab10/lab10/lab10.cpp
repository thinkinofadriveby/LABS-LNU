#include<iostream>
#include<iomanip>
#include<stdlib.h>
#include<chrono>
#include<future>
#include<string>

using namespace std;

class Gauss
{
    
public: void GaussAlgorithm(int numberOfEquations)
    {
        float* a = new float[numberOfEquations, numberOfEquations];
        float* x = new float[numberOfEquations];
        float ratio;
        int i,j,k;
        cout<< setprecision(3)<< fixed;
    
        /* Gauss Elimination */
        for(i=1;i<=numberOfEquations-1;i++)
        {
            if(a[i,i] == 0.0)
            {
                cout<<"Computing error!";
                exit(0);
            }
            for(j=i+1;j<=numberOfEquations;j++)
            {
                ratio = a[j,i]/a[i,i];

                for(k=1;k<=numberOfEquations+1;k++)
                {
                    a[j,k] = a[j,k] - ratio*a[i,k];
                }
            }
        }
        /* Obtaining Solution by Back Substitution Method */
        x[numberOfEquations] = a[numberOfEquations,numberOfEquations+1]/a[numberOfEquations,numberOfEquations];

        for(i=numberOfEquations-1;i>=1;i--)
        {
            x[i] = a[i,numberOfEquations+1];
            for(j=i+1;j<=numberOfEquations;j++)
            {
                x[i] = x[i] - a[i,j]*x[j];
            }
            x[i] = x[i]/a[i,i];
        }
    }
void GenerateRandomMatrix(int numberOfEquations)
{
    int* arr = new int[numberOfEquations,numberOfEquations];
    srand(time(0));
    for (int i=0; i<numberOfEquations; i++)
        for (int j=0; j<numberOfEquations; j++)
            arr[i,j] = 1 + rand() % 100; 
}
};

int main()
{
    int numberOfEquations;
    cout<<"Enter number of equations: ";
    cin>>numberOfEquations;
    
    int numberOfThreads;
    cout<<"Enter number of threads: ";
    cin>>numberOfThreads;
    
    Gauss g;
    auto startTime = std::chrono::high_resolution_clock::now();
    g.GenerateRandomMatrix(numberOfEquations);
    g.GaussAlgorithm(numberOfEquations);
    auto endTime = std::chrono::high_resolution_clock::now();
    
    int elapsed_time_ms = std::chrono::duration<double, std::milli>(endTime-startTime).count();
    cout << "==========================";
    cout << "\nElapsed time for main thread: " << elapsed_time_ms << " ms";
    
    auto startTime1 = std::chrono::high_resolution_clock::now();
    
    for (int i = 0; i < numberOfThreads; ++i) {
        async(launch::async, [&g, &numberOfEquations]() -> void {
            g.GaussAlgorithm(numberOfEquations);
        });
    }
    auto endTime1 = std::chrono::high_resolution_clock::now();
    int elapsed_time_ms1 = std::chrono::duration<double, std::milli>(endTime1-startTime1).count();
    cout << "\nElapsed time for " + std::to_string(numberOfThreads) + " threads: " << elapsed_time_ms1 << " ms";
}