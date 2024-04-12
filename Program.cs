using System;
using System.Globalization;

namespace AssetTrackingV2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of MyDbContext
            using (var context = new MyDbContext())
            {

                bool exit = false;

                while (!exit)
                {
                    // Display menu options
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Menu:");
                    Console.WriteLine("Press 'C' to add an asset to the table");
                    Console.WriteLine("Press 'R' to print the table");
                    Console.WriteLine("Press 'U' to update an asset in the table");
                    Console.WriteLine("Press 'D' to delete an asset from the table");
                    Console.WriteLine("Press 'L' to Generate Asset Summary Report");
                    Console.WriteLine("Press 'X' to exit the program");

                    // Read user input
                    Console.Write("Your choice: ");
                    Console.WriteLine("----------------------------------------");
                    string choice = Console.ReadLine().ToUpper();

                    // Perform action based on user input
                    switch (choice)
                    {
                        case "C":
                            AddAsset(context);
                            break;
                        case "R":
                            PrintAssets(context);
                            break;
                        case "U":
                            UpdateAsset(context);
                            break;
                        case "D":
                            DeleteAssetMenu(context);
                            break;
                        case "L":
                            GenerateAssetSummaryReport(context);
                            break;
                        case "X":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
        }

        // Method to add an asset to the table
        static void AddAsset(MyDbContext context)
        {
            Console.WriteLine("Enter asset information:");
            Console.Write("Type: ");
            string type = Console.ReadLine();

            Console.Write("Brand: ");
            string brand = Console.ReadLine();

            Console.Write("Model: ");
            string model = Console.ReadLine();

            Console.Write("Office: ");
            string office = Console.ReadLine();

            string purchDate;
            do
            {
                Console.Write("Purchase Date (yyyy-MM-dd): ");
                purchDate = Console.ReadLine();
            } while (!DateTime.TryParseExact(purchDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));

            int inUSD;
            do
            {
                Console.Write("In USD: ");
            } while (!int.TryParse(Console.ReadLine(), out inUSD));

            Console.Write("LocCurr: ");
            string locCurr = Console.ReadLine();

            // Automatically adjust LocPrice based on LocCurr and inUSD values
            int locPrice;
            switch (locCurr.ToUpper())
            {
                case "USD":
                    locPrice = inUSD * 1;
                    break;
                case "EUR":
                    locPrice = inUSD * 2;
                    break;
                case "SEK":
                    locPrice = inUSD * 10;
                    break;
                default:
                    locPrice = inUSD; // Default to inUSD if LocCurr is not recognized
                    break;
            }

            // Create a new Asset object with the validated input
            Asset newAsset = new Asset
            {
                Type = type,
                Brand = brand,
                Model = model,
                Office = office,
                PurchDate = purchDate,
                inUSD = inUSD,
                LocCurr = locCurr,
                LocPrice = locPrice
            };

            // Add the new asset to the DbSet
            context.Assets.Add(newAsset);
            context.SaveChanges();

            Console.WriteLine("Asset added successfully.");
        }

        // Method to print the assets table sorted by Office and PurchDate
        static void PrintAssets(MyDbContext context)
        {
            // Retrieve assets from the database sorted by Office and then by PurchDate
            var sortedAssets = context.Assets.OrderBy(a => a.Office).ThenBy(a => a.PurchDate);

            Console.WriteLine("Assets Table:");
            Console.WriteLine("ID\tType\t\tBrand\t\tModel\t\tOffice\t\tPurchDate\t\tinUSD\tLocCurr\tLocPrice");

            foreach (var asset in sortedAssets)
            {
                // Calculate the difference in days between the purchase date and the current date
                TimeSpan difference = DateTime.Now - DateTime.Parse(asset.PurchDate);
                int daysDifference = (int)difference.TotalDays;

                // Set text color based on the difference in days
                if (daysDifference > 990)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (daysDifference > 900)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ResetColor(); // Reset color if difference is within range
                }

                // Print the asset row
                Console.WriteLine($"{asset.Id,-4}\t{asset.Type,-15}\t{asset.Brand,-15}\t{asset.Model,-15}\t{asset.Office,-15}\t{asset.PurchDate,-15}\t{asset.inUSD,-6}\t{asset.LocCurr,-8}\t{asset.LocPrice}");
            }

            // Reset text color after printing all rows
            Console.ResetColor();
        }


        // Method to update an asset in the table
        static void UpdateAsset(MyDbContext context)
        {
            //Print table first
            PrintAssets(context);

            Console.Write("Enter the ID of the asset to update: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("Invalid ID. Please enter a valid integer: ");
            }

            // Find the asset by ID
            var assetToUpdate = context.Assets.FirstOrDefault(a => a.Id == id);

            if (assetToUpdate == null)
            {
                Console.WriteLine("Asset not found.");
                return;
            }
            Console.WriteLine("Enter updated asset details:");

            Console.Write("Type: ");
            string type = Console.ReadLine();

            Console.Write("Brand: ");
            string brand = Console.ReadLine();

            Console.Write("Model: ");
            string model = Console.ReadLine();

            Console.Write("Office: ");
            string office = Console.ReadLine();

            string purchDate;
            do
            {
                Console.Write("Purchase Date (yyyy-MM-dd): ");
                purchDate = Console.ReadLine();
            } while (!DateTime.TryParseExact(purchDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));

            int inUSD;
            do
            {
                Console.Write("In USD: ");
            } while (!int.TryParse(Console.ReadLine(), out inUSD));

            Console.Write("LocCurr: ");
            string locCurr = Console.ReadLine();

            int locPrice;
            do
            {
                Console.Write("LocPrice: ");
            } while (!int.TryParse(Console.ReadLine(), out locPrice));

            // Update the asset with the validated input
            assetToUpdate.Type = type;
            assetToUpdate.Brand = brand;
            assetToUpdate.Model = model;
            assetToUpdate.Office = office;
            assetToUpdate.PurchDate = purchDate;
            assetToUpdate.inUSD = inUSD;
            assetToUpdate.LocCurr = locCurr;
            assetToUpdate.LocPrice = locPrice;

            // Save changes to the database
            context.SaveChanges();

            Console.WriteLine($"Asset with ID {id} updated successfully.");

        }
        // Method to prompt user for ID and delete the corresponding asset
        static void DeleteAssetMenu(MyDbContext context)
        {
            // Prompt user for the ID of the asset to delete
            Console.Write("Enter the ID of the asset to delete: ");
            int id;
            if (int.TryParse(Console.ReadLine(), out id))
            {
                DeleteAsset(context, id);
            }
            else
            {
                Console.WriteLine("Invalid ID. Please enter a valid integer.");
            }
        }
        // Method to delete an asset by ID
        static void DeleteAsset(MyDbContext context, int id)
        {
            // Find the asset by ID
            var assetToDelete = context.Assets.FirstOrDefault(a => a.Id == id);

            if (assetToDelete != null)
            {
                // Remove the asset from the DbSet
                context.Assets.Remove(assetToDelete);

                // Save changes to the database
                context.SaveChanges();

                Console.WriteLine($"Asset with ID {id} deleted successfully.");
            }
            else
            {
                Console.WriteLine("Asset not found.");
            }
        }
        // Method to generate an Asset Summary Report
        static void GenerateAssetSummaryReport(MyDbContext context)
        {
            // Retrieve all assets from the database
            var allAssets = context.Assets.ToList();

            // Calculate total number of assets
            int totalAssets = allAssets.Count;

            // Calculate total value of assets in USD
            decimal totalValueUSD = allAssets.Sum(a => a.inUSD);

            // Calculate average value of assets per office
            var assetsPerOffice = allAssets.GroupBy(a => a.Office).Select(g => new
            {
                Office = g.Key,
                TotalValueUSD = g.Sum(a => a.inUSD),
                NumAssets = g.Count()
            });

            // Print Asset Summary Report
            Console.WriteLine("Asset Summary Report:");
            Console.WriteLine($"Total number of assets: {totalAssets}");
            Console.WriteLine($"Total value of assets (in USD): {totalValueUSD}");

            Console.WriteLine("\nAssets per Office:");
            Console.WriteLine("Office\t\tNum Assets\tTotal Value (USD)");

            foreach (var office in assetsPerOffice)
            {
                Console.WriteLine($"{office.Office,-15}\t{office.NumAssets,-11}\t{office.TotalValueUSD}");
            }
        }

    }
}
