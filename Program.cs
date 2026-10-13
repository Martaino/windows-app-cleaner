using System.Diagnostics;
using System.Dynamic;

// Empty list of apps that holds all user apps
List<App> apps = await AppScanner.GetInstalledAppsAsync();

while (true)
{
    Console.WriteLine("\n=== Windows App Cleaner ===");
    Console.WriteLine("1. List and Select Applications");
    Console.WriteLine("2. Delete Selected Applications");
    Console.WriteLine("3. Exit");
    Console.Write("Choose an option: ");

    string? choice = Console.ReadLine();

    if (choice == "1")
    {

        while (true)
        {
            // List out current apps
            Console.WriteLine("\nApplications:");

            for (int i = 0; i < apps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {apps[i].Name} {apps[i].Selected}");
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
            else if (selection < 1 || selection > apps.Count)
            {
                Console.WriteLine("Invalid Input");
                continue;
            }
            else
            {
                apps[selection - 1].Selected = !apps[selection - 1].Selected;
            } 
        }
    }
    else if (choice == "2")
    {
        for (int i = apps.Count - 1; i >= 0; i--)
        {
            if (apps[i].Selected == true)
            {
                apps.RemoveAt(i);
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