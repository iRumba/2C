﻿using Core.Adapters;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public abstract class BaseRepository<TModel> where TModel : BaseModel
    {
        protected string _connectionString;
        //protected BaseAdapter<TModel> _adapter;

        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection GetConnection()
        {
            var res = new SqlConnection(_connectionString);
            return res;
        }

        public async Task<TModel> GetById(int id)
        {
            var parameters = new List<SqlParameter>();
            var query = $@"{GetSimpleQuery()} WHERE {ModelHelper.GetIdFieldName<TModel>()}=@id";
            var param = new SqlParameter($"@id", SqlDbType.Int)
            {
                Value = id
            };
            parameters.Add(param);
            return await QueryToModel(query, parameters);
        }

        public async Task<List<TModel>> GetByIds(IEnumerable<int> ids)
        {
            var parameters = new List<SqlParameter>();
            var sb = new StringBuilder($"{GetSimpleQuery()} WHERE ");
            if (!ids.Any())
                sb.Append("0=1");
            else
            {
                var counter = 0;

                foreach (var id in ids)
                {
                    if (counter > 0)
                        sb.Append(" OR ");
                    var param = new SqlParameter($"@id{counter}", SqlDbType.Int)
                    {
                        Value = id
                    };
                    parameters.Add(param);
                    sb.Append($"{ModelHelper.GetIdFieldName<TModel>()}=@id{counter}");
                    counter++;
                }
            }

            return await QueryToModelList(sb.ToString(), parameters);
        }

        public async Task<List<TModel>> GetAll()
        {
            var parameters = new List<SqlParameter>();
            var query = $@"{GetSimpleQuery()}";
            return await QueryToModelList(query, parameters);
        }

        public async Task<TModel> Add(TModel model)
        {
            var parameters = new List<SqlParameter>();
            var sb = new StringBuilder(GetSimpleInsert());
            var counter = 0;
            foreach(var mi in ModelHelper.GetMappingInfo<TModel>().Where(info=> info.FieldName != ModelHelper.GetIdFieldName<TModel>()))
            {
                if (counter == 0)
                    sb.Append(" VALUES (");
                var pName = $"@p{counter}";
                object value = null;
                switch (mi.MappingType)
                {
                    case Utils.MappingType.Direct:
                        value = mi.Property.GetValue(model);
                        break;
                    case Utils.MappingType.ForeignKey:
                        var fModel = mi.Property.GetValue(model) as TModel;
                        if (fModel != null)
                            value = fModel.Id;
                        break;
                }
                var param = new SqlParameter(pName, value ?? DBNull.Value);
                parameters.Add(param);
                sb.Append($"{pName},");
                counter++;
            }
            if (counter > 0)
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")");
            }
            var res = await QueryToModel(sb.ToString(), parameters);
            return res;
        }

        public async Task<List<TModel>> Add(List<TModel> models)
        {
            var parameters = new List<SqlParameter>();
            var sb = new StringBuilder(GetSimpleInsert());
            var preCounter = 0;
            foreach(var model in models)
            {
                if (preCounter == 0)
                    sb.Append(" VALUES ");
                else
                    sb.Append(",");
                var counter = 0;
                foreach (var mi in ModelHelper.GetMappingInfo<TModel>().Where(info => info.FieldName != ModelHelper.GetIdFieldName<TModel>()))
                {
                    if (counter == 0)
                        sb.Append("(");
                    var pName = $"@p{preCounter}_{counter}";
                    object value = null;
                    switch (mi.MappingType)
                    {
                        case Utils.MappingType.Direct:
                            value = mi.Property.GetValue(model);
                            break;
                        case Utils.MappingType.ForeignKey:
                            var fModel = mi.Property.GetValue(model) as TModel;
                            if (fModel != null)
                                value = fModel.Id;
                            break;
                    }
                    var param = new SqlParameter(pName, value ?? DBNull.Value);
                    parameters.Add(param);
                    sb.Append($"{pName},");
                    counter++;
                }
                if (counter > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");
                }
                preCounter++;
            }
            var res = await QueryToModelList(sb.ToString(), parameters);
            return res;
        }

        public async Task<bool> Update(TModel model)
        {
            if (model.Id < 1)
                throw new InvalidOperationException("Нельзя изменить объект, так как он не существует");

            var parameters = new List<SqlParameter>();
            var sb = new StringBuilder($"UPDATE {ModelHelper.GetModelTableName<TModel>()} SET ");
            var idFieldName = ModelHelper.GetIdFieldName<TModel>();
            var counter = 0;
            foreach (var mi in ModelHelper.GetMappingInfo<TModel>().Where(info => info.FieldName != idFieldName))
            {
                if (counter > 0)
                    sb.Append(",");
                var pName = $"@{mi.FieldName}";
                sb.Append($"{mi.FieldName}={pName}");
                object value = null;
                switch (mi.MappingType)
                {
                    case Utils.MappingType.Direct:
                        value = mi.Property.GetValue(model);
                        break;
                    case Utils.MappingType.ForeignKey:
                        var fModel = mi.Property.GetValue(model) as TModel;
                        if (fModel != null)
                            value = fModel.Id;
                        break;
                }
                var param = new SqlParameter(pName, value ?? DBNull.Value);
                parameters.Add(param);
                counter++;
            }
            parameters.Add(new SqlParameter($"@{idFieldName}", model.Id));
            sb.Append($" WHERE {idFieldName}=@{idFieldName}");
            var res = await ExecuteQuery(sb.ToString(), parameters);
            return res;
        }

        protected async Task<bool> ExecuteQuery(string query, IEnumerable<SqlParameter> parameters)
        {
            using (var con = GetConnection())
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandType = CommandType.Text;
                    com.CommandText = query;
                    com.Parameters.AddRange(parameters.ToArray());
                    return await com.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        protected async Task<TModel> QueryToModel(string query, IEnumerable<SqlParameter> parameters)
        {
            using (var con = GetConnection())
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandType = CommandType.Text;
                    com.CommandText = query;
                    com.Parameters.AddRange(parameters.ToArray());
                    var adapter = GetAdapter();
                    var res = await adapter.GetModelFromDataSetAsync(await com.ToDataSetAsync());
                    return res;
                }
            }
        }

        protected async Task<List<TModel>> QueryToModelList(string query, IEnumerable<SqlParameter> parameters)
        {
            using (var con = GetConnection())
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandType = CommandType.Text;
                    com.CommandText = query;
                    com.Parameters.AddRange(parameters.ToArray());
                    var adapter = GetAdapter();
                    var res = await adapter.GetModelListFromDataSetAsync(await com.ToDataSetAsync());
                    return res;
                }
            }
        }

        internal BaseAdapter<TModel> GetAdapter()
        {
            return AdaptersManager.GetAdapter<TModel>();
        }

        protected virtual string GetSimpleQuery()
        {
            return $"SELECT {GetFieldsString()} FROM {ModelHelper.GetModelTableName<TModel>()}";
        }

        protected virtual string GetSimpleInsert()
        {
            var fields = GetFields();
            var idField = ModelHelper.GetIdFieldName<TModel>();
            var fsb = new StringBuilder($"INSERT INTO {ModelHelper.GetModelTableName<TModel>()}");
            var fso = new StringBuilder($"OUTPUT INSERTED.{idField}");
            var counter = 0;
            foreach(var field in fields)
            {
                if (counter++ == 0)
                    fsb.Append(" (");
                fso.Append($",INSERTED.{field}");
                if (field == idField)
                    continue;
                fsb.Append($"{field},");
            }
            if (counter > 0)
            {
                fsb.Remove(fsb.Length - 1, 1);
                fsb.Append(")");
            }
            return $"{fsb} {fso}";
        }

        protected virtual IEnumerable<string> GetFields()
        {
            return ModelHelper.GetMappingInfo<TModel>().Select(mi => mi.FieldName);
            return new string[] { ModelHelper.GetIdFieldName<TModel>() };
        }

        protected string GetFieldsString()
        {
            return string.Join(",", GetFields());
        }
    }
}
