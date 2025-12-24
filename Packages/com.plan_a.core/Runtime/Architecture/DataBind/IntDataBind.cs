using System.Collections.Generic;

namespace PlanA.Architecture.DataBinding
{
    public class IntDataBind : DataBind<IntDataBind, int>
    {
        public IntDataBind(int value = 0) : base(value)
        {
        }
    }

    public class IntDataBindList : DataBindList<IntDataBindList, int>
    {
        public IntDataBindList(IEnumerable<int> initialItems) : base(initialItems)
        {
        }
    }
}