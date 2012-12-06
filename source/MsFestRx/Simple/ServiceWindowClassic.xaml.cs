using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Simple
{
	/// <summary>
	/// Interaction logic for ServiceWindowClassic.xaml
	/// </summary>
	public partial class ServiceWindowClassic : Window
	{
		public ServiceWindowClassic()
		{
			Results = new ObservableCollection<string>();
			InitializeComponent();

			_doSearch.Click += (sender, e) =>
			                   {
			                   	if (String.IsNullOrEmpty(_searchText.Text))
			                   	{
			                   		return;
			                   	}
			                   	Results.Clear();
			                   	searchService(_searchText.Text,
			                   	              (result) => Dispatcher
			                   	                          	.BeginInvoke(new Action(() =>
			                   	                          	                        {
			                   	                          	                        	foreach (var text in result)
			                   	                          	                        	{
			                   	                          	                        		Results.Add(text);
			                   	                          	                        	}
			                   	                          	                        })));
			                   };
		}

		private void searchService(string search, Action<List<string>> callback)
		{
			var client = new SourceDataClient.SourceDataClient();
			client.BeginGetData(search, true, ar =>
			                                  {
			                                  	var result = client.EndGetData(ar);
			                                  	callback(result);
			                                  }, null);
		}

		public ObservableCollection<string> Results { get; protected set; }
	}
}
