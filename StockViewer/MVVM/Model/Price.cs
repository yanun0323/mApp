﻿namespace StockViewer.MVVM.Model;

public struct Price
{
    // [0]  "證券代號"
    // [1]  "證券名稱"
    // [2]  "成交股數"
    // [3]  "成交筆數"
    // [4]  "成交金額"
    // [5]  "開盤價"
    // [6]  "最高價"
    // [7]  "最低價"
    // [8]  "收盤價"
    // [9]  "漲跌(+/-)"
    // [10] "漲跌價差"
    // [11] "最後揭示買價"
    // [12] "最後揭示買量"
    // [13] "最後揭示賣價"
    // [14] "最後揭示賣量"
    // [15] "本益比"
    
    public string Volume { get; init; }
    public string VolumeMoney { get; init; }
    public string Start { get; init; }
    public string Max { get; init; }
    public string Min { get; init; }
    public string End { get; init; }
    public string Grade { get; init; }
    public string Spread { get; init; }
    public string Per { get; init; }
    [JsonIgnore]
    public int mVolume { get => int.Parse(Volume.Replace(",", "")) / 1000; }
    [JsonIgnore]
    public long mVolumeMoney { get => long.Parse(VolumeMoney.Replace(",", "")); }
    [JsonIgnore]
    public double mStart { get => double.Parse(Start.Replace(",", "")); }
    [JsonIgnore]
    public double mMax { get => double.Parse(Max.Replace(",", "")); }
    [JsonIgnore]
    public double mMin { get => double.Parse(Min.Replace(",", "")); }
    [JsonIgnore]
    public double mEnd { get => double.Parse(End.Replace(",", "")); }
    [JsonIgnore]
    public double mSpread { get => double.Parse(Spread.Replace(",", "")); }
    [JsonIgnore]
    public double mPer { get => double.Parse(Per.Replace(",", "")); }
    [JsonIgnore]
    public string mRatio
    {
        get
        {
            double lastEnd = mEnd + mSpread;
            return $"{Math.Round(100 * mSpread / lastEnd, 2)} %";
        }
    }
    [JsonIgnore]
    public SolidColorBrush mColor
    {
        get
        {
            if (mSpread > 0)
                return iColor.Red;
            if (mSpread < 0)
                return iColor.Green;
            return iColor.Gray;
        }
    }

    public static Price Deafult() {
        return new Price()
        {
            Volume = "0",
            VolumeMoney = "0",
            Start = "0",
            Max = "0",
            Min = "0",
            End = "0",
            Grade = "0",
            Spread = "0",
            Per = "0",
        };
    }
}