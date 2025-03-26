using HR_system.Constants;
using System.Linq.Expressions;

namespace HR_system.BLogic.Templates
{
    public class Query<T>
    {
        public Expression<Func<T, bool>> Expression { get; set; }
        public int Limit { get; set; } = DefaultValuesConsnts.DEFAULT_LIMIT;
        public int Offset { get; set; } = DefaultValuesConsnts.DEFAULT_OFFSET;

    }
}
