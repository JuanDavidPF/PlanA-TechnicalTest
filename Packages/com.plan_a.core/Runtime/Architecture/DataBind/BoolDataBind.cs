using System.Collections.Generic;

namespace PlanA.Architecture.DataBinding
{
    public class BoolDataBind : DataBind<BoolDataBind, bool>
    {
        public BoolDataBind(bool initialValue = false) : base(initialValue)
        {
        }
    }

    public class BoolListDataBind : DataBindList<BoolListDataBind, bool>
    {
        public BoolListDataBind(IEnumerable<bool> initialItems) : base(initialItems)
        {
        }
    }
}