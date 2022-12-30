namespace rlask_engine
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            string help = "Komentot: \n" +
                "l - Kaikkien laskutietojen hakeminen ja listaaminen\n" +
                "o - Yksittäisen laskun laskutietojen hakeminen ja listaaminen\n" +
                "a - Yksittäisen asiakkaan kaikkien laskujen hakeminen ja listaaminen\n" +
                "t - Kaikkien järjestelmään tallennettujen tuotetietojen listaaminen\n" +
                "i - Laskutietojen hakeminen tuotteen perusteella\n" +
                "s - Testidatan tallentaminen tietokantaan";

            Console.WriteLine(help);
           
            do
            {
                Console.Write("> ");
                var command = Console.ReadLine();
                switch (command)
                {
                    
                    case "l":
                        Console.WriteLine(Commands.ListAllInvoices());
                        break;
                    case "o":
                        Console.WriteLine(Commands.GetInvoiceById());
                        break;
                    case "a":
                        Console.WriteLine(Commands.ListInvoicesByCustomerId());
                        break;
                    case "t":
                        Console.WriteLine(Commands.ListAllProducts());
                        break;
                    case "i":
                        Console.WriteLine(Commands.ListInvoicesByProductId());
                        break;
                    case "s":
                        Commands.SeedTestData();
                        break;
                    default:
                        Console.WriteLine(help);
                        break;
                }
            } while (true);
        }
    }
}