using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using NAudio.Wave;

namespace CChat
{

    public class Program
    {
        public static string MyNickName { get; set; }

        public static void Main(string[] args)
        {
            //SystemTray.SetSystemTray();
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.Title = "cchat";

            RunCChat().Wait();
        }


        private static void GelenMesajiEkranaYazWrapper(string nickname, string mesaj) 
        {
            Task.Run(()=> GelenMesajiEkranaYaz(nickname,  mesaj));

        }

        private static void GelenMesajiEkranaYaz(string nickname, string mesaj) 
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            if (!string.IsNullOrEmpty(mesaj)) 
            {
                Play.PlayAudio("0",false);
                //Console.Beep();
                if (nickname != MyNickName)
                {
                    Console.WriteLine($"{nickname}: {mesaj}");
                }

                string[] mesajArray = mesaj.Split(" ",2);
                string komut = mesajArray[0];
                string param = string.Empty;
                if (mesajArray.Length > 1) 
                {
                    param = mesajArray[1];
                }

                try
                {
                    GelenMesajiKontrolEt(nickname, komut, param);
                }
                catch
                {

                  
                }

               
                
            }
            
            Console.ForegroundColor = oldColor;
        }


        private static void GelenMesajiKontrolEt(string nickname, string komut, string param) 
        {
            switch (komut)
            {
                case "nick":
                    if (!string.IsNullOrEmpty(param))
                    {
                        MyNickName = param;
                    }
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "asd":
                    string igrencEspiri = Asd.Get(param);
                    Console.WriteLine(igrencEspiri);
                    break;
                case "style":
                    try
                    {
                        var asciArt = new WenceyWang.FIGlet.AsciiArt(param);
                        var text = asciArt.ToString();
                        Console.WriteLine(text);
                    }
                    catch 
                    {
                        Console.WriteLine(param);
                    }
                    break;
                case "check":
                    bool isAsal = Asal.IsAsal(param);
                    if (isAsal)
                    {
                        Console.WriteLine($"{param} sayısı asal sayidir.");
                    }
                    else 
                    {
                        Console.WriteLine($"{param} sayısı asal sayi değildir.");
                    }
                    break;

                case "play":
                    Play.PlayAudio(param,true);
                    break;

                case "open":
                    System.Diagnostics.Process.Start("cmd", $"/C start http://{param}");
                    break;

                case "matrix":
                    Matrix.Run();
                    break;

                case "color":
                    Color.Yaz(param);
                    break;
                case "shake":
                    Shake.Run();
                    break;
            }

        }


        private static async Task RunCChat()
        {
            Console.Write("nick name:");
            MyNickName = Console.ReadLine();

            var client = new FirebaseClient("https://cchat-d1a44-default-rtdb.europe-west1.firebasedatabase.app/");
            var child = client.Child("messages");
            var observable = child.AsObservable<InboundMessage>();

            // önceki tüm mesajlari sil
            await child.DeleteAsync();

            // gelen tüm mesajlari 'GelenMesajiEkranaYaz' metodu ile ekrana yaz.
            var subscription = observable
                .Where(f => !string.IsNullOrEmpty(f.Key)) 
                .Where(f => !string.IsNullOrEmpty(f.Object?.Message) )
                .Subscribe(f => GelenMesajiEkranaYazWrapper(f.Object?.NickName, f.Object?.Message));

            while (true)
            {
                var mesaj = Console.ReadLine();
                Matrix.IsMatrixRunning = false;

                if (mesaj?.ToLower() == "exit")
                {
                    break;
                }

                OutboundMessage gidecekMesaj = new OutboundMessage();
                gidecekMesaj.NickName = MyNickName;
                gidecekMesaj.Message = mesaj;

                // mesaj boş degilse mesajı herkese gönder.
                if (!string.IsNullOrEmpty(mesaj)) 
                {
                    await child.PostAsync(gidecekMesaj);
                }
            }

            subscription.Dispose();
            AudioPlaybackEngine.Instance.Dispose();
        }
    }
}