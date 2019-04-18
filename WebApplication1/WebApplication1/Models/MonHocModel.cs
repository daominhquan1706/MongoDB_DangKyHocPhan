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
    public class MonHocModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<MonHoc> MonHocCollection;
        public MonHocModel()
        {
            mongoClient = new MongoClient("mongodb://localhost");
            var db = mongoClient.GetDatabase("DangKyHocPhan");
            MonHocCollection = db.GetCollection<MonHoc>("MonHoc");
        }
        public List<MonHoc> findAll()
        {
            return MonHocCollection.AsQueryable<MonHoc>().ToList();
        }
        public MonHoc find(string id)
        {
            var MonHocId = new ObjectId(id);
            return MonHocCollection.AsQueryable<MonHoc>().SingleOrDefault(a => a.Id == MonHocId);
        }

        public bool isHocPhanThuocMonHoc(string idMonHoc ,string idHocPhan)
        {
            MonHocModel monHocModel = new MonHocModel();
            HocPhanModel hocPhanModel = new HocPhanModel();
            MonHoc monHoc = monHocModel.find(idMonHoc);
            foreach(HocPhan item in monHoc.DanhSachHocPhan)
            {
                if(item.Id.ToString() == idHocPhan)
                {
                    return true;
                }
            }
            return false;
        }

        public void create(MonHoc MonHoc)
        {
            MonHocCollection.InsertOne(MonHoc);
        }
        public void update(MonHoc MonHoc)
        {
            MonHocCollection.UpdateOne(
                Builders<MonHoc>.Filter.Eq("_id", ObjectId.Parse(MonHoc.Id.ToString())),
                Builders<MonHoc>.Update
                    .Set("MaMonHoc", MonHoc.MaMonHoc)
                    .Set("TenMonHoc", MonHoc.TenMonHoc)
                    .Set("DanhSachHocPhan", MonHoc.DanhSachHocPhan)
                );
        }
        
        public void delete(String id)
        {
            MonHocCollection.DeleteOne(Builders<MonHoc>.Filter.Eq("_id", ObjectId.Parse(id)));
        }

        public int ConLai(string idHocPhan,int SiSo)
        {
            AccountModel accountModel = new AccountModel();
            int conLai = SiSo;
            foreach(var item in accountModel.findAll())
            {
                foreach(var itemHocPhanDaDangKy in item.HocPhanDaDangKy)
                {
                    if (itemHocPhanDaDangKy == idHocPhan)
                    {
                        conLai = conLai -1;
                    }
                }
            }

            return conLai;
        }
    }
}