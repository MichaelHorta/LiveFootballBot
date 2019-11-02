using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LiveFootballBot.Commands
{
    public class CommandManager : ICommandManager
    {
        private readonly IBoard _board;
        public readonly static string COMMAND_PREFIX = "/";
        public readonly static string PARAMETER_PREFIX = "-";
        private readonly static string PARAMETER_SEPARATOR = " ";
        private Regex regex = new Regex(@"-[a-z]");
        private IDictionary<string, ICommand> Commands;

        public CommandManager(IBoard board)
        {
            _board = board;
            InitializeCommands();
        }

        public void InitializeCommands()
        {
            var matchesCommand = new MatchesCommand(_board);
            Commands = new Dictionary<string, ICommand>
            {
                { new StartCommand().GetName(), new StartCommand() },
                { matchesCommand.GetName(), matchesCommand }
            };
        }

        public string ExceuteCommand(string input)
        {
            var command = GetCommand(input);

            if (null == command)
            {
                command = new StartCommand();
            }

            var commandParameters = GetCommandParameters(input);
            if (!command.Validate(commandParameters))
            {
                //msg = commandParameters.getOpciones().get(ParametrosComando.KEY_AYUDA_PERSONALIZADA);
                //command = new HelpCommand();
                //parametrosComando = new CommandParameters(command.GetName());
                //parametrosComando.adicionarParametro(ParametrosComando.KEY_AYUDA_PERSONALIZADA, msg);
            }

            return command.Execute(commandParameters);
        }

        public ICommand GetCommand(string input)
        {
            string key = input.Split(PARAMETER_SEPARATOR).FirstOrDefault();
            ICommand command = null;
            Commands.TryGetValue(key, out command);
            return command;
        }

        private CommandParameters GetCommandParameters(string input)
        {
            string key = input.Split(PARAMETER_SEPARATOR).FirstOrDefault();
            var commandParameters = new CommandParameters(key);
            var options = GetCommandOptions(input);

            char? c = null;
            for (int i = 0; i < options.Count; i++)
            {
                if (IsValidOption(options.ElementAt(i)))
                {
                    c = options.ElementAt(i).ToCharArray()[1];
                }
                else if (c.HasValue)
                {
                    string start = null;
                    commandParameters.Options.TryGetValue(c.Value, out start);
                    if (start == null)
                    {
                        start = "";
                    }
                    else
                    {
                        start += " ";
                    }
                    commandParameters.Options[c.Value] = start + options.ElementAt(i);
                }
                else
                {
                    //parametrosComando.setComando((new ComandoAyuda()).getNombre());
                    break;
                }
            }

            return commandParameters;
        }

        private List<string> GetCommandOptions(string input)
        {
            var options = new List<string>();
            string[] array = input.Split(PARAMETER_SEPARATOR);
            for (int i = 1; i < array.Length; i++)
            {
                if (string.IsNullOrEmpty(array[i]))
                    continue;

                options.Add(array[i]);
            }

            return options;
        }

        private bool IsValidOption(string option)
        {
            var match = regex.Match(option);
            return match.Success;
        }
    }
}
