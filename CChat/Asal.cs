using System;
using System.Collections.Generic;
using System.Text;

namespace CChat
{
	public static class Asal
	{

		public static bool IsAsal(string param) 
		{
			bool ret = true;

			int number = Convert.ToInt32(param);

			if (number <= 1) return false;

			for (int i = 2; i < number; i ++) 
			{
				if (number % i == 0) 
				{
					ret = false;
					break;
				}
					
			}

			return ret;
		}



	}
}
