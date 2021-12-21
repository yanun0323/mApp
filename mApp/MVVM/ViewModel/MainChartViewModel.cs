﻿
using System.Collections.ObjectModel;

namespace mApp.MVVM.ViewModel;
public class MainChartViewModel:ObservableObject
{
    public ICommand? MouseWheelCommand { get; set; }
    public ICommand? LoadedCommand { get; set; }

    private double _Top;
    private double _Bottom;
    private DateTime _StartDate;
    private int? _CandleCount = 100;
    private Dictionary<DateTime, TradingData> _TradingDatas = new();
    private Stock _mStock = new();
    private ObservableCollection<CandleViewModel>? _CandleVMs;
    private Size _ChartSize;
    public double Top { get => _Top; set { _Top = value; OnPropertyChanged(); } }
    public double Bottom { get => _Bottom; set { _Bottom = value; OnPropertyChanged(); } }
    public DateTime StartDate { get => _StartDate; set { _StartDate = value; OnPropertyChanged(); } }
    public int? CandleCount { get => _CandleCount; set { _CandleCount = value; OnPropertyChanged(); } }
    public Dictionary<DateTime,TradingData> TradingDatas { get => _TradingDatas; set { _TradingDatas = value; OnPropertyChanged(); } }
    public ObservableCollection<CandleViewModel>? CandleVMs { get => _CandleVMs; set { _CandleVMs = value; OnPropertyChanged(); } }

    public Stock mStock
    {
        get => _mStock;
        set 
        { 
            _mStock = value;
            Update();
            OnPropertyChanged();
        }
    }

    public MainChartViewModel(Stock stock)
    {
        mStock = stock;

        MouseWheelCommand = new RelayCommand<MouseWheelEventArgs>(e =>
        {
            foreach (var candleVN in _CandleVMs!)
            {
                candleVN!.Zoom(e.Delta , _ChartSize, CandleCount);
            }
        });

        LoadedCommand = new RelayCommand<Size>(size => _ChartSize = size);
    }

    void Update()
    {

        StartDate = mStock!.GetLastDate();
        TradingDatas = UpdateTradingDatas();
        Top = TradingDatas!.Max(x => x.Value.mMax);
        Bottom = TradingDatas!.Min(x => x.Value.mMin);
        CandleVMs = UpdateCandleVMs();
    }
    public void UpdateChart(Stock stock) => mStock = stock;
    private Dictionary<DateTime, TradingData> UpdateTradingDatas()
    {
        DateTime date = StartDate;
        Dictionary<DateTime, TradingData> result = new();
        int count = mStock.TradingData.Count();
        while (CandleCount > result.Count && count > 0 )
        {
            if (mStock!.TradingData.ContainsKey(date))
                result.Add(date, mStock.TradingData[date]);

            date = date.AddDays(-1);
            count--;
        }
        return result;
    }
    private ObservableCollection<CandleViewModel> UpdateCandleVMs()
    {
        ObservableCollection<CandleViewModel> result = new();
        foreach (var entry in TradingDatas) 
        {
            CandleParameter cp = new()
            {
                Date = entry.Key,
                Width = 10,
                Height = 200,
                Top = Top,
                Bottom = Bottom,
                Tr = entry.Value
            };

            result.Add(new CandleViewModel(cp));
        }
        return result;
    }

}
