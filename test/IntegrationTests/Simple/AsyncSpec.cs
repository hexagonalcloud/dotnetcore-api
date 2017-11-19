using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests.Simple
{
    public abstract class AsyncSpec
    {
        private string _given = string.Empty;
        private string _when = string.Empty;

        protected AsyncSpec()
        {
            Given();
            When();
        }

        private void Given()
        {
            var type = this.GetType();
            var methodInfos = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(GivenAttribute), false).Length > 0);
            foreach (var methodInfo in methodInfos)
            {
                var task = (Task)methodInfo.Invoke(this, null);
                task.Wait();

                if (methodInfo.GetCustomAttributes(typeof(GivenAttribute), false).First() is GivenAttribute attribute)
                {
                    _given = attribute.Description;
                }
            }
        }

        private void When()
        {
            var type = this.GetType();
            var methodInfos = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(WhenAttribute), false).Length > 0);
            foreach (var methodInfo in methodInfos)
            {
                var task = (Task)methodInfo.Invoke(this, null);
                task.Wait();

                if (methodInfo.GetCustomAttributes(typeof(WhenAttribute), false).First() is  WhenAttribute attribute)
                {
                    _when = attribute.Description;
                }
            }
        }
    }
}
