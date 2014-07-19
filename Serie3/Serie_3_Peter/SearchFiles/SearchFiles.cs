using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ServerClientUtils;

namespace SearchFiles
{
    class SearchFiles
    {
        public class SearchFilesResult
        {
            public IEnumerable<string> AbsolutePathNames { get; set; }
            public int TotalFilesFound { get; set; }
            public int FilesFound { get; set; }
        }

        private readonly Logger _logger;

        public SearchFiles(Logger logger)
        {
            _logger = logger;
        }

        CancellationTokenSource _taskCts = new CancellationTokenSource();

        /// <summary>
        /// Realiza uma pesquisa de ficheiros que contenham uma sequência arbitrária de caracteres. 
        /// A pesquisa é realizada assincronamente tirando partido da eventual existência de vários 
        /// processadores e é passível de ser cancelada. 
        /// </summary>
        /// <param name="root">O caminho absoluto da pasta raiz da pesquisa.</param>
        /// <param name="fileExtension">A extensão dos ficheiros a considerar.</param>
        /// <param name="chars">A sequência de caracteres a pesquisar nos ficheiros considerados.</param>
        /// <returns>
        /// O resultado da pesquisa contém:
        /// - A lista dos nomes dos ficheiros (i.e. caminhos absolutos) que cumprem os critérios de pesquisa; 
        /// - O número de ficheiros encontrados com a extensão especificada;
        /// - O número total de ficheiros encontrados
        /// </returns>
        public IEnumerable<SearchFilesResult> SearchFilesTpl(string root, string fileExtension, string chars)
        {
            foreach (string d in Directory.GetDirectories(root))
            {
                var files = Directory.GetFiles(d, String.Format("*.{0}", fileExtension),
                    SearchOption.TopDirectoryOnly);
                    
                // Execute in parallel if there are enough files in the directory. 
                // Otherwise, execute sequentially
                if (files.Length < Environment.ProcessorCount)
                {
                    foreach (string f in files)
                    {
                        Console.WriteLine(f);
                    }
                }
                else
                {
                    _logger.LogMessage("Directory: " + d);
                    Task [] tasks = new Task[Environment.ProcessorCount];

                    int step = files.Length/tasks.Length;

                    _logger.LogMessage(String.Format("Files Length: {0} ; Tasks Length: {1} ; Step : {2}", files.Length, tasks.Length, step));

                    int leftBound, rightBound = -1;
                        
                    for (int i = 0; i < tasks.Length; i++)
                    {
                        leftBound = rightBound + 1;

                        rightBound = (i*step) + (step-1);

                        if (i + 1 == tasks.Length && rightBound < files.Length - 1) // Is this the last iteration and there was a index missing?
                            rightBound += 1;

                        _logger.LogMessage(String.Format(@"Left Bound: {0} ; Right Bound: {1}", leftBound, rightBound));
                        _logger.EndMark();
                        tasks[i] = Task.Factory.StartNew(() =>
                        {
                            // TODO
                        });
                    }
                }
                SearchFilesTpl(d, fileExtension, chars);
            }

            return null;
        }
    }
}