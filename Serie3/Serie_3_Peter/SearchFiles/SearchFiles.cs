using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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


        private volatile int _searchedFiles, _filesIdx;

        /// <summary>
        /// Realiza uma pesquisa de ficheiros, usando a framework TPL, que contenham uma sequência arbitrária de caracteres. 
        /// A pesquisa é realizada assincronamente tirando partido da eventual existência de vários 
        /// processadores e é passível de ser cancelada. 
        /// </summary>
        /// <param name="directory">O caminho absoluto da pasta raiz da pesquisa.</param>
        /// <param name="fileExtension">A extensão dos ficheiros a considerar.</param>
        /// <param name="chars">A sequência de caracteres a pesquisar nos ficheiros considerados.</param>
        /// <param name="stopButton"></param>
        /// <param name="progressBar">Progress bar to be updated.</param>
        /// <param name="searchButton"></param>
        /// <returns>
        /// O resultado da pesquisa contém:
        /// - A lista dos nomes dos ficheiros (i.e. caminhos absolutos) que cumprem os critérios de pesquisa; 
        /// - O número de ficheiros encontrados com a extensão especificada;
        /// - O número total de ficheiros encontrados
        /// </returns>
        public void SearchFilesTpl(CancellationTokenSource _taskCts, string directory, string fileExtension, string chars, Button searchButton, Button stopButton, ProgressBar progressBar)
        {
            CancellationToken ctk = _taskCts.Token;
            TaskScheduler syncCtxScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            var files = Directory.GetFiles(directory, String.Format("*.{0}", fileExtension),
                SearchOption.TopDirectoryOnly);
                    
            // Execute in parallel if there are enough files in the directory. 
            // Otherwise, execute sequentially
            if (files.Length < Environment.ProcessorCount)
            {
                foreach (string f in files)
                {
                    // TODO
                    Console.WriteLine(f);
                }
            }
            else
            {
                _logger.LogMessage("CPU's: " + Environment.ProcessorCount + " ; Directory: " + directory);
                Task[] tasks = new Task[Environment.ProcessorCount];

                #region Old code
                /*int step = files.Length/tasks.Length;

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
                        // This task will now deal only a portion of the total files
                        // SearchInFilesAux(files, leftBound, rightBound);
                    }, ctk);
                }

                Task.WaitAll(tasks, ctk);
                */
                #endregion

                for (int i = 0; i < tasks.Length; i++)
                {
                    tasks[i] = Task.Factory.StartNew((idx) =>
                    {
                        ctk.ThrowIfCancellationRequested();
                        Thread.Sleep(2000);

                        if (SearchInFilesAux(files[(int) idx], chars))
                        {
                            Task.Factory.StartNew(() =>
                            {
                                _logger.LogMessage(@"File: " + files[(int) idx] + @" contém a sequencia de caracteres.");
                            }, ctk, TaskCreationOptions.None, syncCtxScheduler);
                        }

                        Task.Factory.StartNew(() =>
                        {
                            Interlocked.Increment(ref _searchedFiles);
                            progressBar.Value = (100 * _searchedFiles) / files.Length;
                        }, CancellationToken.None, TaskCreationOptions.None, syncCtxScheduler);  
                    } , i, ctk);
                }

                _filesIdx = tasks.Length;

                while (_filesIdx < files.Length)
                {
                    int taskIdx = Task.WaitAny(tasks);

                    var filesIdxAux = _filesIdx; 
                    tasks[taskIdx] = Task.Factory.StartNew(() =>
                    {
                        ctk.ThrowIfCancellationRequested();
                        Thread.Sleep(2000);

                        if (SearchInFilesAux(files[filesIdxAux], chars))
                        {
                            Task.Factory.StartNew(() =>
                            {
                                // Report this file in the console interface
                                _logger.LogMessage(@"File: " + files[filesIdxAux] + @" contém a sequencia de caracteres.");
                            }, ctk, TaskCreationOptions.None, syncCtxScheduler);
                        }

                        Task.Factory.StartNew(() =>
                        {
                            Interlocked.Increment(ref _searchedFiles);
                            progressBar.Value = (100 * _searchedFiles) / files.Length;
                        }, CancellationToken.None, TaskCreationOptions.None, syncCtxScheduler);
                    }
                    , ctk);

                    Interlocked.Increment(ref _filesIdx);
                }

                Task.Factory.ContinueWhenAll(tasks, tasks1 =>
                {
                    searchButton.Enabled = true;
                    stopButton.Enabled = false;
                }, CancellationToken.None, TaskContinuationOptions.None, syncCtxScheduler);
            }
        }

        private bool SearchInFilesAux(string pathFile, string chars)
        {
            try
            {
                using (StreamReader sr = new StreamReader(pathFile))
                {
                    String line = sr.ReadToEnd();
                    Console.WriteLine(line);

                    return (line.IndexOf(chars) != -1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"The file could not be read:");
                Console.WriteLine(e.Message);

                return false;
            }
        }
    }
}