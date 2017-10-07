using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;

namespace WebApplication1.Helpers
{
    public static class JSONHelper
    {
        //Extended method of object class, converts an object to JSON string
        public static string ToJSON(this object obj)
        {
            var serializer = new JavaScriptSerializer();
            try
            {
                return serializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}