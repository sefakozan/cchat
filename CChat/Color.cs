using System;
using System.Collections.Generic;
using System.Text;

namespace CChat
{
	static class Color
	{
		static Random Random = new Random();

		public static void Yaz(string param) 
		{
			ConsoleColor oldColor = Console.ForegroundColor;
			

			foreach (var c in param)
			{
				int random = Random.Next(1,14);
				Console.ForegroundColor = (ConsoleColor)random;
				Console.Write(c);

			}

			Console.WriteLine();

			Console.ForegroundColor = oldColor;
		}



	}
}
