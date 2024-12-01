using System.Collections.Generic;

namespace SetlistHelper.Services;

/**
 * ArgParser handles collecting the command line args into a Dictionary of options
 * and their values. It uses the state machine pattern to iterate over the
 * args passed to a program and store them as option types and their values.
 */
public class ArgParser {
    public struct Option : IEquatable<Option> {
        public string LongOpt;
        public string ShortOpt;

        public override bool Equals(object? obj) => obj is Option other && this.Equals(other);

        public bool Equals(Option o) => LongOpt == o.LongOpt && ShortOpt == o.ShortOpt;

        public override int GetHashCode() => (LongOpt, ShortOpt).GetHashCode();

        public static bool operator ==(Option a, Option b) => a.Equals(b);
        public static bool operator !=(Option a, Option b) => !(a == b);
    }

    public enum States {
        FindOpt,
        FindVal,
        End
    };

    private readonly string[] _args;
    private List<Option> _opts;
    private Dictionary<string, string?> _parsedOpts;
    private string? _currentOpt;
    private States _state = States.FindOpt;
    private readonly Dictionary<States, Dictionary<object, Transition>> _transitions;

    private record Transition(States NextState, Action<string>? Action);
    private static readonly object DefaultTrigger = new();
    private static readonly Option NullOpt = new Option();

    public ArgParser(string[] args) {
        _opts = GetOpts();
        _parsedOpts = [];
        _currentOpt = null;
        _args = args;
        _transitions = new Dictionary<States, Dictionary<object, Transition>> {
            {
                States.FindOpt, new Dictionary<object, Transition>()
            },
            {
                States.FindVal, new Dictionary<object, Transition> {
                    {DefaultTrigger, new Transition(States.FindOpt, SetOptionVal)}
                }
            }
        };

        foreach (Option opt in _opts) {
            _transitions[States.FindOpt].Add(
                opt.LongOpt, new Transition(States.FindVal, StoreOption)
            );

            _transitions[States.FindVal].Add(
                opt.LongOpt, new Transition(States.FindVal, StoreOption)
            );
        }
    }

    public void Parse() {
        foreach (string arg in _args) {
            HandleArg(arg);
        }

        _state = States.End;
    }

    public Dictionary<string, string?> GetParsedOptions() {
        return _parsedOpts;
    }

    public Option FindOpt(string arg) {
        foreach (Option opt in _opts) {
            if (opt.LongOpt == arg || opt.ShortOpt == arg) {
                return opt;
            }
        }

        return NullOpt;
    }

    static private List<Option> GetOpts() {
        List<Option> opts = [
            new Option {LongOpt="--help", ShortOpt="-h"},
            new Option {LongOpt="--build", ShortOpt="-b"},
            new Option {LongOpt="--edit", ShortOpt="-e"},
            new Option {LongOpt="--add", ShortOpt="-a"},
            new Option {LongOpt="--remove", ShortOpt="-r"},
            new Option {LongOpt="-update", ShortOpt="-u"}
        ];

        return opts;
    }

    private void HandleArg(string arg) {
        object trigger;
        string actionVal;
        Option opt;
        switch (_state) {
            case States.FindOpt:
                opt = FindOpt(arg);
                trigger = opt.LongOpt;
                actionVal = opt.LongOpt;
                break;
            case States.FindVal:
                opt = FindOpt(arg);
                trigger = opt == NullOpt ? DefaultTrigger : opt.LongOpt;
                actionVal = opt == NullOpt ? arg : opt.LongOpt;
                break;
            default:
                throw new Exception("Invalid state in ArgParser: " + _state.ToString());
        }

        if (_transitions[_state].TryGetValue(trigger, out Transition? transition)) {
            transition.Action?.Invoke(actionVal);
            _state = transition.NextState;
        }
    }

    private void StoreOption(string opt) {
        _parsedOpts.Add(opt, null);
        _currentOpt = opt;
    }

    private void SetOptionVal(string val) {
        if (_currentOpt is null) {
            throw new Exception("Can't set option, current option is null");
        }

        _parsedOpts[_currentOpt] = val;
    }
}
