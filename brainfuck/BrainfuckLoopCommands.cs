using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static Dictionary<int, int> Bracket = new Dictionary<int, int>();
        public static Dictionary<int, int> CloseBracket = new Dictionary<int, int>();
        public static Stack<int> Stack = new Stack<int>();

        public static void Loop(IVirtualMachine vm)
        {
            for (int i = 0; i < vm.Instructions.Length; i++)
            {
                var bracket = vm.Instructions[i];
                switch (bracket)
                {
                    case '[':
                        {
                            Stack.Push(i);
                            break;
                        }
                    case ']':
                        {
                            CloseBracket[i] = Stack.Peek();
                            Bracket[Stack.Pop()] = i;
                            break;
                        }
                }
            }
        }
        public static void RegisterTo(IVirtualMachine vm)
        {
            Loop(vm);
            vm.RegisterCommand('[', b =>
            {
                if (b.Memory[b.MemoryPointer] == 0)
                {
                    b.InstructionPointer = Bracket[b.InstructionPointer];
                }

            });
            vm.RegisterCommand(']', b => 
            {
                if(b.Memory[b.MemoryPointer] != 0)
                {
                    b.InstructionPointer = CloseBracket[b.InstructionPointer];
                }
            });
        }
    }
}