using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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



        static void Main(string[] args)
        {
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

        public static async void startSearch(String[]args)
        {
            // Call the method
            List<string> result = null;
            Task<List<string>> t = new Task<List<string>>(delegate()
            {
                return result = SearchFilesWithCharSequence(args[0], args[1], args[2]);

            });
            t.Start();
            await t;

            // Show the resulsts
            Console.WriteLine("\nFiles found:");
            foreach (var fileName in result)
            {
                Console.WriteLine(fileName);
            }
        }

        public static List<string> SearchFilesWithCharSequence(string searchRoot, string fileExtension, string charSequence)
        {
            List<string> result = new List<string>();

            CancellationToken ct = new CancellationToken();
            int nCPU = Environment.ProcessorCount;
            

            List<String> searchableFiles = GetFilesWithExtension(searchRoot, fileExtension);
            int maximumFilesPerCpu = searchableFiles.Count/nCPU;

            if (searchableFiles.Count < nCPU)
            {
                nCPU = searchableFiles.Count;
                maximumFilesPerCpu = 1;
            }
            Task<List<string>>[] tasks = new Task<List<string>>[nCPU];

            for (int i = 0; i < nCPU; i++)
            {
                //TODO Define the files that each task will be responsible for examining!
                List<String> files = new List<string>(maximumFilesPerCpu);
                
                int j = 0;
                while(searchableFiles.Count>0)
                {
                    files.Add(searchableFiles.ToArray().GetValue(0) as string);
                    searchableFiles.RemoveAt(0);
                    j++;
                    if (j == maximumFilesPerCpu) break;
                }

                // Define the action that will be associated to each of the tasks
                tasks[i] = new Task<List<string>>(delegate
                {
                    if (files.Count > 0)
                        return SearchFileWithCharSequence(files, charSequence);
                    return null;
                });
                tasks[i].Start();
            }

            // Wait for all the tasks to finish their work
            Task.WaitAll(tasks);

            // Process the result (the tasks have already finished their work!)
            foreach (var task in tasks)
            {
                List<string> res = task.AsyncState as List<string>;
                if (res != null && res.Count>0)
                {
                    result.AddRange(res);
                }
            }

            // Return the names of all the files that contain the specified char sequence
            return result;
        }

        private static List<String> GetFilesWithExtension(string searchRoot, string fileExtension)
        {
            String[] allFilesInDirectory = Directory.GetFiles(
                searchRoot,
                "*."+fileExtension, 
                SearchOption.AllDirectories);

            return new List<string>(allFilesInDirectory);
        }

        private static List<string> SearchFileWithCharSequence(List<string>fileNames, string charSequence)
        {

            Console.WriteLine("<Called SearchFileWithCharSequence>");
            List<string>res = new List<string>();
            
            //TODO

            foreach (var file in fileNames)
            {
                Console.WriteLine(String.Format("Opening file {0} for reading...", file));  
                var fs = File.OpenRead(file);
                var buffer = new byte[1] ; // 1 byte de cada vez
                AsyncCallback callback = null;

                callback = delegate(IAsyncResult r)
                {
                    int nBytesRead = fs.EndRead(r);
                    if (nBytesRead == 0)
                    {
                        Console.WriteLine("\nEnded reading file "+file);
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
                    return;
                };

                // Começar a leitura... Lendo 1 byte de cada vez
                // É enviado 0 como AsyncState para que se comece a comparar o 1ºchar com o char de idx 0 da sequencia!
                fs.BeginRead(buffer, 0, 1, callback, 0);
               // return res;
            }
            return res;
        }
    }
}
