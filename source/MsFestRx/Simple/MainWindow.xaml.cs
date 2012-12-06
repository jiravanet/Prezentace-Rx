using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simple
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var mousedown = from evnt in Observable.FromEventPattern<MouseButtonEventArgs>(_image, "MouseLeftButtonDown")
			                select evnt.EventArgs.GetPosition(_image);

			var mouseup = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseLeftButtonUp");

			var mousemove = from evnt in Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove")
			                select evnt.EventArgs.GetPosition(this);

			var query = from start in mousedown
			            from end in mousemove.TakeUntil(mouseup)
			            select new 
			                   {
			                   	X = end.X - start.X,
			                   	Y = end.Y - start.Y
			                   };

			query.Subscribe(value =>
			                {
			                	Canvas.SetLeft(_image, value.X);
			                	Canvas.SetTop(_image, value.Y);
			                });
		}
	}
}
