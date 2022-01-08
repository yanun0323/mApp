﻿
namespace StockViewer.MVVM.ViewModel;
public class MainViewModel : ObservableObject
{
    readonly string _DefaultStockId = "2330";

    private StockModel _mStockModel = new();

    public StockModel mStockModel
    {
        get { return _mStockModel; }
        set
        {
            _mStockModel = value;
            OnPropertyChanged();
        }
    }

    private string _searchWords = "";

    private ObservableCollection<PickStockBlockViewModel> _PickStockVMCollection;

    public ObservableCollection<PickStockBlockViewModel> PickStockVMCollection
    {
        get { return _PickStockVMCollection; }
        set { _PickStockVMCollection = value; OnPropertyChanged(); }
    }

    public Dictionary<char, List<string>> StockList { get; set; } = new();
    public MainChartViewModel? MainChartVM { get; set; }
    public string SearchWords
    { 
        get => _searchWords; 
        set 
        { 
            _searchWords = value;
            //Search();
            OnPropertyChanged(); 
        } 
    }

    public MainViewModel()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        MainCrawler.Run();
        MainConverter.Run();

        _mStockModel = new();
        UpdateStock(_DefaultStockId);

        //WebDatas.Update(mDataPath);

        StockList = GenerateStockList();

        //WebDatas.CheckCorrect(mDataPath, _displayStock);

        MainChartVM = new(mStockModel!);
        Trace.WriteLine("Datas Initialize");

        _PickStockVMCollection = new();
        int count = 0;
        while (count++ < 20) {
            _PickStockVMCollection!.Add(new(mStockModel));
        }
        PickStockVMCollection = _PickStockVMCollection;
    }
    public void UpdateStock(string stockId)
    {
        _mStockModel!.Refresh(stockId);
        mStockModel = _mStockModel;
        MainChartVM?.UpdateChart(mStockModel!);
    }

    static Dictionary<char, List<string>> GenerateStockList()
    {
        Dictionary<char, List<string>> result = FileManagement.LoadJson<Dictionary<char, List<string>>?>(FilePath.Path_StockList, FilePath.Name_StockList) ?? new();

        var folders = new DirectoryInfo(FilePath.Path_Stock).EnumerateDirectories("*");

        if (result.Count() == folders.Count())
            return result;

        result = new();

        foreach (DirectoryInfo folder in folders)
        {
            Trace.WriteLine($"Finding {folder.Name}...");
            StockModel? stockModel = FileManagement.LoadJson<StockModel?>(Path.Combine(FilePath.Path_Stock, folder.Name), FirstFileName(folder));
            if (stockModel != null) 
            {
                string idName = stockModel.IdName;
                foreach (char cha in idName)
                {
                    if (cha == ' ')
                        continue;
                    if (!result.ContainsKey(cha))
                        result.Add(cha, new());

                    result[cha].Add(stockModel.IdName);
                }
            }
        }

        result.SaveJson(FilePath.Path_StockList, FilePath.Name_StockList);
        return result;

        static string FirstFileName(DirectoryInfo folder) => folder.GetFiles()[0].Name;
    }
    void Search() 
    {
        if(string.IsNullOrEmpty(SearchWords))
            return;
    }
}

