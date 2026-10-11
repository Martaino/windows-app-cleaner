using System.Diagnostics;
using System.Dynamic;

List<string> apps = new List<string> {
    "Solitaire Collection",
    "Xbox",
    "Weather",
    "Todos",
    "Clipchamp"
};

List<bool> selected = Enumerable.Repeat(false, apps.Count).ToList();

while (true)
{
    Console.WriteLine("\n=== Windows App Cleaner ===");
    Console.WriteLine("1. List and Select Applications");
    Console.WriteLine("2. Delete Selected Applications");
    Console.WriteLine("3. Exit");
    Console.Write("Choose an option: ");

    string? choice = Console.ReadLine();

    int len = apps.Count;

    if (choice == "1")
    {

        while (true)
        {
            // List out current apps
            Console.WriteLine("\nApplications:");

            for (int i = 0; i < len; i++)
            {
                Console.WriteLine($"{i + 1}. {apps[i]} {selected[i]}");
            }

            // Get user to pick which apps to remove
            Console.WriteLine("Choose which applications to remove: ");
            string? listInput = Console.ReadLine();

            if (listInput == null)
            {
                System.Console.WriteLine("Invalid Input");
                continue;
            }

            if (!int.TryParse(listInput, out int selection))
            {
                Console.WriteLine("Invalid Input");
                continue;
            }

            if (selection == 0)
            {
                break;
            }
            else if (selection < 1 || selection > len)
            {
                Console.WriteLine("Invalid Input");
                continue;
            }
            else
            {
                selected[selection - 1] = !selected[selection - 1];
            } 
        }
    }
    else if (choice == "2")
    {
        for (int i = apps.Count - 1; i >= 0; i--)
        {
            if (selected[i] == true)
            {
                apps.RemoveAt(i);
                selected.RemoveAt(i);
            }
        }
    }
    else if (choice == "3")
    {
        Console.WriteLine("Goodbye!");
        break;
    }
    else
    {
        Console.WriteLine("Invalid option.");
    }
}

class App
{
    public string Name { get; init; } = "";
    public string PackageName { get; init; } = "";
    public bool Selected { get; set;}
}