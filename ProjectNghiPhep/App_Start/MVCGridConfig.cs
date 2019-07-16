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
            MVCGridDefinitionTable.Add("DonNghiPhepGrid", new MVCGridBuilder<Document>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                .WithPaging(paging: true, itemsPerPage: 10, allowChangePageSize: true, maxItemsPerPage: 100)
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
                            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(i.createdAt) / 1000d)).ToLocalTime().ToLongDateString();
                        });
                   cols.Add("createdBy").WithColumnName("createdBy")
                        .WithHeaderText("Người tạo")
                        .WithValueExpression(i => i.createdBy.username);
                })
                .WithRetrieveDataMethod((context) =>
                {
                    NghiphepEntities db = new NghiphepEntities();
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var query = (from doc in db.Documents
                                  join user in db.Users
                                  on doc.createdById equals user.C_id
                                  select new 
                                  {
                                      C_id = doc.C_id,
                                      createdBy = user,
                                      code = doc.code,
                                      status = doc.status,
                                      createdAt = doc.createdAt
                                  });
                     var documents = query.ToList().Select(r => new Document
                                   {
                                        C_id = r.C_id,
                                        createdBy = r.createdBy,
                                        code = r.code,
                                        status = r.status,
                                        createdAt = r.createdAt
                                    }).ToList();
                    return new QueryResult<Document>()
                    {
                        Items = documents,
                        TotalRecords = db.Documents.Count() // if paging is enabled, return the total number of records of all pages
                    };

                })
            );
        }
    }
}