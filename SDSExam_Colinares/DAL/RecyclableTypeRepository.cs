using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SDSExam_Colinares.Models;

namespace SDSExam_Colinares.DAL
{
    public class RecyclableTypeRepository
    {
        public List<RecyclableType> GetAll()
        {
            var list = new List<RecyclableType>();
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableType_GetAll", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(Map(dr));
                }
            }
            return list;
        }

        public RecyclableType GetById(int id)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableType_GetById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read()) return Map(dr);
                }
            }
            return null;
        }

        public int Insert(RecyclableType model)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableType_Insert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", model.Type);
                cmd.Parameters.AddWithValue("@Rate", model.Rate);
                cmd.Parameters.AddWithValue("@MinKg", model.MinKg);
                cmd.Parameters.AddWithValue("@MaxKg", model.MaxKg);
                var outId = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outId);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outId.Value;
            }
        }

        public void Update(RecyclableType model)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableType_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Type", model.Type);
                cmd.Parameters.AddWithValue("@Rate", model.Rate);
                cmd.Parameters.AddWithValue("@MinKg", model.MinKg);
                cmd.Parameters.AddWithValue("@MaxKg", model.MaxKg);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableType_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private RecyclableType Map(SqlDataReader dr)
        {
            return new RecyclableType
            {
                Id = (int)dr["Id"],
                Type = dr["Type"].ToString(),
                Rate = (decimal)dr["Rate"],
                MinKg = (decimal)dr["MinKg"],
                MaxKg = (decimal)dr["MaxKg"]
            };
        }
    }
}