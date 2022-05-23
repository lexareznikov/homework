using homework.helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace homework.Static.EPPlus
{
    public static class Extensions
    {
        public static IEnumerable<T> ConvertSheetToObjects<T>(this ExcelWorksheet worksheet) where T : class, new()
        {

            Func<CustomAttributeData, bool> columnOnly = y => y.AttributeType == typeof(Column);

            //var reqCol = typeof(T)
            //    .GetProperties()
            //    .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(RequiredAttribute)));

            var columns = typeof(T)
                    .GetProperties()
                    .Where(x => x.CustomAttributes.Any(columnOnly))
            .Select(p => new
            {
                Property = p,
                Column = p.GetCustomAttributes<Column>().First().ColumnIndex

            }).ToList();


            var rows = worksheet.Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);


            var collection = rows.Skip(1)
                .Select(row =>
                {
                    int nullVuleCount = 0;
                    var tnew = new T();
                    columns.ForEach(col =>
                    {
                        //var val1 = worksheet.Cells[];

                        var val = worksheet.Cells[row, col.Column];

                        //if (val.Value == null)
                        //{
                        //    col.Property.SetValue(tnew, null);
                        //    return;
                        //}
                        if (val.Value == null || string.IsNullOrWhiteSpace(val.GetValue<string>()))
                        {
                            col.Property.SetValue(tnew, null);
                            nullVuleCount++;
                            return;
                        }
                        if (col.Property.PropertyType == typeof(Int32))
                        {
                            col.Property.SetValue(tnew, val.GetValue<int>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(DateTime))
                        {
                            col.Property.SetValue(tnew, val.GetValue<DateTime>());
                            return;
                        }

                        col.Property.SetValue(tnew, val.GetValue<string>().Trim());
                    });

                    if (nullVuleCount == columns.Count)
                        tnew = null;
                    return tnew;
                    //if (nullVuleCount != columns.Count)
                    //    return tnew;
                });

            return collection;
        }
    }
}
