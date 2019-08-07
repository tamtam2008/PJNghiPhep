[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ProjectNghiPhep.MVCGridConfig), "RegisterGrids")]

namespace ProjectNghiPhep
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using ProjectNghiPhep.Models;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            MVCGridDefinitionTable.Add("WaitingDocumentGrid", new MVCGridBuilder<Document>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                //.WithPaging(true, 5)
                .WithAdditionalQueryOptionNames("search")
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add("code").WithColumnName("code")
                        .WithHeaderText("Mã đơn")
                        .WithValueExpression(i => i.code); // use the Value Expression to return the cell text for this column
                    cols.Add("status").WithColumnName("status")
                        .WithHeaderText("Trạng thái")
                        .WithValueExpression((i, c) => {
                            if (i.status == 0) {
                                return "Chờ duyệt";
                            } else
                                if (i.status == 99)
                                {
                                    return "Đã duyệt";
                                }
                            return "Đã hủy";
                        });
                    cols.Add("createdAt").WithColumnName("createdAt")
                        .WithHeaderText("Thời gian tạo")
                        .WithValueExpression((i, c) =>
                        {
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.createdAt) / 1000d)).ToLocalTime().ToString();
                        });
                   cols.Add("createdBy").WithColumnName("createdBy")
                        .WithHeaderText("Người tạo")
                        .WithValueExpression(i => i.createdBy.username);
                   cols.Add("startDate").WithColumnName("startDate")
                        .WithHeaderText("Ngày bắt dầu")
                        .WithValueExpression((i, c) =>
                        {
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.startDate) / 1000d)).ToLocalTime().ToString();
                        });
                   cols.Add("endDate").WithColumnName("endDate")
                        .WithHeaderText("Ngày kết thúc")
                        .WithValueExpression((i, c) =>
                        {
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.endDate) / 1000d)).ToLocalTime().ToString();
                        });
                   cols.Add("count").WithColumnName("count")
                        .WithHeaderText("Số ngày nghỉ")
                        .WithValueExpression((i, c) =>
                        {
                            int count = (int)((i.endDate - i.startDate) / 1000 / 3600 / 24);
                            if (count == 0) {
                                return "1 ngày";
                            } else {
                                return count.ToString() + " ngày";
                            }
                        });
                   cols.Add("VerifyBtn").WithSorting(false)
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => c.UrlHelper.Action("VerifyDocument", "DonNghiPhep", new { id = p.C_id }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-primary btn-verify' role='button' data-form-method='post'>Duyệt</a>");
                   cols.Add("CancelBtn").WithSorting(false)
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => c.UrlHelper.Action("CancelDocument", "DonNghiPhep", new { id = p.C_id }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-danger btn-cancel'>Hủy</a>");
                    cols.Add("DetailBtn").WithSorting(false)
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => c.UrlHelper.Action("ViewDocument", "DonNghiPhep", new { id = p.C_id }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-primary'>Chi tiết</a>");
                })
                .WithRetrieveDataMethod((context) =>
                {
                    var result = new QueryResult<Document>();
                    NghiphepEntities db = new NghiphepEntities();
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                    var query = (from doc in db.Documents
                                  join user in db.Users
                                  on doc.createdById equals user.C_id
                                  where doc.status == 0
                                  select new 
                                  {
                                      C_id = doc.C_id,
                                      createdBy = user,
                                      code = doc.code,
                                      status = doc.status,
                                      createdAt = doc.createdAt,
                                      startDate = doc.startDate,
                                      endDate = doc.endDate
                                  });
                     System.Diagnostics.Debug.WriteLine(options.GetLimitOffset().HasValue);
                     var documents = query.ToList().Select(r => new Document
                                   {
                                        C_id = r.C_id,
                                        createdBy = r.createdBy,
                                        code = r.code,
                                        status = r.status,
                                        createdAt = r.createdAt,
                                        startDate = r.startDate,
                                        endDate = r.endDate
                                    }).ToList();
                     //System.Diagnostics.Debug.WriteLine(options.GetLimitRowcount().Value);
                     int count = 0;
                     if (options.GetLimitOffset().HasValue)
                     {
                         documents = documents.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
                     }
                     count = documents.Count;
                     return new QueryResult<Document>()
                     {
                        Items = documents,
                        TotalRecords = count // if paging is enabled, return the total number of records of all pages
                     };
                })
            );
            MVCGridDefinitionTable.Add("WaitingDocumentGridEm", new MVCGridBuilder<Document>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                //.WithPaging(true, 5)
                .WithAdditionalQueryOptionNames("search")
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add("code").WithColumnName("code")
                        .WithHeaderText("Mã đơn")
                        .WithValueExpression(i => i.code); // use the Value Expression to return the cell text for this column
                    cols.Add("status").WithColumnName("status")
                        .WithHeaderText("Trạng thái")
                        .WithValueExpression((i, c) =>
                        {
                            if (i.status == 0)
                            {
                                return "Chờ duyệt";
                            }
                            else
                                if (i.status == 99)
                                {
                                    return "Đã duyệt";
                                }
                            return "Đã hủy";
                        });
                    cols.Add("createdAt").WithColumnName("createdAt")
                        .WithHeaderText("Thời gian tạo")
                        .WithValueExpression((i, c) =>
                        {
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.createdAt) / 1000d)).ToLocalTime().ToString();
                        });
                    cols.Add("createdBy").WithColumnName("createdBy")
                         .WithHeaderText("Người tạo")
                         .WithValueExpression(i => i.createdBy.username);
                    cols.Add("startDate").WithColumnName("startDate")
                         .WithHeaderText("Ngày bắt dầu")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.startDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("endDate").WithColumnName("endDate")
                         .WithHeaderText("Ngày kết thúc")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.endDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("count").WithColumnName("count")
                         .WithHeaderText("Số ngày nghỉ")
                         .WithValueExpression((i, c) =>
                         {
                             int count = (int)((i.endDate - i.startDate) / 1000 / 3600 / 24);
                             if (count == 0)
                             {
                                 return "1 ngày";
                             }
                             else
                             {
                                 return count.ToString() + " ngày";
                             }
                         });
                    cols.Add("DetailBtn").WithSorting(false)
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => c.UrlHelper.Action("ViewDocument", "DonNghiPhep", new { id = p.C_id }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-primary'>Chi tiết</a>");
                })
                .WithRetrieveDataMethod((context) =>
                {
                    var result = new QueryResult<Document>();
                    NghiphepEntities db = new NghiphepEntities();
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                    var query = (from doc in db.Documents
                                 join user in db.Users
                                 on doc.createdById equals user.C_id
                                 where doc.status == 0
                                 select new
                                 {
                                     C_id = doc.C_id,
                                     createdBy = user,
                                     code = doc.code,
                                     status = doc.status,
                                     createdAt = doc.createdAt,
                                     startDate = doc.startDate,
                                     endDate = doc.endDate
                                 });
                    System.Diagnostics.Debug.WriteLine(options.GetLimitOffset().HasValue);
                    var documents = query.ToList().Select(r => new Document
                    {
                        C_id = r.C_id,
                        createdBy = r.createdBy,
                        code = r.code,
                        status = r.status,
                        createdAt = r.createdAt,
                        startDate = r.startDate,
                        endDate = r.endDate
                    }).ToList();
                    //System.Diagnostics.Debug.WriteLine(options.GetLimitRowcount().Value);
                    int count = 0;
                    if (options.GetLimitOffset().HasValue)
                    {
                        documents = documents.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
                    }
                    count = documents.Count;
                    return new QueryResult<Document>()
                    {
                        Items = documents,
                        TotalRecords = count // if paging is enabled, return the total number of records of all pages
                    };
                })
            );
            MVCGridDefinitionTable.Add("VerifyDocumentGrid", new MVCGridBuilder<Document>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                //.WithPaging(true, 5)
                .WithAdditionalQueryOptionNames("search")
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add("code").WithColumnName("code")
                        .WithHeaderText("Mã đơn")
                        .WithValueExpression(i => i.code); // use the Value Expression to return the cell text for this column
                    cols.Add("status").WithColumnName("status")
                        .WithHeaderText("Trạng thái")
                        .WithValueExpression((i, c) =>
                        {
                            if (i.status == 0)
                            {
                                return "Chờ duyệt";
                            }
                            else
                                if (i.status == 99)
                                {
                                    return "Đã duyệt";
                                }
                            return "Đã hủy";
                        });
                    cols.Add("createdAt").WithColumnName("createdAt")
                        .WithHeaderText("Thời gian tạo")
                        .WithValueExpression((i, c) =>
                        {
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.createdAt) / 1000d)).ToLocalTime().ToString();
                        });
                    cols.Add("createdBy").WithColumnName("createdBy")
                         .WithHeaderText("Người tạo")
                         .WithValueExpression(i => i.createdBy.username);
                    cols.Add("startDate").WithColumnName("startDate")
                         .WithHeaderText("Ngày bắt dầu")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.startDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("endDate").WithColumnName("endDate")
                         .WithHeaderText("Ngày kết thúc")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.endDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("count").WithColumnName("count")
                         .WithHeaderText("Số ngày nghỉ")
                         .WithValueExpression((i, c) =>
                         {
                             int count = (int)((i.endDate - i.startDate) / 1000 / 3600 / 24);
                             if (count == 0)
                             {
                                 return "1 ngày";
                             }
                             else
                             {
                                 return count.ToString() + " ngày";
                             }
                         });
                })
                .WithRetrieveDataMethod((context) =>
                {
                    var result = new QueryResult<Document>();
                    NghiphepEntities db = new NghiphepEntities();
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                    var query = (from doc in db.Documents
                                 join user in db.Users
                                 on doc.createdById equals user.C_id
                                 where doc.status == 99
                                 select new
                                 {
                                     C_id = doc.C_id,
                                     createdBy = user,
                                     code = doc.code,
                                     status = doc.status,
                                     createdAt = doc.createdAt,
                                     startDate = doc.startDate,
                                     endDate = doc.endDate
                                 });
                    System.Diagnostics.Debug.WriteLine(options.GetLimitOffset().HasValue);
                    var documents = query.ToList().Select(r => new Document
                    {
                        C_id = r.C_id,
                        createdBy = r.createdBy,
                        code = r.code,
                        status = r.status,
                        createdAt = r.createdAt,
                        startDate = r.startDate,
                        endDate = r.endDate
                    }).ToList();
                    //System.Diagnostics.Debug.WriteLine(options.GetLimitRowcount().Value);
                    int count = 0;
                    if (options.GetLimitOffset().HasValue)
                    {
                        documents = documents.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
                    }
                    count = documents.Count;
                    return new QueryResult<Document>()
                    {
                        Items = documents,
                        TotalRecords = count // if paging is enabled, return the total number of records of all pages
                    };
                })
            );
            MVCGridDefinitionTable.Add("VerifyDocumentGridEm", new MVCGridBuilder<Document>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                //.WithPaging(true, 5)
                .WithAdditionalQueryOptionNames("search")
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add("code").WithColumnName("code")
                        .WithHeaderText("Mã đơn")
                        .WithValueExpression(i => i.code); // use the Value Expression to return the cell text for this column
                    cols.Add("status").WithColumnName("status")
                        .WithHeaderText("Trạng thái")
                        .WithValueExpression((i, c) =>
                        {
                            if (i.status == 0)
                            {
                                return "Chờ duyệt";
                            }
                            else
                                if (i.status == 99)
                                {
                                    return "Đã duyệt";
                                }
                            return "Đã hủy";
                        });
                    cols.Add("createdAt").WithColumnName("createdAt")
                        .WithHeaderText("Thời gian tạo")
                        .WithValueExpression((i, c) =>
                        {
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.createdAt) / 1000d)).ToLocalTime().ToString();
                        });
                    cols.Add("createdBy").WithColumnName("createdBy")
                         .WithHeaderText("Người tạo")
                         .WithValueExpression(i => i.createdBy.username);
                    cols.Add("startDate").WithColumnName("startDate")
                         .WithHeaderText("Ngày bắt dầu")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.startDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("endDate").WithColumnName("endDate")
                         .WithHeaderText("Ngày kết thúc")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.endDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("count").WithColumnName("count")
                         .WithHeaderText("Số ngày nghỉ")
                         .WithValueExpression((i, c) =>
                         {
                             int count = (int)((i.endDate - i.startDate) / 1000 / 3600 / 24);
                             if (count == 0)
                             {
                                 return "1 ngày";
                             }
                             else
                             {
                                 return count.ToString() + " ngày";
                             }
                         });
                    cols.Add("DetailBtn").WithSorting(false)
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => c.UrlHelper.Action("ViewDocument", "DonNghiPhep", new { id = p.C_id }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-primary'>Chi tiết</a>");
                })
                .WithRetrieveDataMethod((context) =>
                {
                    var result = new QueryResult<Document>();
                    NghiphepEntities db = new NghiphepEntities();
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                    var query = (from doc in db.Documents
                                 join user in db.Users
                                 on doc.createdById equals user.C_id
                                 where doc.status == 99
                                 select new
                                 {
                                     C_id = doc.C_id,
                                     createdBy = user,
                                     code = doc.code,
                                     status = doc.status,
                                     createdAt = doc.createdAt,
                                     startDate = doc.startDate,
                                     endDate = doc.endDate
                                 });
                    System.Diagnostics.Debug.WriteLine(options.GetLimitOffset().HasValue);
                    var documents = query.ToList().Select(r => new Document
                    {
                        C_id = r.C_id,
                        createdBy = r.createdBy,
                        code = r.code,
                        status = r.status,
                        createdAt = r.createdAt,
                        startDate = r.startDate,
                        endDate = r.endDate
                    }).ToList();
                    //System.Diagnostics.Debug.WriteLine(options.GetLimitRowcount().Value);
                    int count = 0;
                    if (options.GetLimitOffset().HasValue)
                    {
                        documents = documents.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
                    }
                    count = documents.Count;
                    return new QueryResult<Document>()
                    {
                        Items = documents,
                        TotalRecords = count // if paging is enabled, return the total number of records of all pages
                    };
                })
            );
            MVCGridDefinitionTable.Add("CancelDocumentGrid", new MVCGridBuilder<Document>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                //.WithPaging(true, 5)
                .WithAdditionalQueryOptionNames("search")
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add("code").WithColumnName("code")
                        .WithHeaderText("Mã đơn")
                        .WithValueExpression(i => i.code); // use the Value Expression to return the cell text for this column
                    cols.Add("status").WithColumnName("status")
                        .WithHeaderText("Trạng thái")
                        .WithValueExpression((i, c) =>
                        {
                            if (i.status == 0)
                            {
                                return "Chờ duyệt";
                            }
                            else
                                if (i.status == 99)
                                {
                                    return "Đã duyệt";
                                }
                            return "Đã hủy";
                        });
                    cols.Add("createdAt").WithColumnName("createdAt")
                        .WithHeaderText("Thời gian tạo")
                        .WithValueExpression((i, c) =>
                        {
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.createdAt) / 1000d)).ToLocalTime().ToString();
                        });
                    cols.Add("createdBy").WithColumnName("createdBy")
                         .WithHeaderText("Người tạo")
                         .WithValueExpression(i => i.createdBy.username);
                    cols.Add("startDate").WithColumnName("startDate")
                         .WithHeaderText("Ngày bắt dầu")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.startDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("endDate").WithColumnName("endDate")
                         .WithHeaderText("Ngày kết thúc")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.endDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("count").WithColumnName("count")
                         .WithHeaderText("Số ngày nghỉ")
                         .WithValueExpression((i, c) =>
                         {
                             int count = (int)((i.endDate - i.startDate) / 1000 / 3600 / 24);
                             if (count == 0)
                             {
                                 return "1 ngày";
                             }
                             else
                             {
                                 return count.ToString() + " ngày";
                             }
                         });
                    cols.Add("DetailBtn").WithSorting(false)
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => c.UrlHelper.Action("ViewDocument", "DonNghiPhep", new { id = p.C_id }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-primary'>Chi tiết</a>");
                })
                .WithRetrieveDataMethod((context) =>
                {
                    var result = new QueryResult<Document>();
                    NghiphepEntities db = new NghiphepEntities();
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                    var query = (from doc in db.Documents
                                 join user in db.Users
                                 on doc.createdById equals user.C_id
                                 where doc.status == 100
                                 select new
                                 {
                                     C_id = doc.C_id,
                                     createdBy = user,
                                     code = doc.code,
                                     status = doc.status,
                                     createdAt = doc.createdAt,
                                     startDate = doc.startDate,
                                     endDate = doc.endDate
                                 });
                    System.Diagnostics.Debug.WriteLine(options.GetLimitOffset().HasValue);
                    var documents = query.ToList().Select(r => new Document
                    {
                        C_id = r.C_id,
                        createdBy = r.createdBy,
                        code = r.code,
                        status = r.status,
                        createdAt = r.createdAt,
                        startDate = r.startDate,
                        endDate = r.endDate
                    }).ToList();
                    //System.Diagnostics.Debug.WriteLine(options.GetLimitRowcount().Value);
                    int count = 0;
                    if (options.GetLimitOffset().HasValue)
                    {
                        documents = documents.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
                    }
                    count = documents.Count;
                    return new QueryResult<Document>()
                    {
                        Items = documents,
                        TotalRecords = count // if paging is enabled, return the total number of records of all pages
                    };
                })
            );
            MVCGridDefinitionTable.Add("CancelDocumentGridEm", new MVCGridBuilder<Document>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                //.WithPaging(true, 5)
                .WithAdditionalQueryOptionNames("search")
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add("code").WithColumnName("code")
                        .WithHeaderText("Mã đơn")
                        .WithValueExpression(i => i.code); // use the Value Expression to return the cell text for this column
                    cols.Add("status").WithColumnName("status")
                        .WithHeaderText("Trạng thái")
                        .WithValueExpression((i, c) =>
                        {
                            if (i.status == 0)
                            {
                                return "Chờ duyệt";
                            }
                            else
                                if (i.status == 99)
                                {
                                    return "Đã duyệt";
                                }
                            return "Đã hủy";
                        });
                    cols.Add("createdAt").WithColumnName("createdAt")
                        .WithHeaderText("Thời gian tạo")
                        .WithValueExpression((i, c) =>
                        {
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.createdAt) / 1000d)).ToLocalTime().ToString();
                        });
                    cols.Add("createdBy").WithColumnName("createdBy")
                         .WithHeaderText("Người tạo")
                         .WithValueExpression(i => i.createdBy.username);
                    cols.Add("startDate").WithColumnName("startDate")
                         .WithHeaderText("Ngày bắt dầu")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.startDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("endDate").WithColumnName("endDate")
                         .WithHeaderText("Ngày kết thúc")
                         .WithValueExpression((i, c) =>
                         {
                             return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.endDate) / 1000d)).ToLocalTime().ToString();
                         });
                    cols.Add("count").WithColumnName("count")
                         .WithHeaderText("Số ngày nghỉ")
                         .WithValueExpression((i, c) =>
                         {
                             int count = (int)((i.endDate - i.startDate) / 1000 / 3600 / 24);
                             if (count == 0)
                             {
                                 return "1 ngày";
                             }
                             else
                             {
                                 return count.ToString() + " ngày";
                             }
                         });
                    cols.Add("DetailBtn").WithSorting(false)
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => c.UrlHelper.Action("ViewDocument", "DonNghiPhep", new { id = p.C_id }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-primary'>Chi tiết</a>");
                })
                .WithRetrieveDataMethod((context) =>
                {
                    var result = new QueryResult<Document>();
                    NghiphepEntities db = new NghiphepEntities();
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                    var query = (from doc in db.Documents
                                 join user in db.Users
                                 on doc.createdById equals user.C_id
                                 where doc.status == 100
                                 select new
                                 {
                                     C_id = doc.C_id,
                                     createdBy = user,
                                     code = doc.code,
                                     status = doc.status,
                                     createdAt = doc.createdAt,
                                     startDate = doc.startDate,
                                     endDate = doc.endDate
                                 });
                    System.Diagnostics.Debug.WriteLine(options.GetLimitOffset().HasValue);
                    var documents = query.ToList().Select(r => new Document
                    {
                        C_id = r.C_id,
                        createdBy = r.createdBy,
                        code = r.code,
                        status = r.status,
                        createdAt = r.createdAt,
                        startDate = r.startDate,
                        endDate = r.endDate
                    }).ToList();
                    //System.Diagnostics.Debug.WriteLine(options.GetLimitRowcount().Value);
                    int count = 0;
                    if (options.GetLimitOffset().HasValue)
                    {
                        documents = documents.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
                    }
                    count = documents.Count;
                    return new QueryResult<Document>()
                    {
                        Items = documents,
                        TotalRecords = count // if paging is enabled, return the total number of records of all pages
                    };
                })
            );
            MVCGridDefinitionTable.Add("UserGrid", new MVCGridBuilder<User>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                //.WithPaging(true, 5)
                .WithAdditionalQueryOptionNames("search")
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add("username").WithColumnName("username")
                        .WithHeaderText("Mã nhân viên")
                        .WithValueExpression(i => i.username); // use the Value Expression to return the cell text for this column
                    cols.Add("ContractType").WithColumnName("ContractType")
                        .WithHeaderText("Ngày phép")
                        .WithValueExpression((i, c) =>
                        {
                            return i.ContractType.dayOff.ToString();
                        });
                    cols.Add("dateOff").WithColumnName("dateOff")
                       .WithHeaderText("Ngày phép còn lại")
                       .WithValueExpression((i, c) =>
                       {
                           return i.dayOff.ToString();
                       });
                    cols.Add("createdAt").WithColumnName("createdAt")
                        .WithHeaderText("Thời gian tạo")
                        .WithValueExpression((i, c) =>
                        {
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.createdAt) / 1000d)).ToLocalTime().ToString();
                        });
                    cols.Add("ContractType").WithColumnName("ContractTypeName")
                        .WithHeaderText("Hợp đồng")
                        .WithValueExpression((i, c) =>
                        {
                            return i.ContractType.name;
                        });
                    cols.Add("VerifyBtn").WithSorting(false)
                         .WithHeaderText("")
                         .WithHtmlEncoding(false)
                         .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "NguoiDung", new { id = p.C_id }))
                         .WithValueTemplate("<a href='{Value}' class='btn btn-primary' role='button' data-form-method='post'>Sửa</a>");
                    cols.Add("CancelBtn").WithSorting(false)
                         .WithHeaderText("")
                         .WithHtmlEncoding(false)
                         .WithValueExpression((p, c) => c.UrlHelper.Action("DeleteUser", "NguoiDung", new { id = p.C_id }))
                         .WithValueTemplate("<a href='{Value}' class='btn btn-danger'>Xóa</a>");
                })
                .WithRetrieveDataMethod((context) =>
                {
                    var result = new QueryResult<User>();
                    NghiphepEntities db = new NghiphepEntities();
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                    var query = (from user in db.Users
                                 join contract in db.ContractTypes
                                 on user.contractId equals contract.C_id
                                 where user.isActive == true
                                 select new
                                 {
                                     C_id = user.C_id,
                                     username = user.username,
                                     dayOff = user.dayOff,
                                     createdAt = user.createdAt,
                                     ContractType = contract
                                 });
                    System.Diagnostics.Debug.WriteLine(options.GetLimitOffset().HasValue);
                    var users = query.ToList().Select(r => new User
                    {
                        C_id = r.C_id,
                        username = r.username,
                        dayOff = r.dayOff,
                        createdAt = r.createdAt,
                        ContractType = r.ContractType
                    }).ToList();
                    //System.Diagnostics.Debug.WriteLine(options.GetLimitRowcount().Value);
                    int count = 0;
                    //if (options.GetLimitOffset().HasValue)
                    //{
                    //    documents = documents.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
                    //}
                    System.Diagnostics.Debug.WriteLine(users.Count);
                    count = users.Count;
                    return new QueryResult<User>()
                    {
                        Items = users,
                        TotalRecords = count // if paging is enabled, return the total number of records of all pages
                    };
                })
            );
        }
    }
}