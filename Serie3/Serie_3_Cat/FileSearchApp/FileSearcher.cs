using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileSearchApp
{
    class FileSearcher
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

        internal const int BUFFER_SIZE = 1024; //TODO use 

        public class FileSearchResult
        {
            public int _totalFilesFound { get; private set; }
            public int _totalFilesWithValidExtension { get; private set; }
            public List<string> _filesWithExtensionAndSequence { get; private set; }

            public FileSearchResult(int allFiles, int validFilesCount, List<string> validfilesfound)
            {
                _totalFilesFound = allFiles;
                _totalFilesWithValidExtension = validFilesCount;
                _filesWithExtensionAndSequence = validfilesfound;
            }
        }
        
        static void Main2(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Invalid number of arguments! Press any key to exit...");
                Console.ReadLine();
                return;
            }
            
            FileSearchResult result = startSearch(args[0],args[1],args[2]);
            
            Console.WriteLine("\n\n ~ ~ ~ RESULTS ~ ~ ~ ");
            Console.WriteLine("\nTotal Files in Directory " + args[0] + " = " + result._totalFilesFound);
            Console.WriteLine("\nTotal Files With Extension " + args[1] + " = " + result._totalFilesWithValidExtension);
            Console.WriteLine("\nFiles found, with sequence = " + args[2] + ":");

            foreach (var fileName in result._filesWithExtensionAndSequence)
                Console.WriteLine(fileName);

            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }

        // arguments:
        //  - search root                   // args[1]
        //  - searchable files extension    // args[2]
        //  - char sequence to search       // args[3]

        public static FileSearchResult startSearch(string searchRoot, string fileExtension, string charSequence)
        {
            var finalResult = new List<string>();
            var result = new List<string>();
            int nCPU = Environment.ProcessorCount;
            
            int allFiles = Directory.GetFiles(searchRoot,"*.*",SearchOption.AllDirectories).Count();
            var searchableFiles = new List<string>(Directory.GetFiles(searchRoot, "*." + fileExtension, SearchOption.AllDirectories));
            int allFilesWithExt = searchableFiles.Count;
            int maximumFilesPerCpu = searchableFiles.Count / nCPU;

            if (searchableFiles.Count < nCPU)
            {
                nCPU = searchableFiles.Count;
                maximumFilesPerCpu = 1;
            }

            Task[]tasks = new Task<List<string>>[nCPU];

            for (var i = 0; i < nCPU; i++)
            {
                //TODO Define the files that each task will be responsible for examining!
                var files = new List<string>(maximumFilesPerCpu);

                int j = 0;
                while (searchableFiles.Count > 0)
                {
                    files.Add(searchableFiles.ToArray().GetValue(0) as string);
                    searchableFiles.RemoveAt(0);
                    if (j++ == maximumFilesPerCpu) break;
                }

                // Define the action that will be associated to each of the tasks
                tasks[i] = Task<List<string>>.Factory.StartNew(() => files.Count > 0 ? 
                    SearchFileWithCharSequence(files, charSequence) : new List<string>());
            }

            Task.WaitAll(tasks);
            
            Task k = Task.Delay(100);
            k.Wait();
            
            foreach (var task in tasks)
            {
                List<string> partialResult = ((Task<List<string>>)task).Result;
                if (partialResult != null && partialResult.Count > 0)
                {
                    finalResult.AddRange(partialResult);
                }
            }

            // Show the results
            return new FileSearchResult(allFiles, allFilesWithExt, finalResult);
        }

        private static List<string> SearchFileWithCharSequence(List<string> fileNames, string charSequence)
        {

            //writer.WriteLine("<Called SearchFileWithCharSequence>");//Console.WriteLine("<Called SearchFileWithCharSequence>");
            List<string> res = new List<string>();

            foreach (var file in fileNames)
            {
                //writer.WriteLine(String.Format("Opening file {0} for reading...", file));//Console.WriteLine(String.Format("Opening file {0} for reading...", file));
                var fs = File.OpenRead(file);
                var buffer = new byte[1]; // 1 byte de cada vez
                AsyncCallback callback = null;

                callback = delegate(IAsyncResult r)
                {
                    int nBytesRead = fs.EndRead(r);
                    if (nBytesRead == 0)
                    {
                        //writer.WriteLine("Sopped reading file " + file);//Console.WriteLine("\nEnded reading file " + file);
                        return;
                    }
                    int charSequenceIdx = (int)r.AsyncState;
                    if (buffer[0] == charSequence[charSequenceIdx])
                    {
                        charSequenceIdx++;
                        if (charSequenceIdx == charSequence.Length)
                        {
//                            writer.WriteLine("Stopped reading file " + file);//Console.WriteLine("\nEnded reading file " + file);
//                            writer.WriteLine("Found a File with the sequence = " + file);//Console.WriteLine("\n~ ~ ~ ~ ~> Found a File with the sequence = " + file);
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