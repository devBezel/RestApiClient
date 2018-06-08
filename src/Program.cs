using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ToDoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            menu();
            
        }

        private static void menu()
        {
            Console.WriteLine("1. Pokaz zadania");
            Console.WriteLine("2. Dodaj zadania");
            int anwser = int.Parse(Console.ReadLine());
            if(anwser == 1)
            {
                Console.Clear();
                getList().Wait();
                Console.ReadKey();
                Console.Clear();
                menu();

            }
            else if (anwser == 2)
            {
                Console.Clear();
                postList().Wait();
                Console.ReadKey();
                Console.Clear();
                menu();
                  
            }
        }

        static async Task getList()
        {
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri("http://localhost:50431/");
                cl.DefaultRequestHeaders.Accept.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await cl.GetAsync("api/List");
                if(res.IsSuccessStatusCode)
                {
                    List<ListTo> list = await res.Content.ReadAsAsync<List<ListTo>>();
                    for(int i = 0; i < list.Count; i++)
                    {
                        Console.WriteLine("ID: {0}\nTytuł: {1}\nOpis: {2}\nPriorytet: {3}\n_______________",list[i].id, list[i].title, list[i].text, list[i].priority);
                    }
                }
            }
        }
        static async Task postList()
        {
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri("http://localhost:50431/");
                cl.DefaultRequestHeaders.Accept.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res;

                ListTo newList = new ListTo();
                Console.Write("Tytuł: ");
                string title = Console.ReadLine();
                Console.Write("Opis: ");
                string description = Console.ReadLine();
                Console.Write("Priorytet: ");
                string priority = Console.ReadLine();

                newList.title = title;
                newList.text = description;
                newList.priority = priority;

                res = await cl.PostAsJsonAsync("api/List", newList);
                if (res.IsSuccessStatusCode)
                {
                    Uri listUrl = res.Headers.Location;
                    Console.WriteLine(listUrl);
                }
                else
                {
                    Console.WriteLine("Bląd");
                }
            }
        }
    }
}
