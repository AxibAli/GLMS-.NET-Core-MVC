//using APIModels.Service;
//using Database;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web.Mvc;

//namespace WhatsappMiddlewareAPI.Controllers
//{
//    [AuthorizeAuthenticate]
//    public class CrudController : Controller
//    {
//        readonly DatabaseContext db;
//        //readonly IRepository<LocationInfo, Database.DatabaseContext> locationInfos;
//        public CrudController(
//           DatabaseContext db_
//          )
//        {
//            this.db = db_;
//        }

//        string getPartialViewString = string.Empty;
//        AjaxResponse ajaxResponse;
//        // GET: Crud
//        public ActionResult Index()
//        {
//            return View();
//        }

//        public ActionResult ListOfTables()
//        {
//            return View();
//        }

//        // Filling table dropdown
//        public ActionResult Gettables()
//        {
//            try
//            {
//                using (var db = new DatabaseContext())
//                {
//                    List<TableSchema> results = db.Database.SqlQuery<TableSchema>("select schema_name(tab.schema_id) as [SchemaName], tab.[name] as TableName, pk.[name] as PkName, substring(column_names, 1, len(column_names)-1) as [ColumnName] from sys.tables tab left outer join sys.indexes pk on tab.object_id = pk.object_id and pk.is_primary_key = 1 cross apply(select top 1 col.[name] +', ' from sys.index_columns ic inner join sys.columns col on ic.object_id = col.object_id and ic.column_id = col.column_id where ic.object_id = tab.object_id and ic.index_id = pk.index_id order by col.column_id for xml path ('') ) D(column_names)  order by schema_name(tab.schema_id), tab.[name]").ToList();
//                    db.Dispose();
//                    ajaxResponse =
//                    new AjaxSuccess<List<TableSchema>>()
//                    {
//                        Data = results,
//                        Response = true,
//                        Status = true
//                    };
//                }



//            }
//            catch (Exception e)
//            {
//                ajaxResponse = AjaxError.CatchError(e);
//            }
//            return Json(ajaxResponse, JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult GettableHeadings(string tableName, string schemaName)
//        {
//            try
//            {

//                using (var db = new DatabaseContext())
//                {
//                    List<string> results =
//                        db.Database.SqlQuery<string>($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='{tableName}' AND TABLE_SCHEMA='{schemaName}' ").ToList();

//                    ajaxResponse =
//                    new AjaxSuccess<List<string>>()
//                    {
//                        Data = results,
//                        Response = true,
//                        Status = true
//                    };
//                }
//            }
//            catch (Exception e)
//            {
//                ajaxResponse = AjaxError.CatchError(e);
//            }
//            return Json(ajaxResponse, JsonRequestBehavior.AllowGet);
//        }



//        [HttpPost]
//        public ActionResult getTableData(
//           int PageSize,
//           int PageNo,
//           string formData,
//           string primaryKeyColumn,
//           string sorting,
//           string fromdate,
//           string Todate,
//           string tablename,
//           string schemaName
//           )
//        {
//            int totalPages = 1;
//            //int totalItems = 0;
//            try
//            {

//                string partialViewPath = "~/Views/Crud/PartialView/_TableList.cshtml";
//                List<string> result = null;
//                List<int> res = null;
//                var primarykey = "";
//                var whereclause = "";
//                bool loadwhere = true;

//                if (string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(Todate))
//                {
//                    loadwhere = false;
//                }

//                using (var db = new DatabaseContext())
//                {
//                    List<string> results =
//                        db.Database.SqlQuery<string>($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='{tablename}' AND TABLE_SCHEMA='{schemaName}' ").ToList();

//                    if (results.Contains("CreatedOn") && loadwhere)
//                        whereclause = $"where CreatedOn between '{fromdate}' AND '{Todate}'  ";
//                }

//                //using (var db = new DatabaseContext())
//                //{
//                //    result =
//                //       db.Database.SqlQuery<string>($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = '{formData}'").ToList();

//                //    db.Dispose();

//                //}



//                //if (result.Count > 0) 
//                {
//                    primarykey = $"order by {primaryKeyColumn} {sorting}";
//                }

//                using (var db = new DatabaseContext())
//                {
//                    res =
//                       db.Database.SqlQuery<int>($"SELECT count(*) as count FROM {formData} {whereclause} ").ToList();

//                    db.Dispose();

//                }

//                int pageStart = (PageNo - 1) * PageSize;

//                string sqlQuery = $"SELECT * FROM {formData} {whereclause} {primarykey}  OFFSET {pageStart} ROWS FETCH NEXT {PageSize} ROWS ONLY ";
//                //string sqlQuery = $"SELECT * FROM {formData} ";

//                List<dynamic> Query = UtilityExtensionMethods.DynamicListFromSql(db, sqlQuery, new Dictionary<string, object> { { "a", true }, { "b", false } }).ToList();

//                //List<dynamic> getObjectList = null;
//                //var getObjectFilteredList = Query.Skip(pageStart).Take(PageSize).ToList();
//                var getObjectFilteredList = Query;


//                var getPartialViewString = Utility.RenderPartialToString(this, partialViewPath, getObjectFilteredList);
//                int findTotalPage = (int)Math.Ceiling((decimal)res[0] / (decimal)PageSize);
//                totalPages = (findTotalPage == 0) ? 1 : findTotalPage;

//                ajaxResponse =
//                    new AjaxSuccess<string>()
//                    {
//                        ListData = new ListData()
//                        {
//                            View = getPartialViewString,
//                            TotalPages = totalPages,
//                            TotalItems = res[0]
//                        },
//                        Response = true,
//                        Status = true
//                    };

//            }
//            catch (Exception e)
//            {
//                ajaxResponse = AjaxError.CatchError(e);
//            }
//            return Json(ajaxResponse);
//        }

//        [HttpPost]
//        public ActionResult getTableDataWithColumnType(string tableName, int id)
//        {
//            try
//            {
//                string sqlQuery = $"SELECT * FROM {tableName} where id={id} ";

//                List<Tuple<string, object, string>> result = UtilityExtensionMethods.DynamicListFromSql(db, sqlQuery).ToList();

//                //int pageStart = (PageNo - 1) * PageSize;
//                ////List<T> getObjectList = null;
//                //var getObjectFilteredList = Query.Skip(pageStart).Take(PageSize).ToList();


//                //var getPartialViewString = Utility.RenderPartialToString(this, partialViewPath, getObjectFilteredList);
//                //int findTotalPage = (int)Math.Ceiling((decimal)Query.Count() / (decimal)PageSize);
//                //totalPages = (findTotalPage == 0) ? 1 : findTotalPage;

//                ajaxResponse =
//                    new AjaxSuccess<List<Tuple<string, object, string>>>()
//                    {
//                        Data = result,
//                        Response = true,
//                        Status = true
//                    };

//            }
//            catch (Exception e)
//            {
//                ajaxResponse = AjaxError.CatchError(e);
//            }
//            return Json(ajaxResponse);
//        }


//        [HttpPost]
//        public ActionResult SaveTableData(List<EditTable> array, string tablename)
//        {
//            try
//            {

//                string query = $"update {tablename} set ";
//                bool firstrow = true;
//                long id = 0;
//                List<string> fkresult = null;
//                long urlid;
//                var replacename = "";

//                foreach (var item in array)
//                {
//                    if (tablename.Contains("dbo."))
//                    {
//                        replacename = tablename.Replace("dbo.", "");
//                    }

//                    string FkCheckQuery =
//                          ($"SELECT a.CONSTRAINT_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS b JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE a " +
//                                            $"ON a.CONSTRAINT_CATALOG = b.CONSTRAINT_CATALOG AND a.CONSTRAINT_NAME = b.CONSTRAINT_NAME " +
//                                            $"WHERE a.COLUMN_NAME = '{item.ColumnName}' And a.TABLE_NAME = '{replacename}'");

//                    using (var db = new DatabaseContext())
//                    {
//                        fkresult =
//                             db.Database.SqlQuery<string>(FkCheckQuery).ToList();

//                        db.Dispose();

//                    }
//                    if (item.ColumnName == "Id")
//                    {
//                        id = Convert.ToInt64(item.ColumnValue);
//                        continue;
//                    }

//                    if (fkresult.Count > 0)
//                    {
//                        urlid = Convert.ToInt64(item.ColumnValue);
//                        continue;
//                    }

//                    if (item.nullableData != "null")
//                    {

//                        if (firstrow)
//                        {
//                            query += $" {item.ColumnName}='{GetColumnDataForQuery(item.ColumnType, item.ColumnValue)}'";
//                            firstrow = false;
//                        }
//                        else
//                        {
//                            query += $" ,{item.ColumnName}='{GetColumnDataForQuery(item.ColumnType, item.ColumnValue)}'";
//                        }
//                    }

//                }

//                query += $" Where id={id}";

//                var result = 0;
//                string message = "";


//                using (var con = db.Database.Connection)
//                {
//                    using (var cmd = con.CreateCommand())
//                    {
//                        cmd.CommandText = query;
//                        if (cmd.Connection.State != ConnectionState.Open)
//                        {
//                            cmd.Connection.Open();
//                        }
//                        result = cmd.ExecuteNonQuery();
//                        cmd.Connection.Close();
//                    }
//                }

//                if (result > 0)
//                {
//                    message = "Record has been updated";
//                }
//                else
//                {
//                    message = "Record updation failed";
//                }

//                ajaxResponse =
//                  new AjaxSuccess<List<string>>()
//                  {
//                      Message = message,
//                      Response = true,
//                      Status = true
//                  };
//            }
//            catch (Exception e)
//            {
//                ajaxResponse = AjaxError.CatchError(e);
//            }
//            return Json(ajaxResponse);
//        }
//        public string GetColumnDataForQuery(string columnType, string columnValue)
//        {
//            string result = columnValue;
//            if (columnType == "System.Int32" || columnType == "System.Boolean")
//            {
//                result = columnValue;
//            }
//            else if (columnType == "System.DateTime")
//            {
//                var dateTime = Convert.ToDateTime(columnValue).ToString("yyyy-MM-dd HH:mm:ss");
//                result = dateTime;
//            }

//            return result;
//        }

//        [HttpPost]
//        public ActionResult DeleteRecord(long id, string tablename)
//        {
//            try
//            {
//                // this function will run only on those tables which have isdeleted column
//                string query = $"update {tablename} set isDeleted='true' Where id={id}";

//                var result = 0;
//                string message = "";
//                using (var cmd = db.Database.Connection.CreateCommand())
//                {
//                    cmd.CommandText = query;
//                    if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }
//                    result = cmd.ExecuteNonQuery();
//                    cmd.Connection.Close();
//                }

//                if (result > 0)
//                {
//                    message = "Record has been deleted";
//                }
//                else
//                {
//                    message = "Record deletion failed";
//                }

//                ajaxResponse =
//                  new AjaxSuccess<List<string>>()
//                  {
//                      Message = message,
//                      Response = true,
//                      Status = true
//                  };
//            }
//            catch (Exception e)
//            {
//                ajaxResponse = AjaxError.CatchError(e);
//            }
//            return Json(ajaxResponse);
//        }


//    }



//    public class EditTable
//    {
//        [JsonProperty("columnName")]
//        public string ColumnName { get; set; }
//        [JsonProperty("columnValue")]

//        public string ColumnValue { get; set; }
//        [JsonProperty("columnType")]
//        public string ColumnType { get; set; }
//        [JsonProperty("nullableData")]
//        public string nullableData { get; set; }

//    }

//    public class TableSchema
//    {
//        public string SchemaName { get; set; }
//        public string TableName { get; set; }
//        public string PkName { get; set; }
//        public string ColumnName { get; set; }
//    }

//    public enum Crud
//    {
//        TableColumns
//    }

//}
