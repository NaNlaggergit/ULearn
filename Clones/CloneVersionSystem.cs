using System.Collections.Generic;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
    private class Clone
    {
        public Stack<string> Learned { get; }
        public Stack<string> Rollbacks { get; }
        public Clone()
        {
            Learned = new Stack<string>();
            Rollbacks = new Stack<string>();
        }
        public Clone(Clone otherClone)
        {
            Learned = new Stack<string>(new Stack<string>(otherClone.Learned));
            Rollbacks = new Stack<string>(new Stack<string>(otherClone.Rollbacks));
        }
    }
    private readonly List<Clone> _clones = new List<Clone>();
    public CloneVersionSystem()
    {
        _clones.Add(new Clone());
    }

    public string Execute(string query)
    {
        var parts = query.Split(' ');
        switch (parts[0])
        {
            case "learn":
                {
                    int ci = int.Parse(parts[1]) - 1;
                    string program = parts[2];
                    var clone = _clones[ci];
                    clone.Learned.Push(program);
                    return null;
                }

            case "rollback":
                {
                    int ci = int.Parse(parts[1]) - 1;
                    var clone = _clones[ci];
                    string last = clone.Learned.Pop();
                    clone.Rollbacks.Push(last);
                    return null;
                }
            case "relearn":
                {
                    int ci = int.Parse(parts[1]) - 1;
                    var clone = _clones[ci];
                    string program=clone.Rollbacks.Pop();
                    clone.Learned.Push(program);
                    return null;
                }
            case "clone":
                {
                    int ci = int.Parse(parts[1]) - 1;
                    _clones.Add(new Clone(_clones[ci]));
                    return null;
                }
            case "check":
                {
                    int ci = int.Parse(parts[1]) - 1;
                    var clone= _clones[ci];
                    if(clone.Learned.Count == 0)
                    {
                        return "basic";
                    }
                    else
                        return clone.Learned.Peek().ToString();
                }
            default:
                return null;
        }
    }
}