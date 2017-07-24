using Newtonsoft.Json;
using System.Collections.Generic;

namespace System
{
    public static class JsonExtends {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
        /// <summary>
        /// 将json格式的字符串，转换成模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToModel<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }
    }


}

