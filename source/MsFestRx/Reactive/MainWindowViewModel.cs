using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace Reactive
{
	public class MainWindowViewModel: ReactiveObject
	{

		public MainWindowViewModel()
		{
			DoSearch = new ReactiveAsyncCommand();

			this.ObservableForProperty(x => x.SearchText)
				.Throttle(TimeSpan.FromMilliseconds(600), RxApp.DeferredScheduler)
				.Select(x => x.Value)
				.DistinctUntilChanged()
				.Where(x => !String.IsNullOrEmpty(x))
				.InvokeCommand(DoSearch);

			var search = DoSearch.RegisterAsyncFunction(o =>
			                                            {
			                                            	var client = new SourceDataClient.SourceDataClient();
			                                            	var data = client.GetData(SearchText, false);
			                                            	return data;
			                                            });

			_Results = search.ToProperty(this, s => s.Results, new List<string>());
		}

		private string _SearchText;
		public string SearchText
		{
			get { return _SearchText; }
			set { this.RaiseAndSetIfChanged(x => x.SearchText, value); }
		}

		private ObservableAsPropertyHelper<List<string>> _Results;
		public List<string> Results
		{
			get { return _Results.Value; }
		}

		public ReactiveAsyncCommand DoSearch { get; protected set; }
	}
}