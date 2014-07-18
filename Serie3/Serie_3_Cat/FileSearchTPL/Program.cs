using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace FileSearchTPL
{
    class Program
    {
        /*
        Exercício 3:
        Implemente em C#, e usando a framework TPL, um método utilitário para pesquisa de ficheiros que 
        contenham uma sequência arbitrária de caracteres. O método recebe como argumentos: o caminho 
        absoluto da pasta raiz da pesquisa, a extensão dos ficheiros a considerar, e a sequência de caracteres a 
        pesquisar nos ficheiros considerados. A pesquisa é realizada assincronamente tirando partido da 
        eventual existência de vários processadores e é passível de ser cancelada. O resultado da pesquisa 
        contém a lista dos nomes dos ficheiros (i.e. caminhos absolutos) que cumprem os critérios de 
        pesquisa, o número de ficheiros encontrados com a extensão especificada e o número total de ficheiros 
        encontrados.*/
        private static volatile List<string> finalResult;
        static void Main(string[] args)
        {
            finalResult = new List<string>();
            if (args.Length < 3)
            {
                Console.WriteLine("Invalid number of arguments! Press any key to exit...");
                Console.ReadLine();
                return;
            }

            startSearch(args);
            Console.ReadKey();
        }

        // arguments:
        //  - search root                   // args[1]
        //  - searchable files extension    // args[2]
        //  - char sequence to search       // args[3]

        public static async void startSearch(String[] args)
        {
            // Call the method
            string searchRoot = args[0];
            string fileExtension = args[1];
            string charSequence = args[2];
            Task[] tasks = null;
            var t = new Task<List<string>>(() =>
            {
                var result = new List<string>();
                int nCPU = Environment.ProcessorCount;
                var searchableFiles = new List<string>(Directory.GetFiles(searchRoot, "*." + fileExtension, SearchOption.AllDirectories));
                int maximumFilesPerCpu = searchableFiles.Count / nCPU;

                if (searchableFiles.Count < nCPU)
                {
                    nCPU = searchableFiles.Count;
                    maximumFilesPerCpu = 1;
                }

                tasks = new Task<List<string>>[nCPU];

                for (var i = 0; i < nCPU; i++)
                {
                    //TODO Define the files that each task will be responsible for examining!
                    List<String> files = new List<string>(maximumFilesPerCpu);

                    int j = 0;
                    while (searchableFiles.Count > 0)
                    {
                        files.Add(searchableFiles.ToArray().GetValue(0) as string);
                        searchableFiles.RemoveAt(0);
                        if (j++ == maximumFilesPerCpu) break;
                    }

                    // Define the action that will be associated to each of the tasks
                    tasks[i] = new Task<List<string>>(
                        () => files.Count > 0 ? SearchFileWithCharSequence(files, charSequence) : null);
                    tasks[i].Start();
                }

                // Wait for all the tasks to finish their work
                Task.WaitAll(tasks, 1000);

                // Process the result (the tasks have already finished their work!)
                foreach (var task in tasks)
                {
                    List<string> partialResult = ((Task<List<string>>)task).Result;
                    if (partialResult != null && partialResult.Count > 0)
                    {
                        finalResult.AddRange(partialResult);
                    }
                }

                // Return the names of all the files that contain the specified char sequence
                return result;
            });
            //            var t = new Task<List<string>>(() => 
            //                result = SearchFilesWithCharSequence(args[0], args[1], args[2]));
            t.Start();
           
            Task.WaitAll(new Task[]{t});
            // Show the resulsts
            Console.WriteLine("\nFiles found:");
            foreach (var fileName in finalResult)
            {
                Console.WriteLine(fileName);
            }
        }

        private static List<string> SearchFileWithCharSequence(List<string> fileNames, string charSequence)
        {

            Console.WriteLine("<Called SearchFileWithCharSequence>");
            List<string> res = new List<string>();

            foreach (var file in fileNames)
            {
                Console.WriteLine(String.Format("Opening file {0} for reading...", file));
                var fs = File.OpenRead(file);
                var buffer = new byte[1]; // 1 byte de cada vez
                AsyncCallback callback = null;

                callback = delegate(IAsyncResult r)
                {
                    int nBytesRead = fs.EndRead(r);
                    if (nBytesRead == 0)
                    {
                        Console.WriteLine("\nEnded reading file " + file);
                        return;
                    }
                    int charSequenceIdx = (int)r.AsyncState;
                    if (buffer[0] == charSequence[charSequenceIdx])
                    {
                        charSequenceIdx++;
                        if (charSequenceIdx == charSequence.Length)
                        {
                            Console.WriteLine("\nEnded reading file " + file);
                            Console.WriteLine("\n~ ~ ~ ~ ~> Found a File with the sequence = " + file);
                            res.Add(file);
                            return;
                        }
                        // Se ainda não se chegou ao fim da sequência, actualizar o idx com o qual se deve comparar o próximo char lido
                        fs.BeginRead(buffer, 0, 1, callback, charSequenceIdx);
                        return;
                    }
                    fs.BeginRead(buffer, 0, 1, callback, 0); // Enviar AsyncState = 0 (para que a nova comparacao comece no idx 0 da sequencia de chars)
                };

                // Começar a leitura... Lendo 1 byte de cada vez
                // É enviado 0 como AsyncState para que se comece a comparar o 1ºchar com o char de idx 0 da sequencia!
                fs.BeginRead(buffer, 0, 1, callback, 0);
            }
            return res;
        }
    }
}