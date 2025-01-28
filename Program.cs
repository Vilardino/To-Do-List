using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ToDoList
{
    class Program
    {
        // Lista de tarefas
        static List<Task> tasks = new List<Task>();

        // Caminho do arquivo onde as tarefas serão salvas
        static string dataFilePath = "tasks.txt";

        static void Main(string[] args)
        {
            try
            {
                // Carrega as tarefas salvas ao iniciar o programa
                LoadTasks();
                RunMainLoop();
            }
            catch (Exception ex)
            {
                // Tratamento de exceções globais
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        static void RunMainLoop()
        {
            bool running = true;

            // Loop principal do programa
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine() ?? string.Empty;
                ProcessChoice(choice, ref running);
            }

            // Salva as tarefas ao sair do programa
            SaveTasks();
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- To-Do List ---");
            Console.ResetColor();
            Console.WriteLine("1. Adicionar Tarefa");
            Console.WriteLine("2. Remover Tarefa");
            Console.WriteLine("3. Visualizar Todas as Tarefas");
            Console.WriteLine("4. Visualizar Tarefas Concluídas");
            Console.WriteLine("5. Visualizar Tarefas Pendentes");
            Console.WriteLine("6. Marcar Tarefa como Concluída");
            Console.WriteLine("7. Sair");
            Console.Write("Escolha uma opção: ");
        }

        static void ProcessChoice(string choice, ref bool running)
        {
            // Processa a escolha do usuário
            switch (choice)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    RemoveTask();
                    break;
                case "3":
                    ViewAllTasks();
                    break;
                case "4":
                    ViewCompletedTasks();
                    break;
                case "5":
                    ViewPendingTasks();
                    break;
                case "6":
                    MarkTaskAsCompleted();
                    break;
                case "7":
                    running = false;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }
        }

        // Método para adicionar uma nova tarefa
        static void AddTask()
        {
            try
            {
                Console.Write("Digite a nova tarefa: ");
                string description = Console.ReadLine() ?? string.Empty;

                Console.Write("Digite a categoria (ex: Trabalho, Casa, Estudo): ");
                string category = Console.ReadLine() ?? string.Empty;

                // Adiciona a nova tarefa à lista
                tasks.Add(new Task(description, category));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Tarefa adicionada com sucesso!");
                Console.ResetColor();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao adicionar tarefa: {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        // Método para remover uma tarefa
        static void RemoveTask()
        {
            try
            {
                if (tasks.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Não há tarefas para remover.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                // Exibe as tarefas para o usuário escolher qual remover
                ViewAllTasks();

                Console.Write("Digite o número da tarefa que deseja remover: ");
                if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
                {
                    // Remove a tarefa escolhida
                    tasks.RemoveAt(taskNumber - 1);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tarefa removida com sucesso!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Número inválido. Tente novamente.");
                    Console.ResetColor();
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao remover tarefa: {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        // Método para visualizar todas as tarefas
        static void ViewAllTasks()
        {
            try
            {
                if (tasks.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Não há tarefas na lista.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n--- Todas as Tarefas ---");
                Console.ResetColor();

                // Exibe cada tarefa com sua categoria e status, com cores diferentes para concluídas e não concluídas
                foreach (var task in tasks.Select((value, index) => new { value, index }))
                {
                    Console.ForegroundColor = task.value.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"{task.index + 1}. [{task.value.Category}] {task.value.Description} {(task.value.IsCompleted ? "[Concluída]" : "")}");
                }
                Console.ResetColor();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao visualizar tarefas: {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        // Método para visualizar apenas as tarefas concluídas
        static void ViewCompletedTasks()
        {
            try
            {
                // Filtra as tarefas concluídas
                var completedTasks = tasks.FindAll(t => t.IsCompleted);

                if (completedTasks.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Não há tarefas concluídas.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n--- Tarefas Concluídas ---");
                Console.ResetColor();
                for (int i = 0; i < completedTasks.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i + 1}. [{completedTasks[i].Category}] {completedTasks[i].Description}");
                    Console.ResetColor();
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao visualizar tarefas concluídas: {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        // Método para visualizar apenas as tarefas pendentes
        static void ViewPendingTasks()
        {
            try
            {
                // Filtra as tarefas não concluídas
                var pendingTasks = tasks.FindAll(t => !t.IsCompleted);

                if (pendingTasks.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Não há tarefas pendentes.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n--- Tarefas Pendentes ---");
                Console.ResetColor();
                for (int i = 0; i < pendingTasks.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{i + 1}. [{pendingTasks[i].Category}] {pendingTasks[i].Description}");
                    Console.ResetColor();
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao visualizar tarefas pendentes: {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        // Método para marcar uma tarefa como concluída
        static void MarkTaskAsCompleted()
        {
            try
            {
                if (tasks.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Não há tarefas para marcar como concluída.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                // Exibe as tarefas para o usuário escolher qual marcar como concluída
                ViewAllTasks();

                Console.Write("Digite o número da tarefa que deseja marcar como concluída: ");
                if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
                {
                    // Marca a tarefa como concluída
                    tasks[taskNumber - 1].IsCompleted = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tarefa marcada como concluída!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Número inválido. Tente novamente.");
                    Console.ResetColor();
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao marcar tarefa como concluída: {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        // Método para carregar as tarefas do arquivo
        static void LoadTasks()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    // Lê todas as linhas do arquivo
                    string[] lines = File.ReadAllLines(dataFilePath);
                    foreach (string line in lines)
                    {
                        // Divide a linha em partes (Descrição|Categoria|Concluída)
                        string[] parts = line.Split('|');
                        if (parts.Length == 3)
                        {
                            // Adiciona a tarefa à lista
                            tasks.Add(new Task(parts[0], parts[1], bool.Parse(parts[2])));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao carregar tarefas: {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        // Método para salvar as tarefas no arquivo
        static void SaveTasks()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (Task task in tasks)
                {
                    // Formata cada tarefa como uma linha no arquivo
                    lines.Add($"{task.Description}|{task.Category}|{task.IsCompleted}");
                }
                // Escreve todas as linhas no arquivo
                File.WriteAllLines(dataFilePath, lines);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao salvar tarefas: {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }

    // Classe que representa uma tarefa
    class Task
    {
        public string Description { get; set; } // Descrição da tarefa
        public string Category { get; set; }    // Categoria da tarefa
        public bool IsCompleted { get; set; }   // Status da tarefa (concluída ou não)

        // Construtor da classe Task
        public Task(string description, string category, bool isCompleted = false)
        {
            Description = description;
            Category = category;
            IsCompleted = isCompleted;
        }
    }
}