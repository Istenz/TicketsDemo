using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;

namespace TicketsDemo.MongoDB
{
    class TicketsContext
    {
        public MongoClient mongoClient { get; set; }
        public IMongoDatabase mongoDB { get; set; }

        public IMongoCollection<Train> Trains
        {
            get { return mongoDB.GetCollection<Train>("TrainsCollection"); }
        }
        public TicketsContext()
        {
            mongoClient = new MongoClient("mongodb://localhost");
            mongoDB = mongoClient.GetDatabase("TicketsContextDb");

        }
    }
}