using System.Net.Http;
using System.Text.Json;

Dictionary<int, List<Holiday>> holidayData = new();

await LoadAllYears();
RunMenu();

async Task LoadAllYears()
{
    int[] years = { 2023, 2024, 2025 };
    foreach (var y in years)
    {
        holidayData[y] = await GetHolidays(y);
    }
}

async Task<List<Holiday>> GetHolidays(int year)
{
    string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/TR";
    using HttpClient client = new HttpClient();
    var json = await client.GetStringAsync(url);
    return JsonSerializer.Deserialize<List<Holiday>>(json);
}

void RunMenu()
{
    while (true)
    {
        Console.WriteLine("===== PublicHolidayTracker =====");
        Console.WriteLine("1. Tatil listesini göster (yıl seçmeli)");
        Console.WriteLine("2. Tarihe göre tatil ara (gg-aa)");
        Console.WriteLine("3. İsme göre tatil ara");
        Console.WriteLine("4. Tüm tatilleri 3 yıl boyunca göster");
        Console.WriteLine("5. Çıkış");
        Console.Write("Seçiminiz: ");

        string secim = Console.ReadLine();
        Console.WriteLine();

        switch (secim)
        {
            case "1":
                ShowHolidaysByYear();
                break;

            case "2":
                SearchByDate();
                break;

            case "3":
                SearchByName();
                break;

            case "4":
                ShowAllYears();
                break;

            case "5":
                return;

            default:
                Console.WriteLine("Geçersiz seçim.");
                break;
        }

        Console.WriteLine();
    }
}

void ShowHolidaysByYear()
{
    Console.Write("Yıl (2023-2025): ");
    int year = int.Parse(Console.ReadLine());

    if (!holidayData.ContainsKey(year))
    {
        Console.WriteLine("Bu yıla ait veri yok.");
        return;
    }

    foreach (var h in holidayData[year])
    {
        Console.WriteLine($"{h.date} - {h.localName}");
    }
}

void SearchByDate()
{
    Console.Write("Tarih (gg-aa): ");
    var input = Console.ReadLine();

    foreach (var list in holidayData.Values)
    {
        foreach (var h in list)
        {
            var d = DateTime.Parse(h.date);
            var formatted = $"{d.Day:00}-{d.Month:00}";

            if (formatted == input)
                Console.WriteLine($"{h.date} - {h.localName}");
        }
    }
}

void SearchByName()
{
    Console.Write("İsim: ");
    var name = Console.ReadLine().ToLower();

    foreach (var list in holidayData.Values)
    {
        foreach (var h in list)
        {
            if (h.localName.ToLower().Contains(name) || h.name.ToLower().Contains(name))
                Console.WriteLine($"{h.date} - {h.localName}");
        }
    }
}

void ShowAllYears()
{
    foreach (var year in holidayData.Keys)
    {
        Console.WriteLine($"--- {year} ---");
        foreach (var h in holidayData[year])
        {
            Console.WriteLine($"{h.date} - {h.localName}");
        }
    }
}
