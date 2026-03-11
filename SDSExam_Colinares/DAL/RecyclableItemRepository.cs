using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SDSExam_Colinares.Models;

namespace SDSExam_Colinares.DAL
{
    public class RecyclableItemRepository
    {
        public List<RecyclableItem> GetAll()
        {
            var list = new List<RecyclableItem>();
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableItem_GetAll", conn))
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

        public RecyclableItem GetById(int id)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableItem_GetById", conn))
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

        public int Insert(RecyclableItem model)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableItem_Insert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RecyclableTypeId", model.RecyclableTypeId);
                cmd.Parameters.AddWithValue("@ItemDescription", model.ItemDescription);
                cmd.Parameters.AddWithValue("@Weight", model.Weight);
                var outId = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outId);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outId.Value;
            }
        }

        public void Update(RecyclableItem model)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableItem_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@RecyclableTypeId", model.RecyclableTypeId);
                cmd.Parameters.AddWithValue("@ItemDescription", model.ItemDescription);
                cmd.Parameters.AddWithValue("@Weight", model.Weight);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_RecyclableItem_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private RecyclableItem Map(SqlDataReader dr)
        {
            return new RecyclableItem
            {
                Id = (int)dr["Id"],
                RecyclableTypeId = (int)dr["RecyclableTypeId"],
                RecyclableTypeName = dr["RecyclableTypeName"].ToString(),
                ItemDescription = dr["ItemDescription"].ToString(),
                Weight = (decimal)dr["Weight"],
                ComputedRate = dr["ComputedRate"] == DBNull.Value ? 0 : (decimal)dr["ComputedRate"]
            };
        }
    }
}