using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;
using System.IO;


namespace MyApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<string> Periods { get; } = new();

    [ObservableProperty]
    private string? selectedPeriod;

    public MainWindowViewModel()
    {
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            var periodsJson = File.ReadAllText(@"C:\Users\darkz\Documents\GitHub\Avalonia_Full_API_2026\MyApp\Periods.json");       //client = new HttpClient();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            List<PeriodItem> data = [.. JsonSerializer.Deserialize<List<PeriodItem>?>(periodsJson)];
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            /*
            var periodsJson = File.ReadAllText("Periods.json");

            List<PeriodItem> periods = [.. JsonSerializer.Deserialize<List<PeriodItem>>(periodsJson)];

                        var data = await client.GetFromJsonAsync<List<PeriodItem>>(
                            "http://localhost:5000/api/periods");
            */
            if (data == null)
                return;

            Periods.Clear();

            foreach (var item in data)
            {
                Periods.Add(item.Name);
            }

            if (Periods.Count > 0)
            {
                SelectedPeriod = Periods[0];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public class PeriodItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
