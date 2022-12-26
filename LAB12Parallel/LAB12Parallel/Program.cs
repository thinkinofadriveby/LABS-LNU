using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCL.Net.Extensions;
using OpenCL.Net;
using System.Diagnostics;

namespace LAB12Parallel
{
    class Class1
    {

        public static void MultiplyMatrices(int[,] A, int[,] B, int[,] C, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        C[i,j] += A[i,k] * B[k,j];
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("\nRANDOM MATRICES MULTIPLICATION: ");
            Console.Write("Input size of matrix: ");
            int count = Convert.ToInt32(Console.ReadLine());
            
            
            int[,] A = new int[count, count];
            int[,] B = new int[count, count];
            int[,] res = new int[count, count];
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    A[i, j] = rnd.Next(10);
                    B[i, j] = rnd.Next(10);
                    res[i, j] = 0;
                }
            }

            Stopwatch sw1 = new Stopwatch(),sw2=new Stopwatch();
            sw1.Start();
            MultiplyMatrices(A, B, res, count);
            sw1.Stop();
            Console.WriteLine("\nMain program time: " +
                "{2} minutes {0} seconds and {1} milliseconds",
                sw1.Elapsed.Seconds, sw1.Elapsed.Milliseconds,sw1.Elapsed.Minutes);
            
            sw2.Start();
            
            Event event0; ErrorCode err;
            Platform[] platforms = Cl.GetPlatformIDs(out err);
            Device[] devices = Cl.GetDeviceIDs(platforms[0], DeviceType.Gpu, out err);
            Device device = devices[0]; 
            Context context = Cl.CreateContext(null, 1, devices, null, IntPtr.Zero, out err);
            CommandQueue cmdQueue = Cl.CreateCommandQueue(context, device, CommandQueueProperties.None, out err);

            
            string programSource = @"
                __kernel void doubleMe(__global int* inputA,__global int* inputB, __global int* output,int n) 
                { 
                    int i = get_global_id(0);
                    int j = get_global_id(1);
                    if (i < n && j < n)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            output[i * n + j] += inputA[i * n + k] * inputB[k * n + j];
                        }
                    }      
                };";
            Program program = Cl.CreateProgramWithSource(context, 1, new[] { programSource }, null, out err);
            Cl.BuildProgram(program, 0, null, string.Empty, null, IntPtr.Zero); 

            
            if (Cl.GetProgramBuildInfo(program, device, ProgramBuildInfo.Status, out err).CastTo<BuildStatus>() != BuildStatus.Success)
            {
                if (err != ErrorCode.Success)
                    Console.WriteLine("ERROR: " + "Cl.GetProgramBuildInfo" + " (" + err.ToString() + ")");
                Console.WriteLine("Cl.GetProgramBuildInfo != Success");
                Console.WriteLine(Cl.GetProgramBuildInfo(program, device, ProgramBuildInfo.Log, out err));
            }


            
            Kernel kernel = Cl.CreateKernel(program, "doubleMe", out err);


            
            Mem memInputA = (Mem)Cl.CreateBuffer(context, MemFlags.ReadOnly, sizeof(int) * count * count, out err);
            Mem memInputB = (Mem)Cl.CreateBuffer(context, MemFlags.ReadOnly, sizeof(int) * count * count, out err);

            
            Mem memoutput = (Mem)Cl.CreateBuffer(context, MemFlags.WriteOnly, sizeof(int) * count * count, out err);
            int[] a = new int[count * count], b = new int[count * count], c = new int[count * count];
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    a[i * count + j] = A[i,j];
                    b[i * count + j] = B[i,j];
                    c[i * count + j] = 0;
                }
            }
            
            Cl.EnqueueWriteBuffer(cmdQueue, (IMem)memInputA, Bool.True, IntPtr.Zero, new IntPtr(sizeof(int) * count * count), a, 0, null, out event0);
            Cl.EnqueueWriteBuffer(cmdQueue, (IMem)memInputB, Bool.True, IntPtr.Zero, new IntPtr(sizeof(int) * count * count), b, 0, null, out event0);
            Cl.EnqueueWriteBuffer(cmdQueue, (IMem)memoutput, Bool.True, IntPtr.Zero, new IntPtr(sizeof(int) * count * count), c, 0, null, out event0);

            IntPtr[] localWorkSize = new IntPtr[2], globalWorkSize = new IntPtr[2];
            globalWorkSize[0] = new IntPtr(count);
            globalWorkSize[1] = new IntPtr(count);
            localWorkSize[0] = new IntPtr(sizeof(int));
            localWorkSize[1] = new IntPtr(sizeof(int));
            
            Cl.SetKernelArg(kernel, 0, new IntPtr(4), memInputA);
            Cl.SetKernelArg(kernel, 1, new IntPtr(4), memInputB);
            Cl.SetKernelArg(kernel, 2, new IntPtr(4), memoutput);
            Cl.SetKernelArg(kernel, 3, new IntPtr(4), count);
            IntPtr[] workGroupSizePtr = new IntPtr[] { new IntPtr(count) }; 
            Cl.EnqueueNDRangeKernel(cmdQueue, kernel, 2, null, workGroupSizePtr, localWorkSize, 0, null, out event0);

            
            Cl.Finish(cmdQueue);

            Cl.EnqueueReadBuffer(cmdQueue, (IMem)memoutput, Bool.True, IntPtr.Zero, new IntPtr(count *count* sizeof(int)), c, 0, null, out event0);

            sw2.Stop();
            Console.WriteLine("OpenCl time: " +
                "{2} minutes {0} seconds and {1} milliseconds",
                sw2.Elapsed.Seconds, sw2.Elapsed.Milliseconds,sw2.Elapsed.Minutes);
            
   
            
        }
    }
}
