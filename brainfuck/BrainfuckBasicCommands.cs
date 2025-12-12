using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b =>
			{
				write((char)b.Memory[b.MemoryPointer]);
			});
			vm.RegisterCommand('+', b =>
			{
				unchecked
                {
                    b.Memory[vm.MemoryPointer]++;
                }

			});
			vm.RegisterCommand('-', b =>
			{
				unchecked
				{
                    b.Memory[vm.MemoryPointer]--;
                }
			});
			vm.RegisterCommand(',', b =>
			{
				int input = read();
				if (input < 0)
					input = 0;
				b.Memory[b.MemoryPointer] = (byte)input;

			});
			vm.RegisterCommand('>', b =>
			{
				if (b.MemoryPointer == b.Memory.Length-1)
				{
					b.MemoryPointer = 0;
				}
				else
					b.MemoryPointer++;
			});
            vm.RegisterCommand('<', b =>
            {
                if (b.MemoryPointer == 0)
                {
                    b.MemoryPointer = b.Memory.Length-1;
                }
                else
                    b.MemoryPointer--;
            });

            for (char c = 'A'; c <= 'Z'; c++)
                RegisterAsciiWrite(vm, c);

            for (char c = 'a'; c <= 'z'; c++)
                RegisterAsciiWrite(vm, c);

            for (char c = '0'; c <= '9'; c++)
                RegisterAsciiWrite(vm, c);
        }
        private static void RegisterAsciiWrite(IVirtualMachine vm, char c)
        {
            vm.RegisterCommand(c, v =>
            {
                v.Memory[v.MemoryPointer] = (byte)c;
            });
        }
    }
}