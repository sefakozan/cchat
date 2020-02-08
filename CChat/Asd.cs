using System;
using System.Collections.Generic;
using System.Text;

namespace CChat
{
	static class Asd
	{
		static string[] AsdArray = new string[] 
		{
			"Geçen gün 'Ödeme noktasına' gittim 'Ö' dedim geldim...",
			"+ Erkek erkeğe yenen yemeğe ne denir?\n- Menemen(Man a man).",
			"+ Bana bir pilav üstüne et.\n- Bana da bir pilav ama üstüne etme.",
			"+ Kediler havaalanına neden giremezler?\n- Çünkü orada pist var.",
			"Sana bir kıllık yapayım, kıllarını koyarsın.",
			"Seven unutmaz oğlum, eight unutur."
		};

		public static string Get(string param) 
		{
			string ret = string.Empty;

			try
			{
				int index = Convert.ToInt32(param);
				ret = AsdArray[index];
				
			}
			catch
			{
				ret = AsdArray[0];
			}
			
			return ret;
		}

	}
}
