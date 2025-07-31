using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CChat
{
	static class Asd
	{
        static Random random = new Random();

        static string[] AsdArray = new string[] 
		{
			"Geçen gün 'Ödeme noktasına' gittim 'Ö' dedim geldim...",
			"+ Erkek erkeğe yenen yemeğe ne denir?\n- Menemen(Man a man).",
			"+ Bana bir pilav üstüne et.\n- Bana da bir pilav ama üstüne etme.",
			"+ Kediler havaalanına neden giremezler?\n- Çünkü orada pist var.",
			"Sana bir kıllık yapayım, kıllarını koyarsın.",
			"Seven unutmaz oğlum, eight unutur.",
            "+ Yıkanan ton balığına ne denir?\n- WASHINGTON",
            "+ İngilizler kendi kıllarına ne der?\n- MICHEAL",
            "+ Taşımasu annesinden nasıl su ister?\n- MATARAMASUKO"
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
                int randomIndex = random.Next(0, AsdArray.Length);
                ret = AsdArray[randomIndex];
			}
			
			return ret;
		}

	}
}
