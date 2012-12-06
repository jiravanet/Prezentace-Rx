using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Simple
{
	/// <summary>
	/// Interaction logic for ServiceWindow.xaml
	/// </summary>
	public partial class ServiceWindow : Window
	{
		public ServiceWindow()
		{
			Results = new ObservableCollection<string>();
			InitializeComponent();

			Results.Add("Vitejte na MS Fest");
			var search = new Subject<RoutedEventArgs>();
			_doSearch.Click += (sender, e) => search.OnNext(e);
			var searchText = search.Select(arg => _searchText.Text)
				.Where(text => String.IsNullOrEmpty(text) == false);
				//.Throttle(TimeSpan.FromSeconds(1))
				//.DistinctUntilChanged();

			searchText.ObserveOn(SynchronizationContext.Current).Subscribe(text =>
													 {
														 Debug.WriteLine(text, "Search");
														 Results.Clear();
													 });
			//var res = (from t in searchText
			//           select searchService(t));
			//          .Switch();
			var res = from t in searchText
			          from word in searchService(t).
			            TakeUntil(searchText)
			          select word;
			
			res.ObserveOn(SynchronizationContext.Current).
				Subscribe(text =>
				          {
				          	Results.Add(text);
				          });
			//var textToAdd = searchText.
			//  SelectMany(text => searchService(text)).
			//  ObserveOn(SynchronizationContext.Current).
			//  Select(result => result);

			//textToAdd.Subscribe(text =>
			//                    {
			//                      Results.Add(text);
			//                      Debug.WriteLine(text);
			//                    });
		}

		IObservable<string> searchService(string search)
		{
			var client = new SourceDataClient.SourceDataClient();
			Func<string, bool, IObservable<List<string>>> func = Observable.FromAsyncPattern<string, bool, List<string>>(client.BeginGetData, client.EndGetData);
			return func(search, true).SelectMany(response => response.ToObservable());
		}


		public ObservableCollection<string> Results { get; protected set; }
	}
}
