using System.Collections.Generic;

namespace PlanA.Architecture.DataBinding
{
    public class FloatDataBind : DataBind<FloatDataBind, float>
    {
        public FloatDataBind(float value = 0) : base(value)
        {
        }
    }

    public class FloatDataBindList : DataBindList<FloatDataBindList, float>
    {
        public FloatDataBindList(IEnumerable<float> initialItems) : base(initialItems)
        {
        }
    }
}