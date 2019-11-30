using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities;

namespace TicketsDemo.MDBSET
{
    class Program
    {
        public static int placeid = 0;
        static void Main(string[] args)
        {

            SaveDocs().GetAwaiter().GetResult();
            Console.WriteLine("successful!");
            Console.ReadKey();

        }
        private static async Task SaveDocs()
        {
            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("TicketsContextDb");
            var collection = database.GetCollection<Train>("TrainsCollection");

            Func<Carriage, List<Place>> placeGenerator = (Carriage car) =>
            {
                var retIt = new List<Place>();
                Random random = new Random();
                for (int i = 0; i < 100; i++)
                {
                    decimal randomNumber = random.Next(80, 120);
                    var newPlace = new Place() { Id = placeid, Number = i, PriceMultiplier = randomNumber / 100, CarriageId = car.Id, Carriage = car };
                    retIt.Add(newPlace);
                    placeid++;
                }
                placeid = 0;
                return retIt;
            };

            List<Train> needToAdd = new List<Train>
            {
               new Train
              {
                  Id =1,
                  Number = 90,
                  StartLocation = "Kiev",
                  EndLocation = "Odessa",
                 

                  Carriages = new List<Carriage>() {
                   new Carriage() {

                          Id=1,
                       //   Places = placeGenerator(),
                          Type = CarriageType.SecondClassSleeping,
                          DefaultPrice = 100m,
                          Number = 1,
                      },new Carriage() {
                          Id=2,
                         // Places = placeGenerator(),
                          Type = CarriageType.SecondClassSleeping,
                          DefaultPrice = 100m,
                          Number = 2,
                      },new Carriage() {
                          Id=3,
                      //    Places = placeGenerator(),
                          Type = CarriageType.FirstClassSleeping,
                          DefaultPrice = 120m,
                          Number = 3,
                      },new Carriage() {
                          Id=4,
                      //    Places = placeGenerator(),
                          Type = CarriageType.FirstClassSleeping,
                          DefaultPrice = 130m,
                          Number = 4,
                      }
                  }
              },
              new Train
              {
                  Id =2,
                  Number = 720,
                  StartLocation = "Kiev",
                  EndLocation = "Vinnitsa",
                  
                  Carriages = new List<Carriage>() {
                      new Carriage() {
                          Id=1,
                       //   Places = placeGenerator(),
                          Type = CarriageType.Sedentary,
                          DefaultPrice = 40m,
                          Number = 1,
                      },new Carriage() {
                          Id=2,
                        //  Places = placeGenerator(),
                          Type = CarriageType.Sedentary,
                          DefaultPrice = 40m,
                          Number = 2,
                      },new Carriage() {
                          Id=3,
                         // Places = placeGenerator(),
                          Type = CarriageType.Sedentary,
                          DefaultPrice = 40m,
                          Number = 3,
                      }
                  }
              }
            };
            foreach (Train train in needToAdd)
            {
                foreach (Carriage car in train.Carriages)
                {
                    car.Places = placeGenerator(car);

                }
            }
            collection.InsertMany(needToAdd);
            Console.WriteLine("So, we are here))");
            Console.Read();
        }


        }
    }
