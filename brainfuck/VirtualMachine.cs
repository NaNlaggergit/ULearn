using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }

		private readonly Dictionary<char, Action<IVirtualMachine>> _commands =
			new Dictionary<char, Action<IVirtualMachine>>();

		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program;
			Memory= new byte[memorySize];
            InstructionPointer = 0;
			MemoryPointer = 0;
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			_commands[symbol] = execute;
		}

		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				var symbol = Instructions[InstructionPointer];
				if(_commands.TryGetValue(symbol,out var action))
				{
					action(this);
				}
				InstructionPointer++;
			}
		}
	}
}