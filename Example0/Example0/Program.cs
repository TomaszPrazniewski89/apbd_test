using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Example0
{
    class Program
    {   //We usually use args parameter to pass the data to the main method
        static async Task Main(string[] args)
        {
            //HttpClient httpClient = new HttpClient();
            //HttpResponseMessage result = await httpClient.GetAsync("https://www.olx.pl/nieruchomosci/lipno");

            //if (result.IsSuccessStatusCode)
            //{
            //    string content = await result.Content.ReadAsStringAsync();
            //    Console.WriteLine(result);
            //}

            //Console.WriteLine("Test");


            if (args.Length == 0)
            {
                throw new ArgumentException("Youd didn;t pass the first parameter");
            }
            //TryCreate is a static method
            //Czy url jest poprawne ?? //first parametter "args[0] is a string so we have to parse it into uri object
            // UriKind.Absolute( what kind or uri)
            // out Uri uriResult (out parameter) allow us later on to to work with parsed uri using variable uriREsult
            //   REturned type                                  Out parameter
            //Uri result(34 linia) zwraca nam zmienna z parsowanym adres uri (ala deklaracja zmiennej) z ktorej kozystamy w lini 35
            bool result = Uri.TryCreate(args[0], UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
            {
                throw new AccessViolationException("URL is not a correct one");
            }
            //Console.WriteLine("your url is " + uriResult);
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uriResult);


            if (response.IsSuccessStatusCode)
                //response.StatusCode == HttpStatusCode.OK
            {
                var content =await  response.Content.ReadAsStringAsync();
                //Console.WriteLine(content);
                //create better Regex
                Regex telRegex = new Regex(" [a-z0-9!#$]+@[a-z.]+");
                var matches = telRegex.Matches(content);

                foreach( var i in matches)
                {
                    Console.WriteLine(i);
                }
                //jesli sa duplikaty emaili wyswietlamy w spsoob unikalny
                //Make sure that we print emails in unique way
                //HashSet<String) <--collects only unique values

                       
            }
            else
            {
                Console.WriteLine("Error during the request");
            }

            //good practice
            //ale jak bedzie usadowiona na koncu kodu to jest mozliwe, ze nigdy sie nie wykona
            // najlepiej uzyc try catch finally albo w c# za pomoca instrukcji using (linia 42 using var httpClient = new HttpClient();) wtedy dispose sie wykona na tym obiekcie
            // dispose uzywa sie aby zwolnic zasoby w sposob bezpieczny
            httpClient.Dispose();

        }
    }
}
