using Jupiter_Task.Models;
using System.Text.Json;

namespace Jupiter_Task.Data
{
    public class SeedData
    {
        public static async Task SeedDataAsync(AppDbContext context)
        {
            if (!context.Employees.Any())
            {
                // reading from json file
                var empData =
                    File.ReadAllText("./Data/EmpData.json");
                //deserilzeing the data to C# list
                var data = JsonSerializer.Deserialize<List<Employee>>(empData);
                // adding the data to database
                foreach (var item in data)
                {
                    context.Employees.Add(item);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
