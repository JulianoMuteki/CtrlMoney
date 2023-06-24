using CtrlMoney.CrossCutting.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlMoney.CrossCutting
{
    public static class MovementTypeList
    {
        public static IDictionary<MovimentType, string> MovimentsTypes => GetMoviments();

        private static IDictionary<MovimentType, string> GetMoviments()
        {
            IDictionary<MovimentType, string> keyValuePairs = new Dictionary<MovimentType, string>();

            foreach (MovimentType movimentType in Enum.GetValues(typeof(MovimentType)))
            {
                string description = GetEnumDescription(movimentType);
                keyValuePairs.Add(movimentType, description);
            }

            return keyValuePairs;
        }

        static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}
