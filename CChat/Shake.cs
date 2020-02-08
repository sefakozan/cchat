using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CChat
{
	public class Shake
	{

		const int SWP_NOZORDER = 0x4;
		const int SWP_NOACTIVATE = 0x10;

		[DllImport("kernel32")]
		static extern IntPtr GetConsoleWindow();


		[DllImport("user32")]
		static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,int x, int y, int cx, int cy, int flags);


		public static void Run() 
		{
			//var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
			//var width = screen.Width;
			//var height = screen.Height;

			var width = 600;
			var height = 450;

			SetWindowPosition(50, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(75, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(50, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(75, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(50, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(75, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(50, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(75, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(50, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(50, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(75, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(50, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(75, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(50, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(75, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(50, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(75, 50, width, height);
			Thread.Sleep(100);
			SetWindowPosition(50, 50, width, height);

		}



		/// <summary>
		/// Sets the console window location and size in pixels
		/// </summary>
		public static void SetWindowPosition(int x, int y, int width, int height)
		{
			SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
		}

		public static IntPtr Handle
		{
			get
			{
				//Initialize();
				return GetConsoleWindow();
			}
		}


	}
}
