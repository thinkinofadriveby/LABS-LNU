#include <iostream>
#include <fstream>
#include <mutex>
#include <thread>
using namespace std;

mutex mut;
void CountNumberOfWordsInFile(string filename)
{
    mut.lock();
    ifstream fin(filename);

    int word = 1; //will not count first word so initial value is 1
    char ch;
    fin.seekg(0,ios::beg); //bring position of file pointer to beginning of file

    while(fin)
    {
        fin.get(ch);
        if(ch == ' '|| ch == '\n')
            word++;
    }
//    cout << "Words in file: " << word;
    fin.close();
    mut.unlock();
}

int main(int argc, char* argv[])
{
    cout << "Input the path of the path: ";
    string path;
    cin >> path;

    int numberOfThreads;
    cout << "Input number of threads: ";
    cin >> numberOfThreads;

    auto startTime = std::chrono::high_resolution_clock::now();
    for (int i = 0; i < numberOfThreads; i++) {
        CountNumberOfWordsInFile(path);
    }
    auto endTime = std::chrono::high_resolution_clock::now();
    int elapsed_time_ms = std::chrono::duration<double, std::milli>(endTime-startTime).count();
    cout << "\nElapsed time for main thread: " << elapsed_time_ms << " ms";

    auto startTime1 = std::chrono::high_resolution_clock::now();

    thread* myThreads = new thread[numberOfThreads];
    for(int i = 0; i < numberOfThreads; i++)
    {
        myThreads[i] = thread(CountNumberOfWordsInFile, path);
    }

        for (int i = 0; i < numberOfThreads; i++)
        {
            std::thread::id threadId = myThreads[i].get_id();
            int ThreadIdAsInt = *static_cast<int*>(static_cast<void*>(&threadId));
            if ( ThreadIdAsInt % 2 == 0)
            {
                myThreads[i].join();
                cout << "\n[!] MULTITHREADING: Run " << threadId << " thread";
            }
        }
        for (int k = 0; k < numberOfThreads; k++)
        {
            std::thread::id threadId = myThreads[k].get_id();
            int ThreadIdAsInt = *static_cast<int*>(static_cast<void*>(&threadId));
            if ( ThreadIdAsInt % 2 == 1)
            {
                myThreads[k].join();
                cout << "\n[!] MULTITHREADING: Run " << threadId << + " thread";
            }
        }
    auto endTime1 = std::chrono::high_resolution_clock::now();
    int elapsed_time_ms1 = std::chrono::duration<double, std::milli>(endTime1-startTime1).count();
    cout << "\nElapsed time for " + std::to_string(numberOfThreads) + " threads: " << elapsed_time_ms1 << " ms";
    return 0;
}

