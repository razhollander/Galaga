using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services.SerializerService.Serializers.Csv
{
    public class CsvSerializer
    {
        public T[] DeserializeCsv<T>(string csv)
        {
            var type = typeof(T);
            var fields = type.GetFields();
            var list = new List<T>();

            using (var reader = new StringReader(csv))
            {
                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    var items = line.Split(',');
                    var header = items[0].Replace(" ", "_");
                    var isHeaderExist = fields.Any(i => i.Name == header);

                    if (isHeaderExist)
                    {
                        for (var i = 1; i < items.Length; i++)
                        {
                            T t;
                            var listIndex = i - 1;

                            if (listIndex < list.Count)
                            {
                                t = list[listIndex];
                            }
                            else
                            {
                                t = Activator.CreateInstance<T>();
                                list.Add(t);
                            }

                            var item = items[i];

                            var concreteFieldInfo = fields.First(f => f.Name == header);

                            if (!string.IsNullOrEmpty(item))
                            {
                                concreteFieldInfo.SetValue(t, GetConcreteValue(item, concreteFieldInfo.FieldType));
                            }
                        }
                    }
                }
            }

            return list.ToArray();

            object GetConcreteValue(string item, Type fieldType)
            {
                if (typeof(string) == fieldType)
                {
                    return item;
                }

                if (typeof(int) == fieldType)
                {
                    return int.Parse(item);
                }

                if (typeof(float) == fieldType)
                {
                    return float.Parse(item);
                }

                if (typeof(double) == fieldType)
                {
                    return double.Parse(item);
                }

                if (typeof(bool) == fieldType)
                {
                    return bool.Parse(item);
                }

                throw new NotImplementedException();
            }
        }
    }
}