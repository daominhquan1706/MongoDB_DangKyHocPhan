﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebApplication1.Entities;

namespace WebApplication1.Models
{
    public class AccountModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<Account> accountCollection;
        public AccountModel()
        {
            mongoClient = new MongoClient("mongodb://localhost");
            var db = mongoClient.GetDatabase("DangKyHocPhan");
            accountCollection = db.GetCollection<Account>("account");
        }
        public List<Account> findAll()
        {
            return accountCollection.AsQueryable<Account>().ToList();
        }
        public Account find(string id)
        {
            var accountId = new ObjectId(id);
            return accountCollection.AsQueryable<Account>().SingleOrDefault(a => a.Id == accountId);


        }
        public void create(Account account)
        {
            accountCollection.InsertOne(account);
        }
        public void update(Account account)
        {
            accountCollection.UpdateOne(
                Builders<Account>.Filter.Eq("_id", ObjectId.Parse(account.Id.ToString())),
                Builders<Account>.Update
                    .Set("username", account.Username)
                    .Set("password", account.Password)
                    .Set("fullname", account.Fullname)
                    .Set("status", account.Status)
                );
        }
        public void delete(String id)
        {
            accountCollection.DeleteOne(Builders<Account>.Filter.Eq("_id", ObjectId.Parse(id)));
        }

    }
}