using System.Collections.Generic;

namespace PlanA.Architecture.DataBinding
{
    public class StringDataBind : DataBind<StringDataBind, string>
    {
        public StringDataBind(string value = null) : base(value)
        {
        }
    }

    public class StringDataBindList : DataBindList<StringDataBindList, string>
    {
        public StringDataBindList(IEnumerable<string> initialItems) : base(initialItems)
        {
        }
    }
}